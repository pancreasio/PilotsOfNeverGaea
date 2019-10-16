using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    public enum Character
    {
        nullCharacter, raildrive, magstream, kunst
    }
    public static GameManager.StartDuelFunction SelectAction;
    private Character p1SelectedCharacter, p2SelectedCharacter;

    private void Start()
    {
        p1SelectedCharacter = Character.nullCharacter;
        p2SelectedCharacter = Character.nullCharacter;
    }

    private void Update()
    {
        if (p1SelectedCharacter != Character.nullCharacter && p2SelectedCharacter != Character.nullCharacter)
        {
            if(SelectAction != null)
            SelectAction(p1SelectedCharacter, p2SelectedCharacter);
        }
    }

    public void P1SelectCharacter(int character)
    {
        Character selectedCharacter = (Character)character;
        p1SelectedCharacter = selectedCharacter;
    }

    public void P2SelectCharacter(int character)
    {
        Character selectedCharacter = (Character)character;
        p2SelectedCharacter = selectedCharacter;
    }
}
