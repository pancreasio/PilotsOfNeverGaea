using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void ButtonAction();
    public delegate void StartDuelFunction(CharacterSelectionManager.Character p1Character, CharacterSelectionManager.Character p2Character);
    public delegate void SceneChange(int value);
    private GameObject gameManagerInstance;
    private int currentScene;

    private void Awake()
    {
        if (gameManagerInstance == null)
            gameManagerInstance = this.gameObject;
        else
            Destroy(this.gameObject);
    }
    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
        MenuManager.StartAction = NextScene;
        MenuManager.ExitAction = ExitApplication;
        LevelManager.RetryAction = ReloadScene;
        LevelManager.ExitAction = LoadScene;
        //CharacterSelectionManager.SelectAction = NextScene;
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(currentScene);
    }

    private void NextScene()
    {
        SceneManager.LoadScene(++currentScene);
    }

    private void PreviousScene()
    {
        SceneManager.LoadScene(--currentScene);
    }

    private void ExitApplication()
    {
        Application.Quit();
    }

    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        currentScene = sceneIndex;
    }
}
