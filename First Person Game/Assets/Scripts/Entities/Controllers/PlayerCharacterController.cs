using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A controller based on <see cref="BaseCharacterController"/> that is used for player characters
/// </summary>
public class PlayerCharacterController : BaseCharacterController
{
    [Header("Player Character Controller Component References")]

    [Tooltip("A camera toggle used to toggle between multiple cameras"),
        SerializeField, VisibleOnly] private CameraToggle _cameraToggle;
    /// <summary>
    /// The attached <see cref="CameraToggle"/> component
    /// </summary>
    public CameraToggle CameraToggle
    {
        get { return _cameraToggle; }
        private set { _cameraToggle = value; }
    }

    InGameControls controls;

    protected override void Awake()
    {
        base.Awake();
        CameraToggle = GetComponent(typeof(CameraToggle)) as CameraToggle;
        controls = new InGameControls();

        controls.Gameplay.Move.performed += context => UpdateMovementInput(context.ReadValue<Vector2>());
        controls.Gameplay.Move.canceled += context => UpdateMovementInput(Vector3.zero);

        controls.Gameplay.Look.performed += context => UpdateCameraInput(context.ReadValue<Vector2>());
        controls.Gameplay.Look.canceled += context => UpdateCameraInput(Vector2.zero);

        controls.Gameplay.ToggleCamera.performed += context => ToggleCamera();
    }

    /// <summary>
    /// Feeds input to the appropriate component
    /// </summary>
    protected override void FixedUpdate()
    {
        CameraMover.RotateCamera(CameraInput);
        Mover.Move(MovementInput);
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void ToggleCamera()
    {
        CameraToggle.AdvanceCurrentCamera();
    }
}
