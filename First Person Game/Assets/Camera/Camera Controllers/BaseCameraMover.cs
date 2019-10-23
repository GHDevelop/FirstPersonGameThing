using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base camera mover that all camera movers derive from. This one simply rotates the camera with input given.
/// </summary>
public class BaseCameraMover : MonoBehaviour
{
    [Header("Cameras and Transforms")]

    [Tooltip("The transform of the object this component is attached to, should typically be the character in question"),
        SerializeField, VisibleOnly] private Transform _cameraMoverTransform;
    /// <summary>
    /// The <see cref="Transform"/> of the object the <see cref="UnityEngine.Camera"/> is attached to (for example, the player character). 
    /// </summary>
    protected Transform CameraMoverTransform
    {
        get { return _cameraMoverTransform; }
        private set { _cameraMoverTransform = value; }
    }

    [Tooltip("The camera that the character will look through. Will be set automatically if not pre-configured"),
        SerializeField] private Camera _camera;
    /// <summary>
    /// The camera itself. Generally only used to get the <see cref="CameraTransform"/> if not already set
    /// </summary>
    /// <seealso cref="CameraTransform"/>
    protected Camera Camera
    {
        get { return _camera; }
        private set { _camera = value; }
    }

    [Tooltip("The transform to move when altering the camera. Will typically just be the camera, but can be set manually if a more complicated configuration is desired."),
        SerializeField] private Transform _cameraTransform;
    /// <summary>
    /// The <see cref="Transform"/> used to move the camera. 
    /// This is by default set to the transform of <see cref="Camera"/>, but can also be set manually to a pivot.
    /// </summary>
    /// <seealso cref="Camera"/>
    protected Transform CameraTransform
    {
        get { return _cameraTransform; }
        private set { _cameraTransform = value; }
    }

    [Header("Camera Mover Properties")]

    [Tooltip("The camera's vertical bounds"),
        SerializeField] private float _maxYRotation = 60;
    /// <summary>
    /// The maximum deviation the camera can achieve in rotation along the Y Axis.
    /// </summary>
    public float MaxYRotation
    {
        get { return _maxYRotation; }
        private set { _maxYRotation = value; }
    }

    [Tooltip("The speed the camera moves per second"),
        SerializeField] private float _cameraSpeed = 300;
    /// <summary>
    /// The speed multiplier used for the camera's rotation
    /// </summary>
    public float CameraSpeed
    {
        get { return _cameraSpeed; }
        private set { _cameraSpeed = value; }
    }

    [Tooltip("The camera's initial rotation"),
        SerializeField, VisibleOnly] private Vector3 _initialCameraRotation;
    /// <summary>
    /// The initial local rotation of the camera. Used in calculations with <see cref="MaxYRotation"/> to limit vertical look
    /// </summary>
    public Vector3 InitialCameraRotation
    {
        get { return _initialCameraRotation; }
        private set { _initialCameraRotation = value; }
    }

    [Tooltip("The current amount the camera is rotated"),
        SerializeField, VisibleOnly] private Vector3 _currentCameraRotation = Vector3.zero;
    /// <summary>
    /// The camera's current intended local rotation
    /// </summary>
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
        if (Camera == null)
        {
            Camera = GetComponentInChildren(typeof(Camera)) as Camera;
        }
        if (CameraTransform == null)
        {
            CameraTransform = Camera.transform;
        }

        InitialCameraRotation = CameraTransform.localEulerAngles;
        CurrentCameraRotation = InitialCameraRotation;
    }

    /// <summary>
    /// Updates the current camera rotation speed, then applies the rotation
    /// </summary>
    /// <param name="rotationDirection"></param>
    public virtual void RotateCamera(Vector3 rotationDirection)
    {
        CurrentCameraRotation += rotationDirection * CameraSpeed * Time.fixedDeltaTime;
        ApplyRotation();
    }

    /// <summary>
    /// Applies rotation to the camera
    /// </summary>
    protected virtual void ApplyRotation()
    {
        CameraTransform.localRotation = Quaternion.Euler(CurrentCameraRotation);
    }
}
