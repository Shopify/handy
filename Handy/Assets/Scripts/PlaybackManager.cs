using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEngine.Formats.Alembic.Exporter;
#endif

public class PlaybackManager : MonoBehaviour
{
    // The path to the `.jsonlines` file to process, either a full path or relative to Streaming Assets
    public string filepath;
    // A flag determining whether this component should begin playback on start, otherwise will need to
    // be triggered programmatically 
    public bool autoplay = true;
#if UNITY_EDITOR
    // A reference to the Alembic exporter that converts editor replays into `.abc` files
    public AlembicExporter alembicExporter;
#endif
    
    // The current timestamp we're playing back right now
    private float m_Timestamp = 0f;
    // The index of the frame that we're playing back right now
    private int m_Index = 0;
    // A flag indicating whether we're currently playing back a recording or not
    private bool m_IsPlaying = false;
    // A list of transforms that we want to marionette during playback
    private CaptureTransform[] m_Capturers = new CaptureTransform[0];
    // A list of frames of transform data, each keyed by the name of its `CaptureTransform`
    private Dictionary<string, List<TransformFrame>> m_Frames = new Dictionary<string, List<TransformFrame>>();
    // The largest count of a value in `m_Frames`
    private int m_MaxFrame = 0;

    public bool IsPlaying()
    {
        return m_IsPlaying;
    }

    // Takes one path to a `.jsonlines` file, and kicks off its playback (and conversion to `.abc`)
    public void ProcessPlayback(string filepath)
    {
        this.filepath = filepath;
    #if UNITY_EDITOR
        alembicExporter.Recorder.Settings.OutputPath = Path.ChangeExtension(filepath, ".abc");
    #endif
        LoadFrames();
        BeginPlayback();
    }

    // Returns either the assigned `filepath`, or its relative value in
    // `Application.streamingAssetsPath` if not found.
    private string GetFilepath()
    {
        if (string.IsNullOrEmpty(filepath))
        {
            throw new ArgumentException(".jsonlines filename cannot be empty");
        }
        if (File.Exists(filepath))
        {
            return filepath;
        }
        return Path.Combine(Application.streamingAssetsPath, filepath);
    }

    // Loads and decodes the configured `.jsonlines` file into `m_Frames`, and
    // sets the value of `m_MaxFrame` for good measure
    private void LoadFrames()
    {
        m_Frames.Clear();
        var fpath = GetFilepath();
        var lines = File.ReadAllLines(fpath);
        if (lines.Length == 0)
        {
            Debug.Log("Could not load any frames for " + fpath);
            return;
        }

        var names = JsonConvert.DeserializeObject<string[]>(lines[0]);
        foreach (var captureName in names)
        {
            m_Frames.Add(captureName, new List<TransformFrame>());
        }

        for (var i = 1; i < lines.Length; ++i)
        {
            var flattenedTransforms = JsonConvert.DeserializeObject<float[][]>(lines[i]);
            for (var j = 0; j < flattenedTransforms.Length; ++j)
            {
                m_Frames[names[j]].Add(TransformFrame.FromFlattened(flattenedTransforms[j]));
            }
        }

        m_MaxFrame = m_Frames.Select(f => f.Value.Count).Max();
        Debug.Log("Max frame: " + m_MaxFrame);
    }

    // Gathers a reference to all `CaptureTransform` instances we can find, which tracks
    // objects we want to marionette with our captured transform information
    private void GatherCapturers()
    {
        m_Capturers = GameObject.FindObjectsOfType<CaptureTransform>();
    }

    // Begins the playback process and starts the conversion to `.abc`
    private void BeginPlayback()
    {
        // Gather any capturers in the scene
        GatherCapturers();

        // Reset timestamps and indices
        m_Timestamp = 0f;
        m_Index = 0;

        // Kick off playback
        m_IsPlaying = true;

    #if UNITY_EDITOR
        // Indicate record start to Alembic
        alembicExporter.BeginRecording();
    #endif
    }

    // FUTURE: Query for data before/after timestamp and lerp values between them
    private void UpdatePlaying()
    {
        // If we're not currently playing back, there's nothing to do
        if (!m_IsPlaying)
        {
            return;
        }

        // Check whether any of the frames at the current index are for a timestamp later than our current one
        bool willAdvance = false;
        foreach (var capturer in m_Capturers)
        {
            try
            {
                var frame = m_Frames[capturer.captureName][m_Index];
                // If we find a timestamp later than our current one, copy the later transform values into it
                if (m_Timestamp > frame.timestamp)
                {
                    frame.CopyToTransform(capturer.transform);
                    willAdvance = true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("Could not get frame for " + capturer.captureName + ": " + e);
            }
        }

        // If we found any changes, advance our index
        if (willAdvance)
        {
            m_Index += 1;
        }

        // Time has passed, whether or not we showed new frames, so advance our internal timestamp
        m_Timestamp += Time.deltaTime;

        // If we're past the max frame, or there are no frames, stop playback and end recording
        if (m_Capturers.Count() == 0 || m_Index >= m_MaxFrame)
        {
            Debug.Log("All frames have been processed, so playback has been stopped");
            m_IsPlaying = false;
#if UNITY_EDITOR
            if (alembicExporter != null)
            {
                alembicExporter.EndRecording();
            }
#endif
        }
    }

    void Start()
    {
        // If autoplay is set, load frames and begin playback on start
        if (autoplay)
        {
            LoadFrames();
            BeginPlayback();
        }
    }

    void Update()
    {
        UpdatePlaying();
    }
}
