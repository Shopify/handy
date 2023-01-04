using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Threading;
#if UNITY_EDITOR
using UnityEngine.Formats.Alembic.Exporter;
#endif

public class CaptureServer : MonoBehaviour
{
    private HttpListener m_Listener = null;
    private string m_RootPath = null;

    private void OnEnable()
    {
        m_RootPath = Application.temporaryCachePath;
        StartServer();
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

    private void StartServer()
    {
        Debug.Log("Starting...");
        m_Listener = new HttpListener();
        m_Listener.Prefixes.Add("http://*:7087/");
        m_Listener.Start();
        var res = m_Listener.BeginGetContext(new AsyncCallback(ContextCallback), null);
        Debug.Log("Started");
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
        Debug.Log("A: " + DateTime.Now.ToString("yyyyMMddHHmmssffff"));
        Debug.Log("B: " + m_RootPath);
        string filename = Path.Join(m_RootPath, DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jsonlines");
        Debug.Log("Receiving upload to: " + filename);
        using (FileStream fs = new FileStream(filename, FileMode.Create))
        {
            context.Request.InputStream.CopyTo(fs);
            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/html";
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8))
                writer.WriteLine("Got it!");
            context.Response.Close();
        }
        Debug.Log("Received upload and saved to " + filename);
    }
}