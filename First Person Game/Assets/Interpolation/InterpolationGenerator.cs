using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that tracks an interpolation factor that is to be used when interpolating
/// </summary>
public class InterpolationGenerator : MonoBehaviour
{
    private static InterpolationGenerator _self;
    /// <summary>
    /// A singleton of the interpolation manager. Used to make sure multiple interpolation managers can't exist in a scene
    /// </summary>
    private static InterpolationGenerator Self
    {
        get { return _self; }
        set { _self = value; }
    }

    [Tooltip("The index of the newer of the 2 fixed update times."),
        SerializeField, VisibleOnly] private double _newTime;
    /// <summary>
    /// The newer of the 2 times used for interpolation
    /// </summary>
    private double NewTime
    {
        get { return _newTime; }
        set { _newTime = value; }
    }

    [Tooltip("The index of the older of the 2 fixed update times."),
        SerializeField, VisibleOnly]
    private double _oldTime;
    /// <summary>
    /// The older of the 2 times used for interpolation
    /// </summary>
    private double OldTime
    {
        get { return _oldTime; }
        set { _oldTime = value; }
    }

    [Tooltip("The interpolation factor used. This is based on the last 2 fixed update times"),
        SerializeField, VisibleOnly] private float _interpolationFactor;
    /// <summary>
    /// the interpolation factor tied to the instance of the InterpolationManager
    /// </summary>
    private float InternalInterpolationFactor
    {
        get { return _interpolationFactor; }
        set { _interpolationFactor = value; }
    }

    /// <summary>
    /// The factor by which to interpolate in other interpolation classes
    /// </summary>
    public static float InterpolationFactor
    {
        get { return Self.InternalInterpolationFactor; }
    }

    /// <summary>
    /// When created: initializes InterpolationManager as singleton, or destroys it if not present
    /// </summary>
    private void Awake()
    {
        if (Self == null)
        {
            Self = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.LogErrorFormat("Attempted to create InterpolationManager on {0}, but one already exists on {1}", this.gameObject.name, Self.gameObject.name);
            Destroy(this);
        }
    }

    /// <summary>
    /// Every Fixed Update: Sets the old fixed update time to the new fixed update time, then sets the new fixed update time to the time of this fixed update
    /// </summary>
    private void FixedUpdate()
    {
        OldTime = NewTime;
        NewTime = TimeButBetter.FixedUpdateTime;
    }

    /// <summary>
    /// Every Update: Calculates an interpolation factor from the times of the last 2 fixed updates
    /// </summary>
    private void Update()
    {
        if (NewTime != OldTime)
        {
            InternalInterpolationFactor = (float)((TimeButBetter.UpdateTime - NewTime) / (NewTime - OldTime));
        }
        else
        {
            InternalInterpolationFactor = 1;
        }
    }
}
