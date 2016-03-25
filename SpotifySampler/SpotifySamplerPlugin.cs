using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using SpotifySampler.Logic;
using SpotifySampler.Properties;
using UlteriusPluginBase;

namespace SpotifySampler
{
    public sealed class SpotifySamplerPlugin : PluginBase
    {
        public SpotifySamplerPlugin()
        {
            Name = "Spotify Sampler";
            Version = 1.0;
            Author = "Andrew Sampson";
            Website = "https://twitter.com/AndrewMD5";
            Description = "Search Spotify for sample songs";
            GUID = new Guid("e64109e0-38a5-42dd-b757-66c3bc72f38d");
            CanonicalName = "com.net.spotify";
            Icon = ImageToBase64String(Resources.spotify.ToBitmap(), ImageFormat.Png);
            
        }


        private void CountForever()
        {
            int i = 0;
            while (true)
            {
                i++;
               // PluginEvent?.Invoke(this, EventArgs.Empty);
            }
        }
        public override object Start(List<object> args = null)
        {
            if (args == null) return null;
            var spotify = new SpotifyLogic();
            var data = spotify.Search(args[0].ToString()).Result;
            var saveLog = (bool) args[1];
            if (saveLog)
            {
                using (var sw = new FileInfo(((LogicConfiguration) Configuration).FileName).CreateText())
                {
                    sw.WriteLine($"Plugin {Name} has run!");

                }


            }
            return data;
        }

        public override void Setup()
        {
            
        }

        #region utils

        public string ImageToBase64String(Image image, ImageFormat format)
        {
            var memory = new MemoryStream();
            image.Save(memory, format);
            var base64 = Convert.ToBase64String(memory.ToArray());
            memory.Close();
            return base64;
        }

        #endregion
    }
}