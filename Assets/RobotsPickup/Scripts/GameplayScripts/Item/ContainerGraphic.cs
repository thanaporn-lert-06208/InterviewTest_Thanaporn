using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContainerGraphic : MonoBehaviour
{
    public TMP_Text weightText;
    public GameObject[] itemGfx;

    public void DisplayContainer(int itemType, float weight)
    {
        for (int i = 0; i < itemGfx.Length; i++)
        {
            itemGfx[i].SetActive(i == itemType);
        }

        weightText.text = weight.ToString();
    }
}
