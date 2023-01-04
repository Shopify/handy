using System.IO;
using System.Text;
using System.Linq;
using System;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class CaptureServer : MonoBehaviour
{
    public string PortNumber = "7087";

    private HttpListener m_Listener = null;
    private string m_RootPath = null;

    private void OnEnable()
    {
        m_RootPath = Application.temporaryCachePath;
        Debug.Log("Starting server at http://" + GetLocalAddress() + ":" + PortNumber + "/ ...");
        m_Listener = new HttpListener();
        m_Listener.Prefixes.Add("http://*:" + PortNumber + "/");
        m_Listener.Start();
        var res = m_Listener.BeginGetContext(new AsyncCallback(ContextCallback), null);
        Debug.Log("Started");
    }

    private void OnDisable()
    {
        if (m_Listener != null)
        {
            m_Listener.Abort();
            m_Listener.Stop();
            m_Listener.Close();
            m_Listener = null;
        }
    }

    private void ContextCallback(IAsyncResult result)
    {
        HttpListenerContext context = m_Listener.EndGetContext(result);
        ProcessRequest(context);
        if (m_Listener != null && m_Listener.IsListening)
        {
            m_Listener.BeginGetContext(new AsyncCallback(ContextCallback), null);
        }
    }

    private void ProcessRequest(HttpListenerContext context)
    {
        Debug.Log("Receiving upload...");
        string filename = Path.Join(m_RootPath, DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jsonlines");
        using (FileStream fs = new FileStream(filename, FileMode.Create))
        {
            context.Request.InputStream.CopyTo(fs);
            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/html";
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8))
            {
                writer.WriteLine("Got it!");
            }
            context.Response.Close();
        }
        Debug.Log("Received upload and saved to " + filename);
    }

    private string GetLocalAddress()
    {
        string hostname = Dns.GetHostName();
        //string hostname = "localhost";
        var entry = Dns.GetHostEntry(hostname);
        //var entry = Dns.Resolve(hostname);
        var addressList = entry.AddressList;
        Debug.Log("Addresses: " + String.Join(", ", addressList.Select(a => a.ToString())));
        foreach (IPAddress ip in addressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return null;
    }
}