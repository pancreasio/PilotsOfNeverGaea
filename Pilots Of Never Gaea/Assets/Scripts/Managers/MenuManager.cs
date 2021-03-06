﻿using System.Collections;
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
    public GameObject mainCanvas, creditsCanvas, controlsCanvas,
        firstMainButton, firstCreditsButton, firstControlsButton;

    private void Start()
    {
        Time.timeScale = 1;
        versionText.text = "v" + Application.version;
        AkSoundEngine.PostEvent((string)"music_menu", gameObject);
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

    public void Credits()
    {
        creditsCanvas.SetActive(true);
        mainCanvas.SetActive(false);
        eventSystem.SetSelectedGameObject(firstCreditsButton);
    }

    public void MainMenu()
    {
        mainCanvas.SetActive(true);
        creditsCanvas.SetActive(false);
        controlsCanvas.SetActive(false);
        eventSystem.SetSelectedGameObject(firstMainButton);
    }

    public void Controls()
    {
        controlsCanvas.SetActive(true);
        mainCanvas.SetActive(false);
        eventSystem.SetSelectedGameObject(firstControlsButton);
    }

    public void ExitGame()
    {
        if (ExitAction != null)
            ExitAction();
    }
}
