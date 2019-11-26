using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class MenuManager : MonoBehaviour
{
    public static GameManager.ButtonAction StartAction;
    public static GameManager.ButtonAction ExitAction;
    public TextMeshProUGUI versionText;
    public EventSystem eventSystem;
    private GameObject lastSelected;

    private void Start()
    {
        Time.timeScale = 1;
        versionText.text = "v" + Application.version;
        AkSoundEngine.PostEvent((string)"music_menu", gameObject);
        lastSelected = eventSystem.currentSelectedGameObject;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            AkSoundEngine.PostEvent((string)"sfx_ui_select", gameObject);

        if (eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(lastSelected);
        else
            lastSelected = eventSystem.currentSelectedGameObject;
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
