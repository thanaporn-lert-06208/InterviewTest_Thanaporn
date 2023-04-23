using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class EditName : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private Button saveBtn;
    [SerializeField] private Button cancleBtn;

    private void OnEnable()
    {
        nameField.text = Profile.name;
        if (saveBtn)
            saveBtn.onClick.AddListener(SaveName);
        if(cancleBtn)
            cancleBtn.onClick.AddListener(CancleSaveName);
    }

    private void SaveName()
    {
        Profile.name = nameField.text;
    }

    private void CancleSaveName()
    {
        nameField.text = Profile.name;
    }
}
