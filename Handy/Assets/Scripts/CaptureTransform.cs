using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureTransform : MonoBehaviour
{
    public string captureName;

    private void Start()
    {
        if (string.IsNullOrEmpty(captureName))
        {
            captureName = name;
        }
    }
}
