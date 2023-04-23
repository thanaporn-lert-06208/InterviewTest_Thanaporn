using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterList", menuName = "ScriptableObjects/CharacterList")]
public class CharacterList : ScriptableObject
{
    public Character[] characters;
}
