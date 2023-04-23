using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTopdown : PlayerCameraState
{
    private Transform target;

    // Start is called before the first frame update
    void Awake()
    {
        Init();
    }

    private void Init()
    {
        target = transform.parent;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    private void OnEnable()
    {
        RotateFaceToCamTfs();
    }


    private void FollowTarget()
    {
        transform.position = target.position;
    }
    private void RotateFaceToCamTfs()
    {
        Debug.Log("rotate faceToCam topdown");
        foreach (var tf in FaceToCamera.faceToCameraTfs)
        {
            tf.LookAt(transform);
            
            //tf.eulerAngles = new Vector3(0f, tf.eulerAngles.y, 0f);
        }
    }
}
