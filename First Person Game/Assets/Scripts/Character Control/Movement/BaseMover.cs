using System;
using UnityEngine;
public abstract class BaseMover : MonoBehaviour
{
    [System.Serializable]
    public struct SpeedAccelerationDeceleration
    {
        [Tooltip("The speed to move at"),
            SerializeField] private float _maxSpeed;
        public float MaxSpeed
        {
            get { return _maxSpeed; }
            private set { _maxSpeed = value; }
        }

        [Tooltip("The rate to accelerate"),
            SerializeField] private float _acceleration;
        public float Acceleration
        {
            get { return _acceleration; }
            private set { _acceleration = value; }
        }

        [Tooltip("The rate to decelerate"),
            SerializeField] private float _deceleration;
        public float Deceleration
        {
            get { return _deceleration; }
            private set { _deceleration = value; }
        }

        public SpeedAccelerationDeceleration(float speed, float acceleration, float deceleration)
        {
            _maxSpeed = speed;
            _acceleration = acceleration;
            _deceleration = deceleration;
        }
    }

    [Header("Universal Movement Properties")]

    [Tooltip("The transform of the attached object"),
        SerializeField, VisibleOnly] private Transform _moverTransform;
    /// <summary>
    /// Transform of the mover's game object
    /// </summary>
    public Transform MoverTransform
    {
        get { return _moverTransform; }
        private set { _moverTransform = value; }
    }

    [Tooltip("The collider associated with this mover"),
        SerializeField, VisibleOnly] private Collider _moverCollider;
    public Collider MoverCollider
    {
        get { return _moverCollider; }
        private set { _moverCollider = value; }
    }

    [Tooltip("The change applied to the unit's position on that frame"),
        SerializeField, VisibleOnly] private Vector3 _movementSpeed;
    /// <summary>
    /// The current movement speed. This is in local terms, so Z is the character's forward, X is the character's right, and Y is the character's up.
    /// </summary>
    public Vector3 MovementSpeed
    {
        get { return _movementSpeed; }
        protected set { _movementSpeed = value; }
    }

    [Tooltip("The change applied to the unit's rotation on that frame in eulers"),
        SerializeField, VisibleOnly] private Vector3 _rotationSpeed;
    /// <summary>
    /// The rotation speed
    /// </summary>
    /// <seealso cref="RotationSpeedQuaternion"/>
    public Vector3 RotationSpeed
    {
        get { return _rotationSpeed; }
        protected set { _rotationSpeed = value; }
    }
    /// <summary>
    /// The rotation speed from <see cref="RotationSpeed"/> but as a quaternion
    /// </summary>
    /// <seealso cref="RotationSpeed"/>
    public Quaternion RotationSpeedQuaternion
    {
        get { return Quaternion.Euler(_rotationSpeed); }
        protected set { _rotationSpeed = value.eulerAngles; }
    }

    [Tooltip("The maximum number of times to iterate through collider checks before moving on with each step"),
        SerializeField] private byte _numColliderChecksPerStep = 5;
    public byte NumColliderChecksPerStep
    {
        get { return _numColliderChecksPerStep; }
        private set { _numColliderChecksPerStep = value; }
    }

    [Tooltip("Whether to reset speed after making contact with a wall on relevant axes" +
        "\ndoes not play well with corners"),
        SerializeField] private bool _resetSpeedOnBonk;
    public bool ResetSpeedOnBonk
    {
        get { return _resetSpeedOnBonk; }
        private set { _resetSpeedOnBonk = value; }
    }

    private void Awake()
    {
        MoverTransform = transform;
        MoverCollider = GetComponent(typeof(Collider)) as Collider;
    }

    /// <summary>
    /// Moves unit based on X, Y, and Z input.<br/>
    /// A player controller might use X as the thumbstick's left and right, 
    /// Z as the thumbstick's up and down, 
    /// and Y as either the result of combining 2 buttons or ignored.
    /// </summary>
    /// <param name="input"></param>
    public abstract void Move(Vector3 input);

    protected void UpdatePosition()
    {
        Vector3 newPosition = MoverTransform.position;
        newPosition += MoverTransform.right * MovementSpeed.x * Time.deltaTime;
        newPosition += MoverTransform.up * MovementSpeed.y * Time.deltaTime;
        newPosition += MoverTransform.forward * MovementSpeed.z * Time.deltaTime;

        //did not move
        if (newPosition == MoverTransform.position)
        {
            return;
        }

        newPosition = StepAndCheckColliders(newPosition);

        MoverTransform.position = newPosition;
    }

    private Vector3 StepAndCheckColliders(Vector3 newPosition)
    {
        for (int index = 0; index < NumColliderChecksPerStep; index++)
        {
            Collider[] colliders = CollisionRetrievers.GetColliderOverlap(MoverTransform.position, MoverCollider);
            Vector3 newerPosition = DepenetrateFromColliderSet(newPosition, colliders);

            newPosition = newerPosition;
        }

        return newPosition;
    }

    private Vector3 DepenetrateFromColliderSet(Vector3 newPosition, Collider[] colliders)
    {
        for (int colliderIndex = 0; colliderIndex < colliders.Length - 1; colliderIndex++)
        {
            Collider collider = colliders[colliderIndex];
            newPosition = PreventJankIfCoordIsNearZero(DepenetrateFromCollider(newPosition, collider));
        }

        return newPosition;
    }

    private Vector3 DepenetrateFromCollider(Vector3 newPosition, Collider collider)
    {
        Vector3 newDirection;
        float newDistance;
        bool penetrated = Physics.ComputePenetration(
            MoverCollider,
            newPosition,
            MoverTransform.rotation,
            collider,
            collider.transform.position,
            collider.transform.rotation,
            out newDirection,
            out newDistance);

        Vector3 depenetrationCorrection = newDirection * newDistance;
        newPosition += depenetrationCorrection;
        if (ResetSpeedOnBonk && penetrated)
        {
            ResetSpeedOnAxesThatHitWall(newDirection);
        }
        return newPosition;
    }

    private void ResetSpeedOnAxesThatHitWall(Vector3 forceOutAxes)
    {
        forceOutAxes = forceOutAxes.normalized;

        MovementSpeed = new Vector3(forceOutAxes.x == 0 ? MovementSpeed.x : 0,
                                    forceOutAxes.y == 0 ? MovementSpeed.y : 0,
                                    forceOutAxes.z == 0 ? MovementSpeed.z : 0);
    }

    private Vector3 PreventJankIfCoordIsNearZero(Vector3 value)
    {
        return new Vector3(ConvertNearZeroToZero(value.x),
                           ConvertNearZeroToZero(value.y),
                           ConvertNearZeroToZero(value.z));
    }

    private float ConvertNearZeroToZero(float value)
    {
        return Mathf.Abs(value) >= 0.0000005f ? value : 0;
    }
}