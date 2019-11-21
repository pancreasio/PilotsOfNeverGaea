using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MenuManager : MonoBehaviour
{
    public static GameManager.ButtonAction StartAction;
    public static GameManager.ButtonAction ExitAction;
    public TextMeshProUGUI versionText;

    private void Start()
    {
        Time.timeScale = 1;
        versionText.text = "v" + Application.version;
    }

    public void StartGame()
    {
        if (StartAction != null)
        {
            LevelManager.p1Selected = CharacterSelectionManager.Character.none;
            LevelManager.p2Selected = CharacterSelectionManager.Character.none;
            StartAction();        
        }
    }

    public void ExitGame()
    {
        if (ExitAction != null)
            ExitAction();
    }
}
