using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Because Unity only exposes <see cref="Time.time"/> and <see cref="Time.fixedTime"/> as floats, using them will
/// result in decreased precision over time. This is notable enough that multiple update frames could have the same <see cref="Time.time"/> result <br/>
/// This class provides double based alternatives to both of them that will preserve their precision for over a century
/// </summary>
public class TimeButBetter : MonoBehaviour
{
    private static TimeButBetter _self;
    /// <summary>
    /// Singleton
    /// </summary>
    private static TimeButBetter Self
    {
        get { return _self; }
        set { _self = value; }
    }

    /// <summary>
    /// The time the timer internally starts at. Used to preserve exact precision for longer by starting the timer internally at a higher value.
    /// </summary>
    private const double TIMESTART = 4294967296;

    [Tooltip("Thank you Unity for only exposing Time.time as a float. Really appreciate it." +
        "\nAnyways this timer should preserve its precision for longer than Time.time's return value. "),
        SerializeField, VisibleOnly] private double _updateTime = TIMESTART;
    /// <summary>
    /// The time since this instance started tracking time
    /// </summary>
    public double InternalUpdateTime
    {
        get { return _updateTime - TIMESTART; }
        private set { _updateTime = value + TIMESTART; }
    }
    /// <summary>
    /// <see cref="Time.time"/> alternative. Since Time.deltaTime uses repeating numbers, some precision may be lost
    /// </summary>
    /// <seealso cref="FixedUpdateTime"/>
    public static double UpdateTime
    {
        get { return Self.InternalUpdateTime; }
    }

    [Tooltip("This timer is like UpdateTime, but it's based on Fixed Update"),
        SerializeField, VisibleOnly] private double _fixedUpdateTime = TIMESTART;
    /// <summary>
    /// The time since this instance started tracking fixed time
    /// </summary>
    public double InternalFixedUpdateTime
    {
        get { return _fixedUpdateTime - TIMESTART; }
        private set { _fixedUpdateTime = value + TIMESTART; }
    }
    /// <summary>
    /// <see cref="Time.fixedTime"/> alternative. If you need to make a timer for something, using this is recommended.
    /// </summary>
    /// <seealso cref="UpdateTime"/>
    public static double FixedUpdateTime
    {
        get { return Self.InternalFixedUpdateTime; }
    }

    /// <summary>
    /// Initialize singleton, then initialize time and fixed time
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
            Debug.LogErrorFormat("Attempted to create TimeButBetter on {0}, but one already exists on {1}", this.gameObject.name, Self.gameObject.name);
            Destroy(this);
        }

        //Time.deltaTime substracted to give best accuracy to Time.time, while still retaining higher accessible precision
        InternalUpdateTime = Time.time - Time.deltaTime;
        InternalFixedUpdateTime = Time.fixedTime - Time.fixedDeltaTime;
    }

    /// <summary>
    /// Update fixed time
    /// </summary>
    private void FixedUpdate()
    {
        InternalFixedUpdateTime += Time.fixedDeltaTime;
    }

    /// <summary>
    /// Update time
    /// </summary>
    void Update()
    {
        InternalUpdateTime += Time.deltaTime;
    }
}
