using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ChoiceCreator
{
    public class Choice : MonoBehaviour
    {
        public string id;
        public Image image;
        public TMP_Text title;
        public Button button;
        public GameObject highlight;
        public ButtonGroupAction buttonGroupAction;

        public Image iconRight;

        private void Awake()
        {
            if (highlight)
                highlight.SetActive(false);
        }

        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Click);
        }

        public void Click()
        {
            if (buttonGroupAction != null && buttonGroupAction.activedButton)
            {
                buttonGroupAction.activedButton.OnDisselect();
            }

            OnSelect();
        }

        public void OnSelect()
        {
            if (highlight)
                highlight.SetActive(true);

            if (buttonGroupAction != null)
                buttonGroupAction.activedButton = this;
        }

        public void OnDisselect()
        {
            if (highlight)
                highlight.SetActive(false);
        }

        public static Button CreateButton(GameObject buttonPrefab, Transform parent, string name, Sprite image, ButtonGroupAction buttonGroupAction, System.Action action)
        {
            var ui = Instantiate(buttonPrefab);
            var button = ui.GetComponent<Button>();
            if (button)
                button.onClick.AddListener(() => { action(); });
            if (parent)
                ui.transform.SetParent(parent, false);
            ui.transform.localScale = Vector3.one;
            var choice = ui.GetComponent<Choice>();
            if (choice)
            {
                choice.buttonGroupAction = buttonGroupAction;
                if(choice.title)
                    choice.title.text = name;
                if (image)
                    choice.image.sprite = image;
            }

            return button;
        }
    }

    [System.Serializable]
    public class ButtonGroupAction
    {
        public string name;
        public Choice activedButton;
        public Choice defaultButton;
        public static List<ButtonGroupAction> keepDefaultList = new List<ButtonGroupAction>();

        public void AddToList()
        {
            keepDefaultList.Add(this);
        }

        public static void SaveDefault()
        {
            foreach (var comp in keepDefaultList)
            {
                comp.defaultButton = comp.activedButton;
            }
        }

        public static void Reset()
        {
            foreach (var comp in keepDefaultList)
            {
                if (comp.activedButton.button == null)
                    Debug.Log(comp.name);
                if (comp.defaultButton && comp.defaultButton.button)
                    comp.defaultButton.button.onClick?.Invoke();
            }
        }
    }
}