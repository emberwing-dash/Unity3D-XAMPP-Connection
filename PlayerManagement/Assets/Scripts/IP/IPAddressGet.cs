using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Net.NetworkInformation;

public class IPAddressGet : MonoBehaviour
{
    [SerializeField] private Text ipText; // Legacy UI Text

    private void Start()
    {
        GetWifiIP();
    }

    public void GetWifiIP()
    {
        string wifiIP = "Not found";

        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            // Only active Wi-Fi adapter
            if (ni.OperationalStatus == OperationalStatus.Up &&
                ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            {
                foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        wifiIP = ip.Address.ToString();
                        break;
                    }
                }
            }
        }

        Debug.Log("WiFi IPv4: " + wifiIP);

        if (ipText != null)
            ipText.text = wifiIP;
    }
}
