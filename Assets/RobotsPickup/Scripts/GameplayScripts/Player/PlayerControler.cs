using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerControler : MonoBehaviour
{
    public static PlayerControler player;
    private bool isMine = true;

    private float speed = 10f;
    private PlayerMovement playerMovement;
    private CameraManager cameraManager;

    private bool isControlEnable = false;

    [SerializeField] private GameObject cameraManagerPrefab;
    void Start()
    {
        CheckIsMine();
        SetUpScript();
    }
 
    void Update()
    {
        if(isControlEnable)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                SwitchCamera();
            }
        }
    }

    private void SwitchCamera()
    {
        var camera = cameraManager.SwitchCamera();
        playerMovement.SetCameraPivot(camera);
    }

    public void SetControlEnable(bool isControlEnable)
    {
        this.isControlEnable = isControlEnable;
        playerMovement.SetEnableControl(isControlEnable);
    }

    public void CheckIsMine()
    {
        if(TryGetComponent<PhotonView>(out var pv))
        {
            isMine = pv.IsMine;
        }
    }

    public void SetUpScript()
    {
        if (isMine)
        {
            player = this;
            //characterController = GetComponent<CharacterController>();

            cameraManager = Instantiate(cameraManagerPrefab,transform).GetComponent<CameraManager>();

            playerMovement = GetComponent<PlayerMovement>();
            playerMovement.SetUp(speed);
            SwitchCamera();

            SetControlEnable(true);
        }
        else
        {
            Destroy(GetComponent<CharacterController>());
            Destroy(GetComponent<PlayerMovement>());
            Destroy(GetComponent<ObjectInteract>());
            enabled = false;
        }
    }

}
