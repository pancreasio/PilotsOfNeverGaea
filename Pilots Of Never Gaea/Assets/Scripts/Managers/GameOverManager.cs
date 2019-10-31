using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameManager.ButtonAction RematchAction;
    public static GameManager.SceneChange CharacterSelectAction, ExitAction;
    public static bool player1Won;
    public TextMeshProUGUI winText;
    

    public void Rematch()
    {
        if (RematchAction != null)
            RematchAction();
    }

    private void Start()
    {
        if (player1Won)
        {
            winText.text = "Player 1 Wins!";
        }
        else
        {
            winText.text = "Player 2 Wins!";
        }
    }

    public void CharacterSelect()
    {
        if (CharacterSelectAction != null)
        {
            LevelManager.p1Selected = CharacterSelectionManager.Character.nullCharacter;
            LevelManager.p2Selected = CharacterSelectionManager.Character.nullCharacter;
            CharacterSelectAction(1);
        }
    }

    public void Exit()
    {
        if (ExitAction != null)
            ExitAction(0);
    }
}
