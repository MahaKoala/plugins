using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using ExamplePlugin.Properties;
using UlteriusPluginBase;

namespace ExamplePlugin
{


    public sealed class ExamplePlugin : PluginBase
    {

        public ExamplePlugin()
        {
            //Required members
            Name = "My Awesome Plugin";
            Version = 1.0;
            Author = "Andrew";
            Website = "https://twitter.com/AndrewMD5";
            Description = "A Really Awesome Plugin!";
            GUID = new Guid("dffb3f56-9323-4fc9-bee9-20d01e0231f0");
            Icon = ImageToBase64String(Resources.mario.ToBitmap(), ImageFormat.Png);
            CanonicalName = "com.net.mario.andrew";
        }

        public string ImageToBase64String(Image image, ImageFormat format)
        {
            var memory = new MemoryStream();
            image.Save(memory, format);
            var base64 = Convert.ToBase64String(memory.ToArray());
            memory.Close();

            return base64;
        }

     
        public override object Start()
        {


            string html = String.Empty;
            using (var wc = new WebClient())
            {
                html = wc.DownloadString("https://api.ipify.org/");
            }
            using (var sw = new FileInfo(((ExamplePluginConfiguration) Configuration).FileName).CreateText())
            {
                sw.WriteLine($"Plugin {Name} has run!");
                sw.WriteLine(html);
            }
            const int someNumber = 204 + 295;
            const string someString = "Mario was here";
            const bool someBool = true;
            var someArray = new[] {"one", "two", "three"};

         

            return new
            {
                someNumber,
                someString,
                someBool,
                specialArray = someArray
            };
        }
       
        public override object Start(List<object> args)
        {
            throw new NotImplementedException();
        }
    }
}