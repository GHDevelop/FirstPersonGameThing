using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Awake()
    {
        base.Awake();
        CameraToggle = GetComponent(typeof(CameraToggle)) as CameraToggle;
    }

    /// <summary>
    /// Collects Inputs for use with FixedUpdate, also toggles which camera is used, since that has no direct gameplay impact
    /// </summary>
    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CameraToggle != null)
        {
            CameraToggle.AdvanceCurrentCamera();
        }

        CameraInput = new Vector3(
            -Input.GetAxis("Mouse Y"),
            Input.GetAxis("Mouse X"),
            0);

        MovementInput = new Vector3(
            Input.GetKey(KeyCode.D) ? 1 : (Input.GetKey(KeyCode.A) ? -1 : 0),
            0,
            Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0));
    }

    /// <summary>
    /// Feeds input to the appropriate component
    /// </summary>
    protected void FixedUpdate()
    {
        CameraMover.RotateCamera(CameraInput);
        Mover.Move(MovementInput.normalized);
    }
}
