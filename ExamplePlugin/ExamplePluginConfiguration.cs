using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;

namespace ExamplePlugin
{
    public sealed class ExamplePluginConfiguration : UlteriusPluginBase.ConfigurationBase
    {
        /// <summary>
        /// Text file's name
        /// </summary>
        [ConfigurationProperty("FileName", DefaultValue = "textFile.txt")]
        public string FileName
        {
            get
            {
                return (String)this["FileName"];
            }
            set
            {
                this["FileName"] = value;
            }
        }
    }
}
