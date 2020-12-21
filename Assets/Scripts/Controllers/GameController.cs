using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public bool isGameRunning = false;

    public static GameController Instance = null;
    private string lapTime = "";

    private void OnEnable()
    {
        EventManager.onGameStarted += StartGame;
        EventManager.onGameOver += GameOver;
        EventManager.onRetry += Retry;
        EventManager.onLapCompleted += GameWon;
    }

    private void OnDisable()
    {
        EventManager.onGameStarted -= StartGame;
        EventManager.onGameOver -= GameOver;
        EventManager.onRetry -= Retry;
        EventManager.onLapCompleted -= GameWon;
    }

    private void Awake()
    {
        Initialize();
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
    }
    private void StartGame()
    {
        SceneManager.UnloadScene(3);
        SetGameState(true);
        AudioManager.Instance.PlayRandomSong();
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
