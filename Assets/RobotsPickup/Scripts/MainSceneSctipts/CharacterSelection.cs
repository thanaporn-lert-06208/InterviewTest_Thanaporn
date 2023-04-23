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
        foreach (var character in characterList.characters)
        {
            var newChoice = Choice.CreateButton(buttonPrefab, buttonContent, character.name, character.image, buttonGroup, () => { ChooseCharacter(character); });
            if (choosedCharacter == null || character == Profile.character)
            {
                newChoice.onClick?.Invoke();
            }
        }
    }

    private void ChooseCharacter(Character character)
    {
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
