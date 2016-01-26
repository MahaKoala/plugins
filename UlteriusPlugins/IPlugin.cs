using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UlteriusPlugins
{
    public interface IPlugin
    {
        string Name { get; }
        double Version { get; }
        string Author { get; }
        string Website { get; }
        string Description { get; }
        Guid GUID { get; }
        string Icon { get; }
        string CanonicalName { get; }
        object Start();
        object Start(List<object> args);
        
    }
}
