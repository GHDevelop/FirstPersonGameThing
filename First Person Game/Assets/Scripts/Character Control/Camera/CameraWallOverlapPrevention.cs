using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWallOverlapPrevention : MonoBehaviour
{
    [Tooltip("The object the camera pivots around (the pivot rotates and the camera's position is determined based on the pivot" +
        "\nSet automatically if not set prior"),
        SerializeField] private Transform _pivot;
    public Transform Pivot
    {
        get { return _pivot; }
        private set { _pivot = value; }
    }

    [Tooltip("The transform of the camera. Set automatically if not set prior"),
        SerializeField] private Transform _cameraTransform;
    public Transform CameraTransform
    {
        get { return _cameraTransform; }
        private set { _cameraTransform = value; }
    }

    [Tooltip("The distance the camera should be from the pivot, determined based on the camera's local position"),
        SerializeField, VisibleOnly] private float _distanceFromPivotToCamera;
    public float DistanceFromPivotToCamera
    {
        get { return _distanceFromPivotToCamera; }
        private set { _distanceFromPivotToCamera = value; }
    }

    [Tooltip("The default position of the camera relative to the pivot"),
        SerializeField, VisibleOnly] private Vector3 _defaultOffsetOfCameraFromPivot;
    public Vector3 DefaultOffsetOfCameraFromPivot
    {
        get { return _defaultOffsetOfCameraFromPivot; }
        private set { _defaultOffsetOfCameraFromPivot = value; }
    }

    private void Awake()
    {
        if (CameraTransform == null)
        {
            CameraTransform = transform;
        }
        if (Pivot == null)
        {
            Pivot = CameraTransform.parent;
        }

        if (Pivot == null)
        {
            Debug.LogError("No pivot was set for the camera, nor was there a parent on the camera to use as a pivot");
            return;
        }

        DefaultOffsetOfCameraFromPivot = CameraTransform.position - Pivot.position;
        DistanceFromPivotToCamera = DefaultOffsetOfCameraFromPivot.magnitude;

        StartCoroutine(KeepCameraOutOfWall());
    }

    protected IEnumerator KeepCameraOutOfWall()
    {
        while (true)
        {
            Vector3 localCameraOffset = Pivot.TransformDirection(DefaultOffsetOfCameraFromPivot);
            Vector3 newCameraPosition = Pivot.position + localCameraOffset;

            RaycastHit hit;
            if (Physics.Raycast(Pivot.position, localCameraOffset.normalized, out hit, DistanceFromPivotToCamera))
            {
                newCameraPosition = hit.point;
            }

            CameraTransform.position = newCameraPosition;

            yield return new WaitForEndOfFrame();
        }
    }
}
