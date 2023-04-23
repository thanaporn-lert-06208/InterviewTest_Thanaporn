using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject[] cameras;
    private int cameraIndex = -1;

    private void Awake()
    {
        HideCam();
    }

    public Transform SwitchCamera()
    {
        if (cameras.Length < 1) return null;

        if (cameraIndex > -1 && cameraIndex < cameras.Length)
            cameras[cameraIndex].gameObject.SetActive(false);

        cameraIndex++;
        if (cameraIndex >= cameras.Length)
            cameraIndex = 0;

        ActiveCamera();

        return cameras[cameraIndex].transform;
    }

    public void ActiveCamera()
    {
        cameras[cameraIndex].SetActive(true);
    }

    public void HideCam()
    {
        foreach(var cam in cameras)
        {
            cam.SetActive(false);
        }
    }
}
