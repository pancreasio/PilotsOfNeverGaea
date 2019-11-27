using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public static GameManager.ButtonAction RematchAction;
    public static GameManager.SceneChange CharacterSelectAction, ExitAction;
    public static bool player1Won;
    public TextMeshProUGUI winText;
    public EventSystem eventSystem;
    private GameObject lastSelected;

    public void Rematch()
    {
        if (RematchAction != null)
            RematchAction();
    }

    private void Start()
    {
        lastSelected = eventSystem.currentSelectedGameObject;
        if (player1Won)
        {
            winText.text = "Player 1 Wins!";
        }
        else
        {
            winText.text = "Player 2 Wins!";
        }
        AkSoundEngine.PostEvent("music_endgame", gameObject);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            AkSoundEngine.PostEvent((string)"sfx_ui_option_eg", gameObject);

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl))
            AkSoundEngine.PostEvent((string)"sfx_ui_ok_eg", gameObject);

        if (eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(lastSelected);
        else
            lastSelected = eventSystem.currentSelectedGameObject;
    }

public void CharacterSelect()
    {
        if (CharacterSelectAction != null)
        {
            LevelManager.p1Selected = CharacterSelectionManager.Character.none;
            LevelManager.p2Selected = CharacterSelectionManager.Character.none;
            CharacterSelectAction(1);
        }
    }

    public void Exit()
    {
        if (ExitAction != null)
            ExitAction(0);
    }
}
