using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class CaptureManager : MonoBehaviour
{
    public bool recording = false;
    public OVRInput.Button activationButton = OVRInput.Button.PrimaryShoulder;
    public GameObject[] disabledOnRecording;
    public GameObject[] enabledOnRecording;

    private float m_CurrentTimestamp = 0f;
    private bool m_WasRecording = false;
    private CaptureTransform[] m_Capturers = new CaptureTransform[0];
    private string m_Filepath;

    private void Start()
    {
        ReconcileRecordingObjects(false);
        GatherCapturers();
    }

    private void GatherCapturers()
    {
        m_Capturers = GameObject.FindObjectsOfType<CaptureTransform>();
    }

    private void ReconcileRecordingObjects(bool rec)
    {
        foreach (var obj in disabledOnRecording)
        {
            obj.SetActive(!rec);
        }
        foreach (var obj in enabledOnRecording)
        {
            obj.SetActive(rec);
        }
    }

    private void HandleStartRecording()
    {
        m_CurrentTimestamp = 0f;
        m_Filepath = Path.Combine(Application.persistentDataPath, System.DateTime.UtcNow.ToString("u").Replace(" ", "-").Replace(":", "-") + ".jsonlines");
        ReconcileRecordingObjects(true);
        GatherCapturers();
        //var capturerIndices = m_Capturers.Select((c, i) => new Tuple<string, int>(c.captureName, i)).ToDictionary(t => t.Item1, t => t.Item2);
        //File.AppendAllLines(m_Filepath, new string[] { JsonConvert.SerializeObject(capturerIndices) });
        var capturerNames = m_Capturers.Select(c => c.captureName);
        File.AppendAllLines(m_Filepath, new string[] { JsonConvert.SerializeObject(capturerNames) });
    }

    private void HandleStopRecording()
    {
        ReconcileRecordingObjects(false);
    }

    private void UpdateRecording()
    {
        if (recording && !m_WasRecording)
        {
            HandleStartRecording();
        } else if (!recording && m_WasRecording)
        {
            HandleStopRecording();
        }
        m_WasRecording = recording;
    }

    private void UpdateDoRecord()
    {
        if (!recording)
        {
            return;
        }
        var flattenedTransforms = m_Capturers.Select(c => TransformFrame.FromTransform(m_CurrentTimestamp, c.transform).Flattened());
        File.AppendAllLines(m_Filepath, new string[] { JsonConvert.SerializeObject(flattenedTransforms) });
        m_CurrentTimestamp += Time.deltaTime;
    }

    private void UpdateButtonPress()
    {
        if (OVRInput.GetDown(activationButton) || Input.GetKey(KeyCode.JoystickButton6))
        {
            recording = !recording;
        }
    }

    void Update()
    {
        UpdateButtonPress();
        UpdateRecording();
        UpdateDoRecord();
    }
}
