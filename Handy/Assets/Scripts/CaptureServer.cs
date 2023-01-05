using System.IO;
using System.Text;
using System.Net;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CaptureServer : MonoBehaviour
{
    // Reference to a `PlaybackManager` where we can send the `.jsonlines` files for processing
    public PlaybackManager playbackManager;
    // Reference to an `AddressUtil` so we know what hostname and port to serve on
    public AddressUtil addressUtil;

    // `HttpListener` controls the HTTP server socket and accepting incoming connections
    private HttpListener m_Listener = null;
    // The root path where we'll save uploaded files
    private string m_RootPath = null;
    // A temporary holding variable for passing strings from HTTP listener threads to Unity's game thread
    private string m_ProcessPlaybackFilename = null;
    // A list of `.jsonlines` files to process when the `PlaybackManager` is ready
    private List<string> m_ProcessPlaybackFilenames = new List<string>();

    // When this component is enabled, we reset paths and all in-flight data, and start a new server
    private void OnEnable()
    {
        // Reset paths and all in-flight data
        m_ProcessPlaybackFilename = null;
        m_RootPath = Application.temporaryCachePath;
        m_ProcessPlaybackFilenames = new List<string>();

        // Start a new server listening
        Debug.Log("Starting server at " + addressUtil.GetLocalHTTPAddress() + " ...");
        m_Listener = new HttpListener();
        m_Listener.Prefixes.Add("http://*:" + addressUtil.PortNumber + "/");
        m_Listener.Start();
        var res = m_Listener.BeginGetContext(new AsyncCallback(ContextCallback), null);
        Debug.Log("Started");
    }

    // When this component is disabled, we disable and close any existing HTTP listeners
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

    // This handles each HTTP request by gathering the proper information,
    // dispatching the request info to the `ProcessRequest` function, and
    // finally preparing the listener for its next request.
    private void ContextCallback(IAsyncResult result)
    {
        var context = m_Listener.EndGetContext(result);
        ProcessRequest(context);
        if (m_Listener != null && m_Listener.IsListening)
        {
            m_Listener.BeginGetContext(new AsyncCallback(ContextCallback), null);
        }
    }

    // Processes an individual request by streaming all of the request body data to a `.jsonlines`
    // file, and then enqueuing that `.jsonlines` file for processing by `PlaybackManager`
    private void ProcessRequest(HttpListenerContext context)
    {
        // Choose a filename to stream the request body to
        var filename = Path.Join(m_RootPath, DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".jsonlines");
        Debug.Log("Receiving upload to " + filename + "...");

        using (FileStream fs = new FileStream(filename, FileMode.Create))
        {
            // Stream the data to the efile
            context.Request.InputStream.CopyTo(fs);

            // Respond with "Got it!"
            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/html";
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream, Encoding.UTF8))
            {
                writer.WriteLine("Got it!");
            }
            context.Response.Close();

            // Enqueue the `.jsonlines` file for processing
            m_ProcessPlaybackFilename = filename;
        }

        // Print intermediary progress
        Debug.Log("Received upload and saved to " + filename);
    }

    // Looks for a file to be ready for processing, and enqueues it
    private void UpdateProcessPlaybackList()
    {
        var tmp = m_ProcessPlaybackFilename;
        if (tmp != null)
        {
            m_ProcessPlaybackFilename = null;
            m_ProcessPlaybackFilenames.Add(tmp);
        }
    }

    // Looks for any enqueued files ready for processing, and sends them to `PlaybackManager`
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

    // Run the above update functions to check for and enqueue work
    private void Update()
    {
        UpdateProcessPlaybackList();
        UpdateProcessPlayback();
    }
}