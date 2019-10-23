using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component to attach to cameras (particularly third person cameras) that prevents them from moving behind walls by forcing them closer to the player.
/// </summary>
public class CameraWallOverlapPrevention : MonoBehaviour
{
    [Tooltip("The object the camera pivots around (the pivot rotates and the camera's position is determined based on the pivot" +
        "\nSet automatically if not set prior"),
        SerializeField] private Transform _pivot;
    /// <summary>
    /// The pivot that the camera is moved around. For example this might be a transform at the center of where the camera is looking
    /// </summary>
    private Transform Pivot
    {
        get { return _pivot; }
        set { _pivot = value; }
    }

    [Tooltip("The transform of the camera. Set automatically if not set prior"),
        SerializeField] private Transform _cameraTransform;
    /// <summary>
    /// The transform of the camera itself
    /// </summary>
    public Transform CameraTransform
    {
        get { return _cameraTransform; }
        private set { _cameraTransform = value; }
    }

    [Tooltip("The distance the camera should be from the pivot, determined based on the camera's local position"),
        SerializeField, VisibleOnly] private float _distanceFromPivotToCamera;
    /// <summary>
    /// The distance from the pivot to the camera at default
    /// </summary>
    public float DistanceFromPivotToCamera
    {
        get { return _distanceFromPivotToCamera; }
        private set { _distanceFromPivotToCamera = value; }
    }

    [Tooltip("The default position of the camera relative to the pivot"),
        SerializeField, VisibleOnly] private Vector3 _defaultOffsetOfCameraFromPivot;
    /// <summary>
    /// The default offset of the camera from the pivot. If the camera is a child of the pivot, then this would be the camera's local position
    /// </summary>
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

        DefaultOffsetOfCameraFromPivot = Pivot.InverseTransformPoint(CameraTransform.position);
        DistanceFromPivotToCamera = DefaultOffsetOfCameraFromPivot.magnitude;

        StartCoroutine(KeepCameraOutOfWall());
    }

    /// <summary>
    /// Raycasts from the pivot to the camera, and moves the camera to the raycast's hit location if it hits something
    /// </summary>
    /// <returns></returns>
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

            yield return null;
        }
    }
}
