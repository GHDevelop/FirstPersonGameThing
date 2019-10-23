using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An extension of <see cref="BaseCameraMover"/> that rotates the object on the Y axis instead of the camera.
/// </summary>
/// <seealso cref="BaseCameraMover"/>
public class FirstPersonCameraMover : BaseCameraMover
{
    /// <summary>
    /// Rotates the object on the Y axis, and the Camera on the X and Z axes (using Euler Angles)
    /// </summary>
    protected override void ApplyRotation()
    {
        Vector3 transformEulers = CameraMoverTransform.localEulerAngles;
        CameraMoverTransform.localRotation = Quaternion.Euler(transformEulers.x, CurrentCameraRotation.y, transformEulers.z);

        CameraTransform.localRotation = Quaternion.Euler(CurrentCameraRotation.x,
                                                         CameraTransform.localRotation.y,
                                                         CurrentCameraRotation.z);
    }
}
