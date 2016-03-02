using System;
using System.Linq;
using System.Collections.Generic;

namespace UlteriusPluginBase
{

    /// <summary>
    /// Base class for a plugin
    /// </summary>
    [Serializable]
    public abstract class PluginBase 
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
        /// Plugin's configuration
        /// </summary>
        public virtual ConfigurationBase Configuration { get; set; }

        /// <summary>
        /// Actually run our code
        /// </summary>
        public abstract object Start();
        public abstract object Start(List<object> args);

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
