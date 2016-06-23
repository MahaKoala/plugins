#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using UlteriusPluginBase;
using vtortola.WebSockets;

#endregion

namespace ExamplePlugin
{
    public class MyExamplePlugin : IUlteriusPlugin
    {
        private WebSocket _client;


        public MyExamplePlugin()
        {
            RequiresSetup = true;
            Website = "https://andrew.im";
        }

        public string Website { get; set; }
        public string Icon { get; set; }
        public string Javascript { get; set; }
        public bool RequiresSetup { get; set; }
        public NotifyIcon NotificationIcon { get; set; }

        public void Start(WebSocket client, List<object> args = null)
        {
            Console.WriteLine("Starting my plugin");
            _client = client;
            var backgroundWorker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true
            };
            backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            backgroundWorker.RunWorkerAsync();
        }

        public void Initialize()
        {
            Console.WriteLine("Do Something");
        }

        public void Dispose()
        {
            //When this call is made, handle your dispose logic, the instance will be removed after.
            Console.WriteLine("Disposing");
        }


        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                _client.WriteStringAsync("{\"firstName\":\"John\"}", CancellationToken.None);
                throw new Exception("Ass hole");
            }
        }
    }
}