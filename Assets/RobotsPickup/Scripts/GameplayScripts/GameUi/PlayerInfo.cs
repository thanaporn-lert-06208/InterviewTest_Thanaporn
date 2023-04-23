using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public UnityEngine.UI.Image bg;
    public TMP_Text nameTxt;
    public TMP_Text scoresTxt;

    public void SetInfo(string name, string scores, Color color)
    {
        bg.color = color;
        nameTxt.text = name;
        scoresTxt.text = scores;
    }
    public void SetInfo(string name, string scores)
    {
        nameTxt.text = name;
        scoresTxt.text = scores;
    }
}
