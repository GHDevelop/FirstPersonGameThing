using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LateUpdaterForTransformInterpolater))]
public class TransformInterpolater : MonoBehaviour
{
    [System.Serializable]
    private struct TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformData (Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }

    [Tooltip("The transform of the object on the last update frame"),
        SerializeField, VisibleOnly] private TransformData _previousTransform;
    private TransformData PreviousTransform
    {
        get { return _previousTransform; }
        set { _previousTransform = value; }
    }

    [Tooltip("The transform of the object on the current update frame"),
        SerializeField, VisibleOnly] private TransformData _currentTransform;
    private TransformData CurrentTransform
    {
        get { return _currentTransform; }
        set { _currentTransform = value; }
    }

    [Tooltip("A cache of the transform the object is attached to"),
        SerializeField, VisibleOnly] private Transform _objectTransform;
    private Transform ObjectTransform
    {
        get { return _objectTransform; }
        set { _objectTransform = value; }
    }

    private void Awake()
    {
        ObjectTransform = transform;
    }

    private void Update()
    {
        float interpolationFactor = InterpolationGenerator.InterpolationFactor;

        ObjectTransform.localPosition = Vector3.Lerp(PreviousTransform.position,
                                                     CurrentTransform.position,
                                                     interpolationFactor);

        ObjectTransform.localRotation = Quaternion.Slerp(PreviousTransform.rotation,
                                                         CurrentTransform.rotation,
                                                         interpolationFactor);

        ObjectTransform.localScale = Vector3.Lerp(PreviousTransform.scale,
                                                  CurrentTransform.scale,
                                                  interpolationFactor);
    }

    private void FixedUpdate()
    {
        UpdateTransformFromData(CurrentTransform);
    }

    public void LateFixedUpdateButNotReally()
    {
        PreviousTransform = CurrentTransform;
        CurrentTransform = GetTransformDataFromObject();
    }

    private void OnEnable()
    {
        ForgetTransforms();
    }

    public void ForgetTransforms()
    {
        TransformData transformData = GetTransformDataFromObject();

        PreviousTransform = transformData;
        CurrentTransform = transformData;
    }

    private TransformData GetTransformDataFromObject()
    {
        return new TransformData(ObjectTransform.localPosition,
                                 ObjectTransform.localRotation,
                                 ObjectTransform.localScale);
    }

    private void UpdateTransformFromData(TransformData data)
    {
        ObjectTransform.localPosition = data.position;
        ObjectTransform.localRotation = data.rotation;
        ObjectTransform.localScale = data.scale;
    }
}
