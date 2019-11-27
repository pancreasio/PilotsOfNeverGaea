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
    public delegate void GameOverFunction(bool value);
    private static GameObject gameManagerInstance;
    private int currentScene;

    private void Awake()
    {
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this.gameObject;
            DontDestroyOnLoad(gameManagerInstance);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        currentScene = SceneManager.GetActiveScene().buildIndex;
        MenuManager.StartAction = NextScene;
        MenuManager.ExitAction = ExitApplication;
        LevelManager.GameOverAction = GameOver;
        LevelManager.CharacterSelectButton = LoadScene;
        LevelManager.ExitButton = LoadScene;
        CharacterSelectionManager.SelectAction = LoadGame;
        CharacterSelectionManager.FadeAction = UnloadAdditiveScene;
        CharacterSelectionManager.ExitButtonAction = PreviousScene;
        GameOverManager.RematchAction = PreviousScene;
        GameOverManager.CharacterSelectAction = LoadScene;
        GameOverManager.ExitAction = LoadScene;
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

    private void GameOver(bool player1won)
    {
        SceneManager.LoadScene(3);
        GameOverManager.player1Won = player1won;
        currentScene = 3;
    }

    private void ExitApplication()
    {
        Application.Quit();
    }

    private void UnloadAdditiveScene(int sceneIndex)
    {
        SceneManager.UnloadSceneAsync(sceneIndex);
    }

    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        currentScene = sceneIndex;
    }

    private void LoadGame(CharacterSelectionManager.Character p1Character, CharacterSelectionManager.Character p2Character)
    {
        LevelManager.p1Selected = p1Character;
        LevelManager.p2Selected = p2Character;
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        currentScene = 2;
    }
}
