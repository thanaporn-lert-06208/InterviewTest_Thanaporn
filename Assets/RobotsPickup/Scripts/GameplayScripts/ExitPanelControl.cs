using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ExitPanelControl : MonoBehaviour
{
    public GameObject exitPanel;
    public Button resume;
    public Button leaveRoom;
    public Button logout;
    public Button quit;

    private void Start()
    {
        exitPanel.SetActive(false);
        InitButton();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleActivePanel();
        }
    }

    private void ToggleActivePanel()
    {
        exitPanel.SetActive(!exitPanel.activeSelf);
    }

    public void InitButton()
    {
        resume.onClick.AddListener(ToggleActivePanel);
        leaveRoom.onClick.AddListener(() => Photon.Pun.PhotonNetwork.LeaveRoom());
        logout.onClick.AddListener(PhotonManager.Instance.Logout);
        quit.onClick.AddListener(() => Application.Quit());
    }
}
