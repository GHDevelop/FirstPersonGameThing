using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A standard mover. Moves in specified direction based on amount.
/// </summary>
public class FirstPersonMover : BaseMover
{
    [System.Serializable]
    public struct FirstPersonMovementSpeedSet
    {
        [Tooltip("Forward movement speed"),
            SerializeField] private SpeedAccelerationDeceleration _forwardSpeed;
        public SpeedAccelerationDeceleration ForwardSpeed
        {
            get { return _forwardSpeed; }
            private set { _forwardSpeed = value; }
        }

        [Tooltip("Backward movement speed"),
            SerializeField] private SpeedAccelerationDeceleration _backwardSpeed;
        public SpeedAccelerationDeceleration BackwardSpeed
        {
            get { return _backwardSpeed; }
            private set { _backwardSpeed = value; }
        }

        [Tooltip("Sideways movement speed"),
            SerializeField] private SpeedAccelerationDeceleration _sidewaysSpeed;
        public SpeedAccelerationDeceleration SidewaysSpeed
        {
            get { return _sidewaysSpeed; }
            private set { _sidewaysSpeed = value; }
        }

        [Tooltip("Vertical movement speed. Special actions such as \"jumping\" not included"),
            SerializeField] private SpeedAccelerationDeceleration _verticalSpeed;
        public SpeedAccelerationDeceleration VerticalSpeed
        {
            get { return _verticalSpeed; }
            private set { _verticalSpeed = value; }
        }

        public FirstPersonMovementSpeedSet(SpeedAccelerationDeceleration forwardSpeed, SpeedAccelerationDeceleration backwardSpeed, SpeedAccelerationDeceleration sidewaysSpeed, SpeedAccelerationDeceleration verticalSpeed)
        {
            _forwardSpeed = forwardSpeed;
            _backwardSpeed = backwardSpeed;
            _sidewaysSpeed = sidewaysSpeed;
            _verticalSpeed = verticalSpeed;
        }
    }

    [Header("Properties")]

    [Tooltip("All the speeds used by this mover"),
        SerializeField] private FirstPersonMovementSpeedSet _speeds;
    public FirstPersonMovementSpeedSet Speeds
    {
        get { return _speeds; }
        private set { _speeds = value; }
    }

    public override void Move(Vector3 input)
    {
        UpdateMovementSpeed(input);
        UpdatePosition();
    }

    #region velocity
    protected virtual Vector3 UpdateMovementSpeed(Vector3 input)
    {
        Vector3 newMovementSpeed = new Vector3(GetSidewaysVelocity(input), GetVerticalVelocity(input), GetForwardVelocity(input));
        MovementSpeed = newMovementSpeed;

        return newMovementSpeed;
    }

    /// <summary>
    /// Gets forward velocity after input
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <seealso cref="GetSidewaysVelocity(Vector3)"/>
    /// <seealso cref="GetVerticalVelocity(Vector3)"/>
    protected virtual float GetForwardVelocity(Vector3 input)
    {
        float acceleration = CalculateAbnormalAcceleration(input.z, MovementSpeed.z, Speeds.ForwardSpeed.Acceleration, Speeds.BackwardSpeed.Acceleration, Speeds.ForwardSpeed.Deceleration, Speeds.BackwardSpeed.Deceleration);
        float velocity = CalculateVelocityFromAcceleration(acceleration, MovementSpeed.z, -Speeds.BackwardSpeed.MaxSpeed, Speeds.ForwardSpeed.MaxSpeed);

        return velocity;
    }

    /// <summary>
    /// Gets sideways velocity after input
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <seealso cref="GetForwardVelocity(Vector3)"/>
    /// <seealso cref="GetVerticalVelocity(Vector3)"/>
    protected virtual float GetSidewaysVelocity(Vector3 input)
    {
        float acceleration = CalculateNormalAcceleration(input.x, MovementSpeed.x, Speeds.SidewaysSpeed.Acceleration, Speeds.SidewaysSpeed.Deceleration);
        float velocity = CalculateVelocityFromAcceleration(acceleration, MovementSpeed.x, -Speeds.SidewaysSpeed.MaxSpeed, Speeds.SidewaysSpeed.MaxSpeed);

        return velocity;
    }

    /// <summary>
    /// Gets vertical velocity after input
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// /// <seealso cref="GetForwardVelocity(Vector3)"/>
    /// <seealso cref="GetSidewaysVelocity(Vector3)"/>
    protected virtual float GetVerticalVelocity(Vector3 input)
    {
        float acceleration = CalculateNormalAcceleration(input.y, MovementSpeed.y, Speeds.VerticalSpeed.Acceleration, Speeds.VerticalSpeed.Deceleration);
        float velocity = CalculateVelocityFromAcceleration(acceleration, MovementSpeed.y, -Speeds.VerticalSpeed.MaxSpeed, Speeds.VerticalSpeed.MaxSpeed);

        return velocity;
    }

    /// <summary>
    /// Takes an acceleration, current speed on an axis, and min/max speed to calculate velocity.<br/>
    /// </summary>
    /// <param name="acceleration"></param>
    /// <param name="currentSpeed"></param>
    /// <param name="minSpeed"></param>
    /// <param name="maxSpeed"></param>
    /// <returns></returns>
    protected virtual float CalculateVelocityFromAcceleration(float acceleration, float currentSpeed, float minSpeed, float maxSpeed)
    {
        return Mathf.Clamp(currentSpeed + acceleration, minSpeed, maxSpeed);
    }

    /// <summary>
    /// Calculates acceleration for normal axes, where positive and negative directions use the same speed.
    /// Acceleration is adjusted for frame rate here, so do not multiply it by <see cref="Time.deltaTime"/> prior
    /// </summary>
    /// <param name="inputOnAxis"></param>
    /// <param name="currentSpeedOnAxis"></param>
    /// <param name="baseAcceleration"></param>
    /// <param name="deceleration"></param>
    /// <returns></returns>
    /// <seealso cref="CalculateAbnormalAcceleration(float, float, float, float, float, float)"/>
    protected virtual float CalculateNormalAcceleration(float inputOnAxis, float currentSpeedOnAxis, float baseAcceleration, float deceleration)
    {
        float currentSpeedAbs = Mathf.Abs(currentSpeedOnAxis);
        float acceleration = inputOnAxis != 0 ?
            inputOnAxis * baseAcceleration * Time.deltaTime :
            Mathf.Clamp(Mathf.Sign(currentSpeedOnAxis) * -deceleration * Time.deltaTime, -currentSpeedAbs, currentSpeedAbs);

        return acceleration;
    }

    /// <summary>
    /// Calculates acceleration for abnormal axes, where positive and negative directions use different speed.
    /// Acceleration is adjusted for frame rate here, so do not multiply it by <see cref="Time.deltaTime"/> prior
    /// </summary>
    /// <param name="inputOnAxis"></param>
    /// <param name="currentSpeedOnAxis"></param>
    /// <param name="baseForwardAcceleration"></param>
    /// <param name="baseBackwardAcceleration"></param>
    /// <param name="baseForwardDeceleration"></param>
    /// <param name="baseBackwardDeceleration"></param>
    /// <returns></returns>
    /// <seealso cref="CalculateNormalAcceleration(float, float, float, float)"/>
    protected virtual float CalculateAbnormalAcceleration(float inputOnAxis, float currentSpeedOnAxis, float baseForwardAcceleration, float baseBackwardAcceleration, float baseForwardDeceleration, float baseBackwardDeceleration)
    {
        float acceleration;
        if (inputOnAxis != 0)
        {
            acceleration = (inputOnAxis > 0 ?
                baseForwardAcceleration :
                baseBackwardAcceleration)
                * inputOnAxis * Time.deltaTime;
        }
        else
        {
            acceleration = (currentSpeedOnAxis > 0 ?
                Mathf.Clamp(baseForwardDeceleration * Time.deltaTime, -currentSpeedOnAxis, currentSpeedOnAxis) :
                Mathf.Clamp(baseBackwardDeceleration * Time.deltaTime, currentSpeedOnAxis, -currentSpeedOnAxis))
                * -Mathf.Sign(currentSpeedOnAxis);
        }

        return acceleration;
    }
#endregion
}
