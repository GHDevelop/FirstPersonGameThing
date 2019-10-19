using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    [Tooltip("The list of cameras to toggle between"),
        SerializeField] private Camera[] _cameras;
    public Camera[] Cameras
    {
        get { return _cameras; }
        private set { _cameras = value; }
    }

    [Tooltip("The index of the current camera"),
        SerializeField, VisibleOnly] private int _currentCameraIndex;
    public int CurrentCameraIndex
    {
        get { return _currentCameraIndex; }
        private set { _currentCameraIndex = value; }
    }

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
