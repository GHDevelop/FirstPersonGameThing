using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// There is no LateFixedUpdate in Unity, so this component (if set correctly in Script Execution Order) effectively functions as one for <see cref="TransformInterpolater"/>
/// </summary>
/// <seealso cref="TransformInterpolater"/>
public class LateUpdaterForTransformInterpolater : MonoBehaviour
{
    [Tooltip("The transform interpolater"),
        SerializeField, VisibleOnly] private TransformInterpolater _tfInterpolater;
    private TransformInterpolater TfInterpolater
    {
        get { return _tfInterpolater; }
        set { _tfInterpolater = value; }
    }

    private void Awake()
    {
        TfInterpolater = GetComponent(typeof(TransformInterpolater)) as TransformInterpolater;
    }

    private void FixedUpdate()
    {
        TfInterpolater.LateFixedUpdateButNotReally();
    }
}
