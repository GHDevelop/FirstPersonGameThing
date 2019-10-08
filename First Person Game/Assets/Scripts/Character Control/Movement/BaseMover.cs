using System.Collections;
using System.Collections.Generic;
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
        SerializeField, VisibleOnly] private Collider _associatedCollider;
    public Collider AssociatedCollider
    {
        get { return _associatedCollider; }
        private set { _associatedCollider = value; }
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
        set { _rotationSpeed = value.eulerAngles; }
    }

    private void Awake()
    {
        MoverTransform = transform;
        AssociatedCollider = GetComponent(typeof(Collider)) as Collider;
        Application.targetFrameRate = 120;
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

        Vector3 positionDifference = newPosition - MoverTransform.position;
        RaycastHit[] collisions = CollisionRetrievers.GetCollisions(MoverTransform, AssociatedCollider, positionDifference.normalized, positionDifference.magnitude);
        foreach (RaycastHit collision in collisions)
        {
            if (collision.collider.gameObject.isStatic)
            {
                Vector3 newDirection;
                float newDistance;
                Physics.ComputePenetration(
                    AssociatedCollider, 
                    MoverTransform.position, 
                    MoverTransform.rotation, 
                    collision.collider, 
                    collision.transform.position, 
                    collision.transform.rotation, 
                    out newDirection, 
                    out newDistance);
                newPosition = MoverTransform.position + newDirection * newDistance;
                
                break;
            }
        }

        MoverTransform.position = newPosition;
    }
}


//Scrapped code
/*Vector3 positionDifference = newPosition - MoverTransform.position;
        float distance = positionDifference.magnitude;

        Vector3 closestCollisionInDirection = AssociatedCollider.ClosestPointOnBounds(MoverTransform.position + positionDifference.normalized);
        Debug.DrawLine(MoverTransform.position, closestCollisionInDirection + positionDifference);

        RaycastHit hit;
        if (Physics.Raycast(closestCollisionInDirection, positionDifference.normalized, out hit, distance))
        {
            float offset = 0.001f;
            Vector3 minisculePositionOffset = new Vector3(
                positionDifference.x == 0 ? 0 : Mathf.Sign(positionDifference.x) * offset,
                positionDifference.y == 0 ? 0 : Mathf.Sign(positionDifference.y) * offset,
                positionDifference.z == 0 ? 0 :Mathf.Sign(positionDifference.z) * offset);


            Vector3 difference = (closestCollisionInDirection - MoverTransform.position);
            newPosition = (hit.point - difference) - minisculePositionOffset;
        }
        
     private Vector3 GetNewPositionAdjustedForCollision(Vector3 positionDifference, RaycastHit collision)
    {
        Vector3 closestCollisionInDirection = AssociatedCollider.ClosestPointOnBounds(collision.point);
        Vector3 difference = (closestCollisionInDirection - MoverTransform.position);
        Debug.Log(difference.magnitude);

        debugSphere.position = collision.point;
        debugSphere2.position = closestCollisionInDirection;
        return collision.point - difference;// + GetMinisculePositionOffset(positionDifference);
    }

    private Vector3 GetMinisculePositionOffset(Vector3 positionDifference)
    {
        float offset = 0.00001f;
        Vector3 minisculePositionOffset = new Vector3(
            positionDifference.x == 0 ? 0 : Mathf.Sign(positionDifference.x) * offset,
            positionDifference.y == 0 ? 0 : Mathf.Sign(positionDifference.y) * offset,
            positionDifference.z == 0 ? 0 : Mathf.Sign(positionDifference.z) * offset);

        return minisculePositionOffset;
    }*/
