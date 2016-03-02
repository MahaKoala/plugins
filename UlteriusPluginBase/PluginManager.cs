using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace UlteriusPluginBase
{
    /// <summary>
    /// Plugin manager class to interact with the host application
    /// </summary>
    [Serializable]
    public sealed class PluginManager 
    {
        // Dictionary that contains instances of assemblies for loaded plugins
        private readonly Dictionary<Assembly, PluginBase> plugins;

        /// <summary>
        /// Default constructor. Plugins will be loaded into the same application domain as the host application
        /// </summary>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        public PluginManager()
        {
            plugins = new Dictionary<Assembly, PluginBase>();
            // There are no any references to plugins assemblies in the project.
            // That's why the ConfigurationErrorsException will be thrown during
            // loading of the plugin's configuration. Actually this happens
            // because an assembly defined in a configuration file couldn't be resolved.
            // We need to manually resolve that assembly.
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        /// <summary>
        /// Factory method that creates PluginManager's instance with limited permission set. Plugins will be loaded into the sandboxed application domain
        /// </summary>
        /// <param name="grantSet">Permission set to grant</param>
        /// <returns>Instance of the PluginManager's class</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        public static PluginManager GetInstance(PermissionSet grantSet)
        {
            if (grantSet == null)
                throw new ArgumentNullException("grantSet");

            /* Grant "base" permissions */
            grantSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
            grantSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));

            /* Note that the SetupInformation is the same, i.e. ApplicationBase is the same */
            var sandbox = AppDomain.CreateDomain("sandbox", null, AppDomain.CurrentDomain.SetupInformation, grantSet, getStrongName(Assembly.GetExecutingAssembly()));

            return Activator.CreateInstanceFrom(sandbox, typeof(PluginManager).Assembly.ManifestModule.FullyQualifiedName, typeof(PluginManager).FullName).Unwrap() as PluginManager;
        }
        
        /// <summary>
        /// Loads a plugin
        /// </summary>
        /// <param name="fullName">Full path to a plugin's file</param>
        /// <returns>Instance of the loaded plugin's class</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        public PluginBase LoadPlugin(string fullName)
        {
            Assembly pluginAssembly;
            try
            {
                new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, fullName).Assert();
                pluginAssembly = AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(fullName));
            }
            catch (BadImageFormatException)
            {
                /* Skip not managed dll files */
                return null;
            }
            finally
            {
                CodeAccessPermission.RevertAssert();
            }

            var pluginType = pluginAssembly.GetTypes().FirstOrDefault(x => x.BaseType == typeof(PluginBase));
            if (pluginType == null)
                throw new InvalidOperationException("Plugin's type has not been found in the specified assembly!");
            var pluginInstance = Activator.CreateInstance(pluginType) as PluginBase;
            plugins.Add(pluginAssembly, pluginInstance);

            var pluginConfigurationType = pluginAssembly.GetTypes().FirstOrDefault(x => x.BaseType == typeof(ConfigurationBase));

            if (pluginConfigurationType != null)
            {
                string processPath = String.Empty;
                try
                {
                    new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
                    processPath = Process.GetCurrentProcess().MainModule.FileName;
                }
                finally
                {
                    CodeAccessPermission.RevertAssert();
                }

                try
                {

                    var pset = new PermissionSet(PermissionState.None);
                    pset.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
                    pset.AddPermission(new ConfigurationPermission(PermissionState.Unrestricted));
                    pset.Assert();

                    if (pluginInstance != null)
                        pluginInstance.Configuration =
                            typeof(ConfigurationBase)
                                .GetMethod("Open")
                                .MakeGenericMethod(pluginConfigurationType)
                                .Invoke(null, new object[] { Path.GetFileNameWithoutExtension(fullName), processPath }) as ConfigurationBase;
                }
                finally
                {
                    CodeAccessPermission.RevertAssert();
                }
            }
            return pluginInstance;
        }

        /// <summary> 
        /// Get a strong name that matches the specified assembly. 
        /// </summary> 
        /// <exception cref="ArgumentNullException">
        /// if <paramref name="assembly"/> is null 
        /// </exception> 
        /// <exception cref="InvalidOperationException">
        /// if <paramref name="assembly"/> does not represent a strongly named assembly
        /// </exception> 
        /// <param name="assembly">Assembly to create a StrongName for</param>
        /// <returns>A StrongName for the given assembly</returns> 
        private static StrongName getStrongName(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            AssemblyName assemblyName = assembly.GetName();

            // Get the public key blob. 
            byte[] publicKey = assemblyName.GetPublicKey();
            if (publicKey == null || publicKey.Length == 0)
                throw new InvalidOperationException("Assembly is not strongly named");

            StrongNamePublicKeyBlob keyBlob = new StrongNamePublicKeyBlob(publicKey);

            // Return the strong name. 
            return new StrongName(keyBlob, assemblyName.Name, assemblyName.Version);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return plugins.Keys.FirstOrDefault(x => x.FullName == args.Name);
        }
    }
}
