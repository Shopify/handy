using System.IO;
using System.Text;
using System.Net;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

public class AddressUtil : MonoBehaviour
#if UNITY_EDITOR
, IPreprocessBuildWithReport
#endif
{
    public static AddressUtil Instance = null;

    public string PortNumber = "7087";
    public string IpPrefix = "192.168";

    private string m_StreamingPath = null;

    private void Awake()
    {
        m_StreamingPath = Application.streamingAssetsPath;
        SaveLocalAddress();
        Instance = this;
    }

    public string GetLocalAddress()
    {
        var hostname = Dns.GetHostName();
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        var addressList = new List<IPAddress>();
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            foreach (var ip in ni.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    addressList.Add(ip.Address);
                }
            }
        }
        foreach (IPAddress ipAddr in addressList)
        {
            var ip = ipAddr.ToString();
            if (ip.StartsWith(IpPrefix))
            {
                return ip;
            }
        }
        foreach (IPAddress ipAddr in addressList)
        {
            return ipAddr.ToString();
        }
        return null;
    }

    public string GetLocalHTTPAddress()
    {
        return "http://" + GetLocalAddress() + ":" + PortNumber + "/";
    }

    public string GetStreamingAssetAddressPath()
    {
        return Path.Join(m_StreamingPath, "serveraddress.txt");
    }

    public void SaveLocalAddress()
    {
        File.WriteAllText(GetStreamingAssetAddressPath(), GetLocalHTTPAddress());
    }

    public string ReadSavedLocalAddress()
    {
        return File.ReadAllText(GetStreamingAssetAddressPath());
    }

#if UNITY_EDITOR
    // IPreprocessBuildWithReport
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        SaveLocalAddress();
    }
#endif
}