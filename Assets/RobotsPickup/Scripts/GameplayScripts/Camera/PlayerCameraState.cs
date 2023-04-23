using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraState : MonoBehaviour
{
    private static PlayerCameraState cameraState;
    [SerializeField] internal Transform cam;

    public static void EnableCameraMovement(bool isEnable)
    {
        if (cameraState != null)
            cameraState.enabled = isEnable;
    }

    public void EnableMovement()
    {
        SetCursor.EnableCursor(true);
        cameraState.enabled = false;
        cameraState = this;
        enabled = true;
    }
}
