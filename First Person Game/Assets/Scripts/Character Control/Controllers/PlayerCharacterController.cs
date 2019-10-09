using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : BaseController
{
    [Tooltip("The character's mover property"),
        SerializeField, VisibleOnly] private BaseMover _mover;
    public BaseMover Mover
    {
        get { return _mover; }
        private set { _mover = value; }
    }

    private void Awake()
    {
        Mover = GetComponent(typeof(BaseMover)) as BaseMover;
    }

    protected override void Update()
    {
        Vector3 movementInput = new Vector3(
            Input.GetKey(KeyCode.D) ? 1 : (Input.GetKey(KeyCode.A) ? -1 : 0),
            0,
            Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0));

        Mover.Move(movementInput.normalized);
    }
}
