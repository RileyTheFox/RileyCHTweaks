/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Buttplug.Client;
using Buttplug.Server;
using UnityEngine;

namespace PlugHero
{
    public class CHPlug
    {

        public ButtplugClient Client { get; }
        public ButtplugEmbeddedConnector Connector { get; }
        public ButtplugServer Server { get; }

        public async void Run()
        {
            try
            {
                await Client.ConnectAsync();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Can't connect to Buttplug Server, exiting! Message: {ex.InnerException.Message}");
                return;
            }
        }

        public CHPlug()
        {
            Connector = new ButtplugEmbeddedConnector("CHPlug Server", 500);
            Client = new ButtplugClient("CHPlug Client", Connector);

            Server = Connector.Server;

            Client.DeviceAdded += (aObj, aDeviceEventArgs) =>
                Debug.Log($"Device {aDeviceEventArgs.Device.Name} Connected!");

            Client.DeviceRemoved += (aObj, aDeviceEventArgs) =>
                Debug.Log($"Device {aDeviceEventArgs.Device.Name} Removed!");

            new Thread(Run).Start();
        }

    }
}
*/