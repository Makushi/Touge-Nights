using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool isGameRunning = false;
    private bool gameStarted = false;

    public static GameController Instance = null;
    private string lapTime = "";

    private void OnEnable()
    {
        EventManager.onGameStarted += StartGame;
        EventManager.onGameOver += GameOver;
        EventManager.onRetry += Retry;
        EventManager.onLapCompleted += GameWon;
        EventManager.onContinue += Continue;
        EventManager.onQuitToMainMenu += QuitToMainMenu;
    }

    private void OnDisable()
    {
        EventManager.onGameStarted -= StartGame;
        EventManager.onGameOver -= GameOver;
        EventManager.onRetry -= Retry;
        EventManager.onLapCompleted -= GameWon;
        EventManager.onContinue -= Continue;
        EventManager.onQuitToMainMenu += QuitToMainMenu;
    }

    private void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (!gameStarted)
            return; 

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameRunning)
            {
                Pause();
            }
            else
            {
                Continue();
            }
        }
    }

    private void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetGameState(false);
        //AudioManager.Instance.PlayRandomSong();
    }
    private void StartGame()
    {
        SceneManager.UnloadScene(3);
        SetGameState(true);
        AudioManager.Instance.PlayRandomSong();
        gameStarted = true;
    }
    
    private void GameWon()
    {
        SetGameState(false);
        EventManager.SwitchUI(UIType.EndScreen);
    }
    private void GameOver()
    {
        SetGameState(false);
        EventManager.SwitchUI(UIType.GameOver);
    }

    private void Retry()
    {
        SetGameState(true);
        EventManager.SwitchUI(UIType.GameUI);
        AudioManager.Instance.PlayRandomSong();
    }

    private void Pause()
    {
        SetGameState(false);
        EventManager.SwitchUI(UIType.PauseMenu);
        EventManager.Pause();
        AudioManager.Instance.SetPauseMenuVolume();
    }

    private void Continue()
    {
        SetGameState(true);
        EventManager.SwitchUI(UIType.GameUI);
        AudioManager.Instance.SetGameplayVolume();
    }

    private void QuitToMainMenu()
    {
        gameStarted = false;
        SetGameState(false);
        AudioManager.Instance.StopMusic();
        EventManager.SwitchUI(UIType.MainMenu);
        SceneManager.LoadScene(3, LoadSceneMode.Additive);
    }

    private void SetGameState(bool state)
    {
        isGameRunning = state;

        Time.timeScale = isGameRunning ? 1 : 0;
    }

    public void SaveTime(string lapTime)
    {
        this.lapTime = lapTime;
        EventManager.ShowResults();
    }

    public string GetTime()
    {
        return lapTime;
    }
}
