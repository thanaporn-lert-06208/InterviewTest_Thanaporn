using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private Button quitBtn;
    [SerializeField] private PanelComp homePanel;
    [SerializeField] private NamePanel namePanel;
    [SerializeField] private PanelComp characterPanel;

    [SerializeField] private CharacterSelection characterSelection;

    void Start()
    {
        if(Profile.name == "-")
            Profile.name = "Player " + Random.Range(1000, 10000);

        InitPanel();
    }

    private void InitPanel()
    {
        homePanel.nextBtn.onClick.AddListener(OpenNameEdit);
        quitBtn.onClick.AddListener(QuitApp);

        InitNamePanel();
        InitCharacterPanel();
    }

    private void QuitApp()
    {
#if UNITY_EDITOR
        Debug.Log("QuitApp");
#else
        Application.Quit();
#endif
    }

    public void OpenNameEdit()
    {
        homePanel.panel.SetActive(false);
        namePanel.panel.SetActive(true);
    }

    private void OpenCharacterSelection()
    {
        namePanel.panel.SetActive(false);
        characterPanel.panel.SetActive(true);
    }

    private void Connect()
    {
        characterPanel.panel.SetActive(false);
        PhotonManager.Instance.Connect();
    }

    private void InitNamePanel()
    {
        namePanel.panel.SetActive(false);
        namePanel.nextBtn.onClick.AddListener(SaveName);
    }

    private void InitCharacterPanel()
    {
        characterPanel.panel.SetActive(false);
        characterPanel.nextBtn.onClick.AddListener(SaveCharacter);
    }

    private void SaveName()
    {
        if (namePanel.nameField.text == "" || namePanel.nameField.text == "-")
        {
            AlertPanel.AlertContent alertContent = new AlertPanel.AlertContent("", "Invalid Name");
            AlertPanel.ChoiceContent choiceContent = new AlertPanel.ChoiceContent("Yes");
            AlertPanel.CreateAlertPanel(alertContent, choiceContent);
        }
        else if(namePanel.nameField.text.Length < 3)
        {
            AlertPanel.AlertContent alertContent = new AlertPanel.AlertContent("", "Enter not less than 3 characters");
            AlertPanel.ChoiceContent choiceContent = new AlertPanel.ChoiceContent("Yes");
            AlertPanel.CreateAlertPanel(alertContent, choiceContent);
        }
        else
        {
            Profile.name = namePanel.nameField.text;
            OpenCharacterSelection();
        }
    }

    private void SaveCharacter()
    {
        characterSelection.SaveCharacter();
        if (Profile.character == null)
        {
            AlertPanel.AlertContent alertContent = new AlertPanel.AlertContent("", "Please Select a Character");
            AlertPanel.ChoiceContent choiceContent = new AlertPanel.ChoiceContent("Yes","");
            AlertPanel.CreateAlertPanel(alertContent, choiceContent);
        }
        else
            Connect();
    }


    [System.Serializable]
    public class PanelComp
    {
        public GameObject panel;
        public Button nextBtn;
    }
    [System.Serializable]
    public class NamePanel : PanelComp
    {
        public TMPro.TMP_InputField nameField;
    }


}
