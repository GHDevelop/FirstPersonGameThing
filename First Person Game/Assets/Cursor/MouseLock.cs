using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple component that locks the mouse cursor to the game window
/// </summary>
public class MouseLock : MonoBehaviour
{
    private void Awake()
    {
        LockCursor();
    }

    private void OnMouseDown()
    {
        LockCursor();
    }

    /// <summary>
    /// Locks cursor to the game window, and disables it from view
    /// </summary>
    private static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
