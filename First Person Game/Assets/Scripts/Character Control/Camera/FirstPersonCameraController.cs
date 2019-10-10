using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : BaseCameraController
{
    protected override void ApplyRotation()
    {
        Vector3 cameraEulers = CameraMoverTransform.eulerAngles;
        CameraMoverTransform.rotation = Quaternion.Euler(cameraEulers.x, CurrentCameraRotation.y, cameraEulers.z);

        base.ApplyRotation();
    }
}
