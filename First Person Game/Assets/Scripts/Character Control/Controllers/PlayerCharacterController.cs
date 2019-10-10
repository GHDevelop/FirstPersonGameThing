using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : BaseController
{
    [Tooltip("The character's mover"),
        SerializeField, VisibleOnly] private BaseMover _mover;
    /// <summary>
    /// The attached <see cref="BaseMover"/> component
    /// </summary>
    public BaseMover Mover
    {
        get { return _mover; }
        private set { _mover = value; }
    }

    [Tooltip("The character's camera mover"),
        SerializeField, VisibleOnly] private BaseCameraController _cameraMover;
    public BaseCameraController CameraMover
    {
        get { return _cameraMover; }
        private set { _cameraMover = value; }
    }

    private void Awake()
    {
        Mover = GetComponent(typeof(BaseMover)) as BaseMover;
        CameraMover = GetComponent(typeof(BaseCameraController)) as BaseCameraController;
    }

    protected override void Update()
    {
        Vector3 cameraInput = new Vector3(
            -Input.GetAxis("Mouse Y"),
            Input.GetAxis("Mouse X"),
            0);

        CameraMover.RotateCamera(cameraInput);

        Vector3 movementInput = new Vector3(
            Input.GetKey(KeyCode.D) ? 1 : (Input.GetKey(KeyCode.A) ? -1 : 0),
            0,
            Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0));

        Mover.Move(movementInput.normalized);
    }
}
