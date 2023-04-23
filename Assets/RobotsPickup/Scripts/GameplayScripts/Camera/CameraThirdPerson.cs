using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThirdPerson : PlayerCameraState
{
    private Transform target;
    private float _mouseSense = 1.8f;

    private void OnEnable()
    {
        if(target)
        {
            Vector3 angle = transform.localEulerAngles;
            angle.y = target.eulerAngles.y;
            transform.eulerAngles = angle;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetTarget();
    }

    private void SetTarget()
    {
        target = transform.parent;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        FollowTarget();
        RotateWithMouse();
        RotateFaceToCamTfs();
    }

    private void FollowTarget()
    {
        if(target)
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
    }

    private void RotateWithMouse()
    {
        //// Pitch
        //transform.rotation *= Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * _mouseSense, Vector3.right);

        // Paw
        transform.rotation = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y + Input.GetAxis("Mouse X") * _mouseSense,
            transform.eulerAngles.z
        );
    }

    private void RotateFaceToCamTfs()
    {
        foreach(var tf in FaceToCamera.faceToCameraTfs)
        {
            tf.LookAt(cam);
            //tf.eulerAngles = new Vector3(0f, 0f,0f);
            tf.eulerAngles = new Vector3(0f, tf.eulerAngles.y, 0f);
        }
    }
}
