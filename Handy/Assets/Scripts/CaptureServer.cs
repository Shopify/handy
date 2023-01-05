using System.IO;
using System.Text;
using System.Net;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CaptureServer : MonoBehaviour
{
    
    public PlaybackManager playbackManager;
    public AddressUtil addressUtil;

    private HttpListener m_Listener = null;
    private string m_RootPath = null;
    private string m_ProcessPlaybackFilename = null;
    private List<string> m_ProcessPlaybackFilenames = new List<string>();

    private void OnEnable()
    {
        m_ProcessPlaybackFilename = null;
        m_RootPath = Application.temporaryCachePath;
        Debug.Log("Starting server at " + addressUtil.GetLocalHTTPAddress() + " ...");
        m_Listener = new HttpListener();
        m_Listener.Prefixes.Add("http://*:" + addressUtil.PortNumber + "/");
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
        var context = m_Listener.EndGetContext(result);
        ProcessRequest(context);
        if (m_Listener != null && m_Listener.IsListening)
        {
            m_Listener.BeginGetContext(new AsyncCallback(ContextCallback), null);
        }
    }

    private void ProcessRequest(HttpListenerContext context)
    {
        var filename = Path.Join(m_RootPath, DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jsonlines");
        Debug.Log("Receiving upload to " + filename + "...");
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
            m_ProcessPlaybackFilename = filename;
        }
        Debug.Log("Received upload and saved to " + filename);
    }

    private void UpdateProcessPlaybackList()
    {
        var tmp = m_ProcessPlaybackFilename;
        if (tmp != null)
        {
            m_ProcessPlaybackFilename = null;
            m_ProcessPlaybackFilenames.Add(tmp);
        }
    }

    private void UpdateProcessPlayback()
    {
        var pbm = playbackManager;
        if (pbm == null || pbm?.IsPlaying() == true || m_ProcessPlaybackFilenames.Count == 0)
        {
            return;
        }
        var tmp = m_ProcessPlaybackFilenames[0];
        m_ProcessPlaybackFilenames.RemoveAt(0);
        playbackManager?.ProcessPlayback(tmp);
    }

    private void Update()
    {
        UpdateProcessPlaybackList();
        UpdateProcessPlayback();
    }
}