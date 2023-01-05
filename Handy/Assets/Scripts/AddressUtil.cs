using System.IO;
using System.Net;
using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
#endif

// This component allows you to make a special build of your application that can
// open a connection back to the server that built it, for the purposes of debugging
// and improving ease of development
public class AddressUtil : MonoBehaviour
#if UNITY_EDITOR
, IPreprocessBuildWithReport
#endif
{
    // The port that we should listen on for our HTTP communication
    public string PortNumber = "7087";
    // The prefix of the IP address used to communicate with the local network
    public string IpPrefix = "192.168";

    private string m_StreamingPath = null;

    private void Awake()
    {
        // We save the streaming path as soon as we awaken, so that we can access
        // it without worrying too much about multithreading
        m_StreamingPath = Application.streamingAssetsPath;

        // Save the local address on awake just for good measure
        // This should not be needed, since we do this on build, but in case that
        // doesn't work, this will.
        SaveLocalAddress();
    }

    // Gets the local IP address that we will listen on for our HTTP communication
    public string GetLocalAddress()
    {
        // Use the `GetAllNetworkInterfaces` API to get a list of ip addresses
        // that are used by this device to talk to a networ
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
        
        // First, check to see if any of the IP addresses have the prefix provided by
        // the component
        foreach (IPAddress ipAddr in addressList)
        {
            var ip = ipAddr.ToString();
            if (ip.StartsWith(IpPrefix))
            {
                return ip;
            }
        }

        // Otherwise, none matched the requested prefix, so return the first IP in the list
        foreach (IPAddress ipAddr in addressList)
        {
            return ipAddr.ToString();
        }

        // Hmm, there were no IPs
        return null;
    }

    // Combines the local IP address and the configured port number to create a local
    // network URI for this server
    public string GetLocalHTTPAddress()
    {
        return "http://" + GetLocalAddress() + ":" + PortNumber + "/";
    }

    // A path to a file in streaming assets that will contain the local network URI of the server
    public string GetStreamingAssetAddressPath()
    {
        return Path.Join(m_StreamingPath, "serveraddress.txt");
    }

    // Writes the local network URI of this server to a file in streaming assets
    public void SaveLocalAddress()
    {
        File.WriteAllText(GetStreamingAssetAddressPath(), GetLocalHTTPAddress());
    }

    // Loads the saved network URI from a file in streaming assets
    public void LoadSavedLocalAddress(Action<string> OnLoad, Action<UnityWebRequest.Result> OnError)
    {
#if UNITY_ANDROID
        StartCoroutine(_LoadSavedLocalAddress(OnLoad, OnError));
#else
        OnLoad(File.ReadAllText(GetStreamingAssetAddressPath()));
#endif
    }

    // Uses a UnityWebRequest to load the network URI from its file in streaming assets
    private IEnumerator _LoadSavedLocalAddress(Action<string> OnLoad, Action<UnityWebRequest.Result> OnError)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(GetStreamingAssetAddressPath()))
        {
            yield return req.SendWebRequest();
            switch (req.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    OnError?.Invoke(req.result);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    OnError?.Invoke(req.result);
                    break;
                case UnityWebRequest.Result.Success:
                    OnLoad?.Invoke(req.downloadHandler.text);
                    break;
            }
        }
        yield break;
    }

    // The following section implements `IPreprocessBuildWithReport`, which will run this code to
    // generate the file with the server network URI on build

#if UNITY_EDITOR
    public int callbackOrder { get { return 0; } }

    public void OnPreprocessBuild(BuildReport report)
    {
        SaveLocalAddress();
    }
#endif
}