using System.Net.NetworkInformation;
using System.Net.Sockets;
using Component;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screen
{
    public class SyncScreen : Page
    {
        [SerializeField] private Button start;
        [SerializeField] private TMP_InputField address;

        private void Start()
        {
            address.text = GetLocalIPAddress();

            start.onClick.AddListener(Move2Main);
            Communicator.Start();
        }

        private string GetLocalIPAddress()
        {
            var ipAddress = "";
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var iface in interfaces)
            {
                // Consider only the interfaces that are up and connected to a network
                if (iface.OperationalStatus == OperationalStatus.Up &&
                    iface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    var addresses = iface.GetIPProperties().UnicastAddresses;
                    foreach (var address in addresses)
                    {
                        // Consider only IPv4 addresses
                        if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                            continue;

                        ipAddress = address.Address.ToString();
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(ipAddress))
                    break;
            }

            return ipAddress;
        }

        private void OnDestroy() => 
            start.onClick.RemoveListener(Move2Main);

        private static void Move2Main() => 
            Navigator.Open(Enum.Screen.Main);
    }
}
