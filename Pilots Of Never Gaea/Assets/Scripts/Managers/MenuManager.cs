using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static GameManager.ButtonAction StartAction;
    public static GameManager.ButtonAction ExitAction;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        if (StartAction != null)
        {
            LevelManager.p1Selected = CharacterSelectionManager.Character.nullCharacter;
            LevelManager.p2Selected = CharacterSelectionManager.Character.nullCharacter;
            StartAction();        
        }
    }

    public void ExitGame()
    {
        if (ExitAction != null)
            ExitAction();
    }
}
