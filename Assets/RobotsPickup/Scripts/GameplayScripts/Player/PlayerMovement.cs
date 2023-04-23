using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool isEnableControl;
    private Transform cameraPivot;
    private float speed;
    //private CharacterController characterController;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    void Update()
    {
        if (isEnableControl)
            Move();
    }

    public void SetEnableControl(bool isEnableControl)
    {
        this.isEnableControl = isEnableControl;
    }

    public void SetUp(float speed)
    {
        //this.characterController = characterController;
        this.speed = speed;
    }

    public void SetCameraPivot(Transform cameraPivot)
    {
        this.cameraPivot = cameraPivot;
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float cameraAngle = 0;
        if (cameraPivot)
            cameraAngle = cameraPivot.eulerAngles.y;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraAngle;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            //characterController.Move(moveDir.normalized * speed * Time.deltaTime);
            transform.position += moveDir.normalized * speed * Time.deltaTime;
        }
    }
}
