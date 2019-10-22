using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract extension of <see cref="BaseController"/> used specifically for characters
/// </summary>
public abstract class BaseCharacterController : BaseController
{
    [Header("Universal Character Controller Component References")]

    [Tooltip("The character's mover"),
        SerializeField, VisibleOnly]
    private BaseMover _mover;
    /// <summary>
    /// The attached <see cref="BaseMover"/> component
    /// </summary>
    public BaseMover Mover
    {
        get { return _mover; }
        private set { _mover = value; }
    }

    [Tooltip("The character's camera mover"),
        SerializeField, VisibleOnly]
    private BaseCameraMover _cameraMover;
    /// <summary>
    /// The attached <see cref="BaseCameraMover"/> component
    /// </summary>
    public BaseCameraMover CameraMover
    {
        get { return _cameraMover; }
        private set { _cameraMover = value; }
    }

    [Header("Input Buffer")]

    [Tooltip("The current input for movement"),
        SerializeField, VisibleOnly]
    private Vector3 _movementInput;
    /// <summary>
    /// The input buffered for movement
    /// </summary>
    public Vector3 MovementInput
    {
        get { return _movementInput; }
        protected set { _movementInput = value; }
    }

    [Tooltip("The current input for the camera"),
        SerializeField, VisibleOnly]
    private Vector3 _cameraInput;
    /// <summary>
    /// The input buffered for the camera
    /// </summary>
    public Vector3 CameraInput
    {
        get { return _cameraInput; }
        protected set { _cameraInput = value; }
    }

    /// <summary>
    /// Overrides of this should always use base.Awake
    /// </summary>
    protected virtual void Awake()
    {
        Mover = GetComponent(typeof(BaseMover)) as BaseMover;
        CameraMover = GetComponent(typeof(BaseCameraMover)) as BaseCameraMover;
    }

    protected virtual void UpdateMovementInput(Vector2 input)
    {
        Vector3 inputAsVector3 = new Vector3(input.x,
                                             0,
                                             input.y);

        UpdateMovementInput(inputAsVector3);
    }

    protected virtual void UpdateMovementInput(Vector3 input)
    {
        MovementInput = input;
    }

    protected virtual void UpdateCameraInput(Vector2 input)
    {
        CameraInput = new Vector3(-input.y, input.x, 0);
    }
}
