using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void ButtonAction();
    public delegate void StartDuelFunction(CharacterSelectionManager.Character p1Character, CharacterSelectionManager.Character p2Character);
    public delegate void BindInputFunction(PlayerInput p1Input, PlayerInput p2Input);
    public delegate void SceneChange(int value);
    public delegate void GameOverFunction(bool value);
    private static GameObject gameManagerInstance;
    private int currentScene;

    [SerializeField]
    protected GameObject playerPrefab;

    public User player1;
    public User player2;

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
        player1 = transform.Find("Player 1").GetComponent<User>();
        player2 = transform.Find("Player 2").GetComponent<User>();

        MenuManager.StartAction = NextScene;
        MenuManager.ExitAction = ExitApplication;
        LevelManager.GameOverAction = GameOver;
        LevelManager.CharacterSelectButton = LoadScene;
        LevelManager.ExitButton = LoadScene;
        CharacterSelectionManager.SelectAction = LoadGame;
        CharacterSelectionManager.FadeAction = UnloadAdditiveScene;
        CharacterSelectionManager.ExitButtonAction = PreviousScene;
        CharacterSelectionManager.BindControlsAction = BindPlayerInput;
        GameOverManager.RematchAction = PreviousScene;
        GameOverManager.CharacterSelectAction = LoadScene;
        GameOverManager.ExitAction = LoadScene;
        ControlSelectionManager.controlSelectionManagerInstance.GetComponent<ControlSelectionManager>().controlsSetAction += ChangeControlSettings;
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

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public User GetPlayer1()
    {
        return player1;
    }

    public User GetPlayer2()
    {
        return player2;
    }

    private void ChangeControlSettings(ControllerDeviceUI newP1Controller, ControllerDeviceUI newP2Controller)
    {
        player1.currentControls.PairControllerData(newP1Controller.assignedDevice, newP1Controller.assignedScheme);
        player2.currentControls.PairControllerData(newP2Controller.assignedDevice, newP2Controller.assignedScheme);
    }

    private void BindPlayerInput(PlayerInput p1Input, PlayerInput p2Input)
    {
        p1Input.SwitchCurrentControlScheme(player1.currentControls.controlScheme, player1.currentControls.inputDevice);
        p2Input.SwitchCurrentControlScheme(player2.currentControls.controlScheme, player2.currentControls.inputDevice);
    }
}
