using System.Configuration;
using UlteriusPluginBase;

namespace SpotifySampler.Logic
{
    public sealed class LogicConfiguration : ConfigurationBase
    {
        /// <summary>
        ///     We can configure the log file name here
        /// </summary>
        [ConfigurationProperty("LogFile", DefaultValue = "spotify.txt")]
        public string FileName
        {
            get { return (string) this["LogFile"]; }
            set { this["LogFile"] = value; }
        }
    }
}