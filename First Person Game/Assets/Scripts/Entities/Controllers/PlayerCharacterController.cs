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

    [Header("Player Character Controller Config")]

    [Tooltip("The controls the character uses"),
        SerializeField, VisibleOnly] private InGameControls _controls;
    /// <summary>
    /// The controls the character uses
    /// </summary>
    public InGameControls Controls
    {
        get { return _controls; }
        private set { _controls = value; }
    }

    [Tooltip("The number of places after the decimal to round input to"),
        SerializeField, Range(1, 8)] private byte _numDigitsAfterDecimalOnInput = 6;
    /// <summary>
    /// Input is rounded to this many digits after the decimal place
    /// </summary>
    public byte NumDigitsAfterDecimalOnInput
    {
        get { return _numDigitsAfterDecimalOnInput; }
        private set { _numDigitsAfterDecimalOnInput = value; }
    }

    [Tooltip("The number of places after the decimal to round input to, represented as a power of 10"),
        SerializeField, VisibleOnly] private int _multiplierUsedForRounding;
    /// <summary>
    /// Same as <see cref="NumDigitsAfterDecimalOnInput"/>, but represented as a power of 10
    /// </summary>
    public int MultiplierUsedForRounding
    {
        get { return _multiplierUsedForRounding; }
        private set { _multiplierUsedForRounding = value; }
    }

    [Header("Degrees of Input")]

    [Tooltip("How many degrees the player character can move in. Ideally should be multiple of 4"),
        SerializeField, Range(4, 360)] private int _numDegreesPlayerCanMoveIn = 16;
    /// <summary>
    /// The number of degrees that movement input will be recognized in
    /// </summary>
    public int NumDegreesPlayerCanMoveIn
    {
        get { return _numDegreesPlayerCanMoveIn; }
        private set { _numDegreesPlayerCanMoveIn = value; }
    }

    [Tooltip("The angle gap between directions the player can move in. This is calculated at runtime"),
        SerializeField, VisibleOnly] private float _degreeGapBetweenPlayerMovementAngles;
    /// <summary>
    /// The number of degrees between each of the player's possible movement angles
    /// </summary>
    public float DegreeGapBetweenPlayerMovementAngles
    {
        get { return _degreeGapBetweenPlayerMovementAngles; }
        private set { _degreeGapBetweenPlayerMovementAngles = value; }
    }

    [Tooltip("How many degrees the player character can move the camera in. Ideally should be multiple of 4"),
        SerializeField, Range(4, 360)]
    private int _numDegreesPlayerCanMoveCameraIn = 16;
    /// <summary>
    /// The number of degrees that camera input will be recognized in
    /// </summary>
    public int NumDegreesPlayerCanMoveCameraIn
    {
        get { return _numDegreesPlayerCanMoveCameraIn; }
        private set { _numDegreesPlayerCanMoveCameraIn = value; }
    }

    [Tooltip("The angle gap between directions the player can move the camera in. This is calculated at runtime"),
        SerializeField, VisibleOnly]
    private float _degreeGapBetweenPlayerCameraAngles;
    /// <summary>
    /// The number of degrees between each of the player's possible camera movement angles
    /// </summary>
    public float DegreeGapBetweenPlayerCameraAngles
    {
        get { return _degreeGapBetweenPlayerCameraAngles; }
        private set { _degreeGapBetweenPlayerCameraAngles = value; }
    }

    protected override void Awake()
    {
        base.Awake();
        CameraToggle = GetComponent(typeof(CameraToggle)) as CameraToggle;

        DegreeGapBetweenPlayerMovementAngles = 360.0f / NumDegreesPlayerCanMoveIn;
        DegreeGapBetweenPlayerCameraAngles = 360.0f / NumDegreesPlayerCanMoveCameraIn;
        MultiplierUsedForRounding = (int)Mathf.Pow(10.0f, NumDigitsAfterDecimalOnInput);

        InitializeControls();
    }

    private void InitializeControls()
    {
        Controls = new InGameControls();

        Controls.Gameplay.Move.performed += context => UpdateMovementInput(context.ReadValue<Vector2>());
        Controls.Gameplay.Move.canceled += context => UpdateMovementInput(Vector3.zero);

        Controls.Gameplay.Look.performed += context => UpdateCameraInput(context.ReadValue<Vector2>());
        Controls.Gameplay.Look.canceled += context => UpdateCameraInput(Vector3.zero);

        Controls.Gameplay.ToggleCamera.performed += context => ToggleCamera();
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
        Controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        Controls.Gameplay.Disable();
    }

    private void ToggleCamera()
    {
        CameraToggle.AdvanceCurrentCamera();
    }

    protected override void UpdateMovementInput(Vector3 input)
    {
        Vector3 snappedInput = RoundingMethods.RoundAngle(input, Vector3.forward, DegreeGapBetweenPlayerMovementAngles, MultiplierUsedForRounding);
        base.UpdateMovementInput(snappedInput);
    }

    protected override void UpdateCameraInput(Vector3 input)
    {
        Vector3 snappedInput = RoundingMethods.RoundAngle(input, Vector3.left, DegreeGapBetweenPlayerCameraAngles, MultiplierUsedForRounding);
        base.UpdateCameraInput(snappedInput);
    }
}
