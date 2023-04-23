using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyRoomButton : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;
    
    public void SetButtonRoomInfo()
    {
        roomName.text = "new room";
    }
}
