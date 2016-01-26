# ulterius-plugins
Write and create plugins for the Ulterius server

## Creating an Ulterius Plugin

### Create a new project in Visual Studios

![image](http://i.andrew.im/1ir472.png)

### Add the Ulterius DLL to your project

//TODO put on nuget

Download from [here](https://github.com/StrikeOrg/ulterius-plugins/releases)

Here is an example of a valid plugin

```csharp
using System;
using UlteriusPlugins;

namespace MyUlteriusPlugin
{
    public class MyAwesomePlugin : IPlugin
    {
        #region IPlugin Members

        //Required members
        public string Name => "My Awesome Plugin";
        public double Version => 1.0;
        public string Author => "Andrew";
        public string Website => "https://twitter.com/AndrewMD5";
        public string Description => "A Really Awesome Plugin!";
        public Guid GUID => new Guid("dffb3f56-9323-4fc9-bee9-20d01e0231f0");
        public string Icon => ImageToBase64String(Resources.mario.ToBitmap(), ImageFormat.Png);
        public string CanonicalName => "com.net.mario.andrew";
        
        
        public string ImageToBase64String(Image image, ImageFormat format)
        {
            MemoryStream memory = new MemoryStream();
            image.Save(memory, format);
            string base64 = Convert.ToBase64String(memory.ToArray());
            memory.Close();

            return base64;
        }

        public object Start()
        {
            return null;
        }

        public object Start(object args)
        {
            return null;
        }

        #endregion
    }
}
```

*You'll need to make sure your Guid is unique, as this is how plugins are identified*

### Returning data

Plugin entry points are treated as objects, you're free to extend your plugins functionality as far as you'd like, but returning data is how data is passed through the API. 
//TODO Improve this

If your plugin requires no arguments, ```Start();``` function will be called, otherwise ```Start(args);``` will be called

Here are some examples
```csharp
 public object Start()
        {
            var someNumber = 204 + 295;
            var someString = "Hello World";
            var someBool = true;
            var someArray = new string[] { "one", "two", "three" };
            var pluginResponse = new
            {
                someNumber,
                someString,
                someBool,
                specialArray = someArray
            };
            return pluginResponse;
        }
```

This would return the following data to the API

```json
{
   "endpoint":"plugin",
   "syncKey":null,
   "results":{
      "pluginData":{
         "someNumber":499,
         "someString":"Hello World",
         "someBool":true,
         "specialArray":[
            "one",
            "two",
            "three"
         ]
      },
      "pluginStarted":true
   }
}
```

Alternatively, you can have arguments.

```csharp
 public object Start(List<object> args)
        {
            //While args is listed as an object, it is passed from the server as a List<object>
            var fileText = File.ReadAllText(args.First().ToString());
            var pluginResponse = new
            {
                textData = fileText,
            };
            return pluginResponse;
        }
```

Returns 
```json
{
   "endpoint":"plugin",
   "syncKey":null,
   "results":{
      "pluginData":{
         "textData":"Hello World! I am a text file!"
      },
      "pluginStarted":true
   }
}
```

### Running your plugin
Drag and drop your DLL into (/data/plugins/)

After installing your plugin on the server, you can structure like such to call your plugin

```javascript
var packet = {
endpoint: 'plugin',
apiKey: 'XLXffaxpny4LsHyRt9KM8pO3nSKM85adZXi',
args: ['dffb3f56-9323-4fc9-bee9-20d01e0231f0', 'c:/users/andrew/desktop/hop.ahk' ]
}
```
*All plugins must have the Guid as the first argument, all others are optional*

*AS of now all plugins require authentication*

### Verify your plugin is loaded

```javascript
var packet = {
endpoint: 'getplugins',
apiKey: 'XLXffaxpny4LsHyRt9KM8pO3nSKM85adZXi'
}
```

Produces
```json
{
   "endpoint":"getplugins",
   "syncKey":null,
   "results":{
      "dffb3f56-9323-4fc9-bee9-20d01e0231f0":{
         "Name":"My Awesome Plugin",
         "Version":1,
         "Author":"Andrew",
         "Website":"https://twitter.com/AndrewMD5",
         "Description":"A Really Awesome Plugin!",
         "GUID":"dffb3f56-9323-4fc9-bee9-20d01e0231f0",
         "Icon":"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",
         "CanonicalName":"com.net.mario.andrew"
      }
   }
}
```

You can also see if it failed to load by doing

```javascript
var packet = {
endpoint: 'getbadplugins',
apiKey: 'XLXffaxpny4LsHyRt9KM8pO3nSKM85adZXi'
}
```

Which produces
```json
{
   "endpoint":"getbadplugins",
   "syncKey":null,
   "results":[
      "FirstPlugin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
   ]
}
```
