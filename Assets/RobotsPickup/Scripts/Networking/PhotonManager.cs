using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks, IConnectionCallbacks
{
    public static PhotonManager Instance { get => instance; }
    private static PhotonManager instance;

    public TMPro.TMP_Text clientStateTxt;

    #region UNITY
    
    public void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);

        PhotonNetwork.AutomaticallySyncScene = true;
        Application.runInBackground = true;
    }

    private void Update()
    {
        string isConnect = PhotonNetwork.IsConnected ? "Connect" : "NotConnect";
        clientStateTxt.text = $"Photon Client State: ({isConnect}) " + PhotonNetwork.NetworkClientState.ToString();
    }
    public void StartConnect()
    {
        StartCoroutine(Connect());
    }

    private IEnumerator Connect()
    {
        var alertPanel = AlertPanel.CreateAlertPanel(new AlertPanel.AlertContent("", "On Connecting..."), new AlertPanel.ChoiceContent(0));
        yield return new WaitUntil(() => PhotonNetwork.IsConnected == false);

        PhotonNetwork.LocalPlayer.NickName = Profile.name;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("asia");
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (!SceneControl.GetCurrentSceneName().Contains("Lobby"))
            SceneControl.LoadScene("Lobby");
    }


    #endregion

    #region Logout

    void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
    {
        switch(cause)
        {
            case DisconnectCause.None:
            case DisconnectCause.DisconnectByClientLogic:
            case DisconnectCause.ApplicationQuit:
                break;
            default:
                Debug.Log("OnDisconnected " + cause.ToString());
                Invoke(nameof(Logout), 1.5f);
                break;
        }
    }
    public void Logout()
    {
        Debug.Log("PhotonManager Logout");
        if (PhotonNetwork.IsConnected == true)
        {
            Debug.Log("Disconnect");
            PhotonNetwork.Disconnect();
        }

        SceneControl.LoadScene("Main");
    }

    #endregion

}
