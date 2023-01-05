using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using UnityEngine;

public class CaptureManager : MonoBehaviour
{
    // Boolean flag representing whether the manager is currently capturing a recording
    public bool recording = false;
    // The primary VR button that should start/stop recording
    public OVRInput.Button activationButton = OVRInput.Button.PrimaryShoulder;
    // A list of GameObjects that should be enabled only when recording, and disabled otherwise
    public GameObject[] enabledOnRecording;
    // An optional reference to `AddressUtil` so that we can easily send recordings back to the server
    public AddressUtil addressUtil = null;

    private float m_CurrentTimestamp = 0f;
    private bool m_WasRecording = false;
    private CaptureTransform[] m_Capturers = new CaptureTransform[0];
    private string m_Filepath;

    // Initialize by disabling all objects that should only be enabled during recording,
    // and getting a list of all transforms we want to capture
    private void Start()
    {
        ReconcileEnabledObjects(false);
        GatherCapturers();
    }

    // Gathers a reference to all `CaptureTransform` instances we can find, which tracks
    // objects we want to capture transform information about
    private void GatherCapturers()
    {
        m_Capturers = GameObject.FindObjectsOfType<CaptureTransform>();
    }

    // Disable or enable objects (like recording indicators) based on the given
    // recording status
    private void ReconcileEnabledObjects(bool rec)
    {
        foreach (var obj in enabledOnRecording)
        {
            obj.SetActive(rec);
        }
    }

    // Starts the recording process
    private void HandleStartRecording()
    {
        // Recording starts at timestamp 0
        m_CurrentTimestamp = 0f;

        // The filepath includes the current time
        m_Filepath = Path.Combine(Application.persistentDataPath, System.DateTime.UtcNow.ToString("u").Replace(" ", "-").Replace(":", "-") + ".jsonlines");

        // Enable all objects that should only be enabled during recording
        ReconcileEnabledObjects(true);

        // Gather the capturers one more time (should not be needed, but maybe the list
        // changed dynamically since `Start`)
        GatherCapturers();

        // Our first line written to the file will be the names of all of the tracked objects
        var capturerNames = m_Capturers.Select(c => c.captureName);
        File.AppendAllLines(m_Filepath, new string[] { JsonConvert.SerializeObject(capturerNames) });

        Debug.Log("Began recording");
    }

    // Stops the recording process
    private void HandleStopRecording()
    {
        // Disable all objects that should only be enabled during recording
        ReconcileEnabledObjects(false);

        // Print the location of the finished recording (.jsonlines)
        Debug.Log("Finished recording. The .jsonlines file is located here: " + m_Filepath);

        // If we have a reference to an `AddressUtil`, then use it to send the `.jsonlines` file
        // back to the development server
        addressUtil?.LoadSavedLocalAddress((address) => {
            if (!string.IsNullOrEmpty(address))
            {
                Debug.Log("Uploading to " + address);
                using(var ws = (new WebClient()).OpenWrite(address))
                using(var rs = File.OpenRead(m_Filepath))
                {
                    rs.CopyTo(ws);
                }
                Debug.Log("Finished uploaading to " + address);
            }
        }, (error) => {
            Debug.Log("Got error getting saved local address: " + error.ToString());
        });
    }

    // Keeps track of recording and invokes logic when transitioning between recording states
    private void UpdateRecording()
    {
        // Check if recording status has flipped from off to on, and if so, run the
        // `HandleStartRecording` method to handle the transition
        if (recording && !m_WasRecording)
        {
            HandleStartRecording();
        }
        // Check if recording status has flipped from on to off, and if so, run the
        // `HandleStopRecording` method to handle the transition
        else if (!recording && m_WasRecording)
        {
            HandleStopRecording();
        }

        m_WasRecording = recording;
    }

    // When recording, captures transform information and appends it as a line to the `.jsonlines` file
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

    // Watches for VR button presses to toggle recording state
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
