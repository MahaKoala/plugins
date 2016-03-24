using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UlteriusPluginBase
{

    /// <summary>
    /// Base class for a plugin
    /// </summary>
   
    public abstract class PluginBase : MarshalByRefObject
    {
       /// <summary>
        /// Plugin's name
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Plugin's author
        /// </summary>
        public virtual string Author { get; protected set; }

        /// <summary>
        /// Plugin's website
        /// </summary>
        public virtual string Website { get; protected set; }


        /// <summary>
        /// Plugin's GUID
        /// </summary>
        public virtual Guid GUID { get; protected set; }


        /// <summary>
        /// Plugin's icon
        /// </summary>
        public virtual string Icon { get; protected set; }

        /// <summary>
        /// Plugin's CanonicalName 
        /// </summary>
        public virtual string CanonicalName { get; protected set; }

        /// <summary>
        /// Plugin's description
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// Base64 encoded javascript to be loaded on the frontend
        /// </summary>
        public virtual string Javascript { get; protected set; }

        /// <summary>
        /// Plugin's Version 
        /// </summary>
        public virtual double Version { get; protected set; }

        /// <summary>
        /// IF true then we call Setup(); to do things on load.
        /// </summary>
        public virtual bool RequiresSetup { get; protected set; }

        /// <summary>
        /// Plugin's configuration
        /// </summary>
        public virtual ConfigurationBase Configuration { get; set; }


        /// <summary>
        /// So we can expose the main tray icon to all plugins, they will still need permission to access the UI.
        /// </summary>
        public virtual NotifyIcon NotificationIcon { get; set; }

        /// <summary>
        /// Actually run our code
        /// </summary>
        public abstract object Start(List<object> args = null);

        /// <summary>
        /// Run setup code, there is no return value.
        /// </summary>
        public abstract void Setup();

        /// <summary>
        /// Plugin's Event Handler
        /// </summary>
        public virtual EventHandler PluginEvent { get; set; }



        /// <summary>
        /// Default constructor
        /// </summary>
        public PluginBase()
        {
            Name = "Unnamed plugin";
            Description = "No description";
        }

    }
}
