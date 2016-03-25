using System.Configuration;
using UlteriusPluginBase;

namespace SpotifySampler.Logic
{
    public sealed class LogicConfiguration : ConfigurationBase

    {
        [ConfigurationProperty("LogFile", DefaultValue = "spotify.txt")]
        public string FileName
        {
            get { return (string) this["LogFile"]; }
            set { this["LogFile"] = value; }
        }
    }
}