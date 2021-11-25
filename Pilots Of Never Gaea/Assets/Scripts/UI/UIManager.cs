using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public EventSystem eventSystem;
    private GameObject lastSelected;
    public enum MenuScreen
    {
        main,
        characterSelect,
        game,
        gameOver
    }
    public MenuScreen currentScreen;

    private void Start()
    {
        lastSelected = eventSystem.currentSelectedGameObject;
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        // {
        //     if(currentScreen == MenuScreen.gameOver)
        //         AkSoundEngine.PostEvent((string)"sfx_ui_option_eg", gameObject);
        //     else
        //         AkSoundEngine.PostEvent((string)"sfx_ui_select", gameObject);
        // }

        // if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightControl))
        // {
        //     if (currentScreen == MenuScreen.gameOver)
        //         AkSoundEngine.PostEvent((string)"sfx_ui_ok_eg", gameObject);
        //     else
        //         AkSoundEngine.PostEvent((string)"sfx_ui_ok", gameObject);
        // }

        if (eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(lastSelected);
        else
            lastSelected = eventSystem.currentSelectedGameObject;
    }

    public void ResetSelected()
    {
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
}
