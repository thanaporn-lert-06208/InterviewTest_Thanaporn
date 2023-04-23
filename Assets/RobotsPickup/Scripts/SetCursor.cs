using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SetCursor
{
    private static CursorLockMode _wantedMode;
    public static void EnableCursor(bool enableCursor)
    {
        if (enableCursor)
        {
            Cursor.lockState = _wantedMode = CursorLockMode.None;
            //_active = false;
        }

        /*if (Input.GetMouseButtonDown(0))
        {
            _wantedMode = CursorLockMode.Locked;
        }*/

        else
        {
            _wantedMode = CursorLockMode.Locked;
            //_active = true;
        }

        // Apply cursor state
        Cursor.lockState = _wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != _wantedMode);
    }
}
