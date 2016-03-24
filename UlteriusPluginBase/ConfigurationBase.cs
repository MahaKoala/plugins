using System;
using System.Configuration;

namespace UlteriusPluginBase
{
    /// <summary>
    /// Base configuration class for a plugin
    /// </summary>
    public abstract class ConfigurationBase : ConfigurationSection
    {
        /// <summary>
        /// Opens the configuration section with the specified type and name
        /// </summary>
        /// <param name="sectionName">Configuration section's name</param>
        /// <param name="configPath">Configuration file's path</param>
        /// <returns>Instance of the configuration section's class</returns>
        public static T Open<T>(string sectionName, string configPath) where T : ConfigurationBase, new()
        {
            T instance = new T();
            if (configPath.EndsWith(".config", StringComparison.InvariantCultureIgnoreCase))
                configPath = configPath.Remove(configPath.Length - 7);
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(configPath);
                /* section not found */
                if (config.GetSection(sectionName) == null)
                {
                    config.Sections.Add(sectionName, instance);

                    foreach (ConfigurationProperty p in instance.Properties)
                        ((T)config.GetSection(sectionName)).SetPropertyValue(p, p.DefaultValue, true);

                    config.Save();
                }
                else
                    /* section already exists */
                    instance = (T)config.Sections[sectionName];
            }
            catch (ConfigurationErrorsException)
            {
                if (instance == null)
                    instance = new T();
            }
            return instance;
        }
    }
}