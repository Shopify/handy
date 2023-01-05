using UnityEngine;

// Tags a component in the view hierarchy to be captured by `PlaybackManager`
public class CaptureTransform : MonoBehaviour
{
    // The name of the component to capture (will default to the component's name)
    public string captureName;

    private void Start()
    {
        if (string.IsNullOrEmpty(captureName))
        {
            captureName = name;
        }
    }
}
