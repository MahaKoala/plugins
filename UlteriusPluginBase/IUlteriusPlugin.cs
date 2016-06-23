#region

using System.Collections.Generic;
using System.Windows.Forms;
using vtortola.WebSockets;

#endregion

namespace UlteriusPluginBase
{
    public interface IUlteriusPlugin
    {
       
        /// <summary>
        ///     Plugin's website
        /// </summary>
        string Website { get; set; }

        /// <summary>
        ///     Plugin's icon
        /// </summary>
        string Icon { get; set; }

        /// <summary>
        ///     Base64 encoded javascript to be loaded on the frontend
        /// </summary>
        string Javascript { get; set; }

        /// <summary>
        ///     IF true then we call Setup(); to do things on load.
        /// </summary>
        bool RequiresSetup { get; set; }

        /// <summary>
        ///     So we can expose the main tray icon to all plugins, they will still need permission to access the UI.
        /// </summary>
        NotifyIcon NotificationIcon { get; set; }

        /// <summary>
        ///     Actually run our code
        /// </summary>
        void Start(WebSocket client, List<object> args = null);

        /// <summary>
        ///     Run setup code, there is no return value.
        /// </summary>
        void Initialize();
        void Dispose();
    }
}