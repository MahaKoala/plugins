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
{"endpoint":"plugin","syncKey":null,"results":{"pluginData":{"someNumber":499,"someString":"Hello World","someBool":true,"specialArray":["one","two","three"]},"pluginStarted":true}}
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
```
{"endpoint":"plugin","syncKey":null,"results":{"pluginData":{"textData":"Hello World! I am a text file!"},"pluginStarted":true}}
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

```
var packet = {
endpoint: 'getplugins',
apiKey: 'XLXffaxpny4LsHyRt9KM8pO3nSKM85adZXi'
}
```

Produces
```
{"endpoint":"getplugins","syncKey":null,"results":{"dffb3f56-9323-4fc9-bee9-20d01e0231f0":{"Name":"My Awesome Plugin","Version":1,"Author":"Andrew","Website":"https://twitter.com/AndrewMD5","Description":"A Really Awesome Plugin!","GUID":"dffb3f56-9323-4fc9-bee9-20d01e0231f0"}}}
```

You can also see if it failed to load by doing

```
var packet = {
endpoint: 'getbadplugins',
apiKey: 'XLXffaxpny4LsHyRt9KM8pO3nSKM85adZXi'
}
```

Which produces
```
{"endpoint":"getbadplugins","syncKey":null,"results":["BadPlugin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"]}
```
