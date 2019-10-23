using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component that toggles between game windows
/// </summary>
public class CameraToggle : MonoBehaviour
{
    [Tooltip("The list of cameras to toggle between"),
        SerializeField] private Camera[] _cameras;
    /// <summary>
    /// The list of cameras to toggle between
    /// </summary>
    private Camera[] Cameras
    {
        get { return _cameras; }
        set { _cameras = value; }
    }

    [Tooltip("The index of the current camera"),
        SerializeField, VisibleOnly] private int _currentCameraIndex;
    /// <summary>
    /// The index of the current camera
    /// </summary>
    private int CurrentCameraIndex
    {
        get { return _currentCameraIndex; }
        set { _currentCameraIndex = value; }
    }

    /// <summary>
    /// Disables the current camera and enables the next camera
    /// </summary>
    public void AdvanceCurrentCamera()
    {
        if (Cameras.Length == 0)
        {
            return;
        }

        Cameras[CurrentCameraIndex].enabled = false;
        CurrentCameraIndex = (CurrentCameraIndex + 1) % Cameras.Length;
        Cameras[CurrentCameraIndex].enabled = true;
    }
}
