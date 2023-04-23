using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlertPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text content;
    [SerializeField] private Button leftButton;
    [SerializeField] private TMP_Text leftButtonTxt;
    [SerializeField] private Button rightButton;
    [SerializeField] private TMP_Text rightButtonTxt;
    private ChoiceContent choiceContent;

    public static AlertPanel CreateAlertPanel(AlertContent alertContent, ChoiceContent choiceContent)
    {
        var alertPanelPrefab = Resources.Load<GameObject>("AlertCanvasPrefab");
        if(alertPanelPrefab)
        {
            var newAlert = Instantiate(alertPanelPrefab).GetComponent<AlertPanel>();
            newAlert.SetUpPanel(alertContent, choiceContent);

            return newAlert;
        }
        Debug.Log("Return Null");
        return null;
    }

    public void SetUpPanel(AlertContent alertContent, ChoiceContent choiceContent)
    {
        header.text = alertContent.header;
        content.text = alertContent.content;
        this.choiceContent = choiceContent;
        if (!string.IsNullOrEmpty(choiceContent.leftButtonLabel))
        {
            leftButton.onClick.AddListener(LeftClick);
            leftButtonTxt.text = choiceContent.leftButtonLabel;
            leftButton.gameObject.SetActive(true);
        }
        else
            leftButton.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(choiceContent.rightButtonLabel))
        {
            rightButton.onClick.AddListener(RightClick);
            rightButtonTxt.text = choiceContent.rightButtonLabel;
            rightButton.gameObject.SetActive(true);
        }
        else
        {
            rightButton.gameObject.SetActive(false);
        }

        if (choiceContent.delayDestroy > 0)
            Destroy(gameObject, choiceContent.delayDestroy);
    }

    private void LeftClick()
    {
        choiceContent.leftButtonAction?.Invoke();
        Destroy(gameObject);
    }

    private void RightClick()
    {
        choiceContent.rightButtonAction?.Invoke();
        Destroy(gameObject);
    }

    public class AlertContent
    {
        public string header;
        public string content;

        public AlertContent(string header, string content)
        {
            this.header = header;
            this.content = content;
        }
    }


    public class ChoiceContent
    {
        public string leftButtonLabel = string.Empty;
        public string rightButtonLabel = string.Empty;

        public System.Action leftButtonAction;
        public System.Action rightButtonAction;

        public float delayDestroy = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leftButtonLabel"></param>
        /// <param name="rightButtonLabel"></param>
        /// <param name="delayDestroy">The Panel will be automatically destroyed after spawning with a delay time. Enter 0 if not want auto destroy</param>
        public ChoiceContent(string leftButtonLabel, string rightButtonLabel = "")
        {
            this.leftButtonLabel = leftButtonLabel;
            this.rightButtonLabel = rightButtonLabel;

            if (leftButtonLabel == string.Empty && rightButtonLabel == string.Empty && delayDestroy == 0)
                this.leftButtonLabel = "Yes";
        }
        public ChoiceContent(float delayDestroy = 0)
        {
            this.delayDestroy = delayDestroy;
        }
    }


}
