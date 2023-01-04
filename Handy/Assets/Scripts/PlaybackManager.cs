using System.Collections;
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
    public string filepath;
    public bool autoplay = true;
#if UNITY_EDITOR
    public AlembicExporter alembicExporter;
#endif
    
    private float m_Timestamp = 0f;
    private int m_Index = 0;
    private bool m_IsPlaying = false;
    private CaptureTransform[] m_Capturers = new CaptureTransform[0];
    private Dictionary<string, List<TransformFrame>> m_Frames = new Dictionary<string, List<TransformFrame>>();
    private int m_MaxFrame = 0;
    private GameObject m_Tmp;
    private bool m_PlaybackCompleted = false;

    private string GetFilepath()
    {
        return Path.Combine(Application.streamingAssetsPath, filepath);
    }

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

    private void GatherCapturers()
    {
        m_Capturers = GameObject.FindObjectsOfType<CaptureTransform>();
    }

    private void BeginPlayback()
    {
        GatherCapturers();
        m_Timestamp = 0f;
        m_Index = 0;
        m_IsPlaying = true;
    }

    private void UpdatePlaying()
    {
        if (!m_IsPlaying)
        {
            return;
        }

        // TODO: Actually make this timestamp-based and maybe lerp between timestamps
        bool willAdvance = false;
        foreach (var capturer in m_Capturers)
        {
            try {
                var frame = m_Frames[capturer.captureName][m_Index];
                if (m_Timestamp > frame.timestamp) {
                    frame.CopyToTransform(capturer.transform);
                    willAdvance = true;
                }
            } catch (Exception e) {
                Debug.Log("Could not get frame for " + capturer.captureName + ": " + e);
            }
        }

        if (willAdvance)
        {
            m_Index += 1;
        }

        m_Timestamp += Time.deltaTime;

        if (m_Capturers.Count() == 0 || m_Index >= m_MaxFrame)
        {
            Debug.Log("All frames have been processed, so playback has been stopped");
            m_PlaybackCompleted = true;
            m_IsPlaying = false;
#if UNITY_EDITOR
            if (alembicExporter != null)
            {
                alembicExporter.EndRecording();
            }
#endif
        }
    }

    public bool PlaybackCompleted()
    {
        return m_PlaybackCompleted;
    }

    void Start()
    {
        m_Tmp = new GameObject();
        m_Tmp.SetActive(false);
        LoadFrames();
        if (autoplay)
        {
            BeginPlayback();
        }
    }

    void Update()
    {
        UpdatePlaying();
    }
}
