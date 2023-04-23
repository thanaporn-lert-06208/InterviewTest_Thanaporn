using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Item : MonoBehaviourPun
{
    public int type;
    public int weight;
    public TMP_Text weightLabel;

    private void Start()
    {
        weightLabel.text = weight.ToString();
    }

    public void SetDestroy()
    {
        if (TryGetComponent<PhotonView>(out var pv))
        {
            if (pv.IsMine)
                RPCSetDestroy();
            else
                pv.RPC(nameof(RPCSetDestroy), RpcTarget.MasterClient);
        }
        else
            Destroy(gameObject);
    }

    [PunRPC]
    public void RPCSetDestroy()
    {
        PhotonNetwork.Destroy(photonView);
    }
}
