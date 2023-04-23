using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerContainer : MonoBehaviourPun
{
    public PhotonView pv;
    public int carryingItemType { get; private set; }
    public float weight { get; private set; }
    private float limitWeight = 15f;

    public ContainerGraphic gfx;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        ResetOnCarry();
    }

    public float DeliverItem()
    {
        var deliverWeight = weight;
        ResetOnCarry();
        return deliverWeight;
    }

    [PunRPC]
    public void PunUpdateContainer(int itemType, float itemWeight)
    {
        UpdateContainer(itemType, itemWeight);
    }

    public void UpdateContainer(int itemType, float itemWeight)
    {
        carryingItemType = itemType;
        weight = itemWeight;

        gfx.DisplayContainer(carryingItemType, weight);
    }

    public bool PickUpItem(Item item)
    {
        if (CheckType(item.type) && CheckWeight(item.weight))
        {
            if (pv)
                photonView.RPC(nameof(PunUpdateContainer), RpcTarget.All, item.type, weight + item.weight);
            else
                UpdateContainer(item.type, weight + item.weight);
            return true;
        }

        return false;
    }

    public bool CheckType(int type)
    {
        return (carryingItemType == -1 || carryingItemType == type);
    }

    public bool CheckWeight(float weight)
    {
        if (this.weight + weight > limitWeight)
            return false;

        else
            return true;
    }

    private void ResetOnCarry()
    {
        carryingItemType = -1;
        weight = 0;
        UpdateContainer(-1, 0);
    }
}
