using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of methods that can help round values.<br/>
/// Currently supports rounding <see cref="float"/>s, <see cref="Vector3"/> coordinates, and <see cref="Vector3"/> angles
/// </summary>
public static class RoundingMethods
{
    /// <summary>
    /// Rounds a <see cref="float"/> to X digits past the decimal place.
    /// </summary>
    /// <param name="value">The value you want to round</param>
    /// <param name="multiplier">A multiplier that is applied to make the value whole for rounding.<br/>
    /// As an example, if you want to round to 6 past the decimal you would use 1,000,000 and not 6</param>
    /// <returns></returns>
    public static float RoundFloatToXPastDecimal(float value, int multiplier)
    {
        return Mathf.Round(value * multiplier) / multiplier;
    }

    /// <summary>
    /// Similar to <see cref="RoundFloatToXPastDecimal(float, int)"/>, but it takes a vector3 and rounds all 3 coordinates contained within
    /// </summary>
    /// <param name="vector">The vector you want rounded</param>
    /// <param name="multiplier">A multiplier that is applied to make the value whole for rounding.<br/>
    /// As an example, if you want to round to 6 past the decimal you would use 1,000,000 and not 6</param>
    /// <seealso cref="RoundFloatToXPastDecimal(float, int)"/>
    /// <returns></returns>
    public static Vector3 RoundVector3ToXPastDecimal(Vector3 vector, int multiplier)
    {
        return new Vector3(RoundFloatToXPastDecimal(vector.x, multiplier),
                           RoundFloatToXPastDecimal(vector.y, multiplier),
                           RoundFloatToXPastDecimal(vector.z, multiplier));
    }

    /// <summary>
    /// Rounds a <see cref="Vector3"/>'s angle to the nearest of 360 / <paramref name="differenceBetweenAngles"/> angles.<br/>
    /// Also rounds the resulting <see cref="Vector3"/> to a number of digits using <see cref="RoundVector3ToXPastDecimal(Vector3, int)"/>
    /// </summary>
    /// <param name="vector">The <see cref="Vector3"/> you want to snap to an angle</param>
    /// <param name="forwardDirection">The direction that <paramref name="vector"/> considers to be 0 degrees.<br/>
    /// For example, movement input for a character in 3D space would likely use <see cref="Vector3.forward"/></param>
    /// <param name="differenceBetweenAngles">The difference between angles. Should be 360 / number of possible degrees</param>
    /// <param name="roundingMultiplier">A multiplier that is applied to make the value whole for rounding.<br/>
    /// As an example, if you want to round to 6 past the decimal you would use 1,000,000 and not 6</param>
    /// <returns></returns>
    public static Vector3 RoundAngle(Vector3 vector, Vector3 forwardDirection, float differenceBetweenAngles, int roundingMultiplier = 1000000)
    {
        float initialAngle = Vector3.Angle(vector, forwardDirection);

        //Cross Product doesn't work with 0 degree angles
        if (initialAngle < differenceBetweenAngles / 2.0f)
        {
            return forwardDirection * RoundFloatToXPastDecimal(vector.magnitude, roundingMultiplier);
        }
        //Or 180 degree angles
        if (initialAngle > 180 - differenceBetweenAngles / 2.0f)
        {
            return -forwardDirection * RoundFloatToXPastDecimal(vector.magnitude, roundingMultiplier);
        }

        float rotation = CalculateRotationToSnap(differenceBetweenAngles, initialAngle);
        Vector3 axis = Vector3.Cross(forwardDirection, vector);

        Vector3 newVector = CalculateSnappedVectorFromRotationAndAxis(vector, rotation, axis);
        newVector = RoundVector3ToXPastDecimal(newVector, roundingMultiplier);
        return newVector;
    }

    /// <summary>
    /// Helper for <see cref="RoundAngle(Vector3, Vector3, float, int)"/> that calculates the rotation needed for the angle to snap
    /// </summary>
    /// <param name="differenceBetweenAngles"></param>
    /// <param name="initialAngle"></param>
    /// <seealso cref="RoundAngle(Vector3, Vector3, float, int)"/>
    /// <returns></returns>
    private static float CalculateRotationToSnap(float differenceBetweenAngles, float initialAngle)
    {
        float angleDividedByDifferenceRounded = Mathf.Round(initialAngle / differenceBetweenAngles);
        float rotation = angleDividedByDifferenceRounded * differenceBetweenAngles - initialAngle;
        return rotation;
    }

    /// <summary>
    /// Helper for <see cref="RoundAngle(Vector3, Vector3, float, int)"/> that calculates the vector after snapping
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="rotation"></param>
    /// <param name="axis"></param>
    /// <seealso cref="RoundAngle(Vector3, Vector3, float, int)"/>
    /// <returns></returns>
    private static Vector3 CalculateSnappedVectorFromRotationAndAxis(Vector3 vector, float rotation, Vector3 axis)
    {
        Quaternion rotationAtAngle = Quaternion.AngleAxis(rotation, axis);
        Vector3 newVector = rotationAtAngle * vector;
        return newVector;
    }
}
