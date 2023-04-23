using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChoiceCreator;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private CharacterList characterList;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonContent;
    [SerializeField] private TMPro.TMP_Text characterName;
    private ButtonGroupAction buttonGroup = new ButtonGroupAction();
    private Character choosedCharacter;

    void Start()
    {
        if (Profile.character)
            Debug.Log($"Profile.character : {Profile.character.name}");
        foreach (var character in characterList.characters)
        {
            var newChoice = Choice.CreateButton(buttonPrefab, buttonContent, character.name, character.image, buttonGroup, () => { ChooseCharacter(character); });
            if (choosedCharacter == null || character == Profile.character)
            {
                if (choosedCharacter == null)
                    Debug.Log("choosedCharacter == null");
                else if (character == Profile.character)
                    Debug.Log("character == Profile.character");

                newChoice.onClick?.Invoke();
            }
        }
    }

    private void ChooseCharacter(Character character)
    {
        Debug.Log("Choose " + character.name);
        choosedCharacter = character;
        characterName.text = character.name;
    }

    public Character GetCharacter()
    {
        return choosedCharacter;
    }

    public Character SaveCharacter()
    {
        Profile.character = choosedCharacter;
        return choosedCharacter;
    }
}
