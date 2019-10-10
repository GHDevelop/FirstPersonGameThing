using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCameraController : MonoBehaviour
{
    [Tooltip("The transform of the object this component is attached to, should typically be the character in question"),
        SerializeField, VisibleOnly] private Transform _cameraMoverTransform;
    public Transform CameraMoverTransform
    {
        get { return _cameraMoverTransform; }
        private set { _cameraMoverTransform = value; }
    }

    [Tooltip("The camera that the character will look through. Will be set automatically if not pre-configured"),
        SerializeField] private Camera _eyes;
    public Camera Eyes
    {
        get { return _eyes; }
        private set { _eyes = value; }
    }

    [Tooltip("The transform to move when altering the camera. Will typically just be the camera, but can be set manually if a more complicated configuration is desired."),
        SerializeField] private Transform _cameraTransform;
    public Transform CameraTransform
    {
        get { return _cameraTransform; }
        private set { _cameraTransform = value; }
    }

    [Tooltip("The camera's vertical bounds"),
        SerializeField] private float _maxYRotation = 60;
    public float MaxYRotation
    {
        get { return _maxYRotation; }
        private set { _maxYRotation = value; }
    }

    [Tooltip("The current amount the camera is rotated"),
        SerializeField, VisibleOnly] private Vector3 _currentCameraRotation = Vector3.zero;
    public Vector3 CurrentCameraRotation
    {
        get { return _currentCameraRotation; }
        private set
        {
            value.x = Mathf.Clamp(value.x, -MaxYRotation, MaxYRotation);
            _currentCameraRotation = value;
        }
    }

    private void Awake()
    {
        CameraMoverTransform = transform;
        if (Eyes == null)
        {
            Eyes = GetComponentInChildren(typeof(Camera)) as Camera;
        }
        if (CameraTransform == null)
        {
            CameraTransform = Eyes.transform;
        }
    }

    public virtual void RotateCamera(Vector3 rotationDirection)
    {
        CurrentCameraRotation += rotationDirection * 60 * Time.deltaTime;
        ApplyRotation();
    }

    protected virtual void ApplyRotation()
    {
        CameraTransform.rotation = Quaternion.Euler(CurrentCameraRotation);
    }
}
