using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
public class ObjectInteract : MonoBehaviour
{
    private PhotonView photonView;
    private PlayerContainer itemContainer;
    // Start is called before the first frame update
    void Start()
    {
        itemContainer = GetComponent<PlayerContainer>();
        photonView = GetComponent<PhotonView>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            var item = other.GetComponent<Item>();
            var canPutItem = itemContainer.PickUpItem(item);
            if (canPutItem)
            {
                item.SetDestroy();
            }
        }

        else if (other.CompareTag("DeliverPoint"))
        {
            var container = other.GetComponent<InteractObj>();
            if (itemContainer.carryingItemType == container.itemType)
            {
                float deliverWeight = itemContainer.DeliverItem();
                if(photonView)
                    photonView.Owner.AddScore((int)deliverWeight);
            }
        }
    }
}
