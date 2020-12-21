using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void OnLapCompleted();
    public static event OnLapCompleted onLapCompleted;

    public delegate void OnSpeedUpdate(float currentSpeed);
    public static event OnSpeedUpdate onSpeedUpdate;

    public delegate void OnEnergyUpdate(float currentEnergy);
    public static event OnEnergyUpdate onEnergyUpdate;

    public delegate void OnGameOver();
    public static event OnGameOver onGameOver;

    public delegate void OnCameraSwitch();
    public static event OnCameraSwitch onCameraSwitch;

    public delegate void OnGameStarted();
    public static event OnGameStarted onGameStarted;

    public delegate void OnRetry();
    public static event OnRetry onRetry;

    public delegate void OnSwitchUI(UIType ui);
    public static event OnSwitchUI onSwitchUI;

    public delegate void OnShowResults();
    public static event OnShowResults onShowResults;

    public delegate void OnPlayerSpawned();
    public static event OnPlayerSpawned onPlayerSpawned;

    public static void LapCompleted()
    {
        if (onLapCompleted != null)
        {
            onLapCompleted();
        }
    }
    public static void StartGame()
    {
        if (onGameStarted != null)
        {
            onGameStarted();
        }
    }
    public static void GameOver()
    {
        if (onGameOver != null)
        {
            onGameOver();
        }
    }

    public static void Retry()
    {
        if (onRetry != null)
        {
            onRetry();
        }
    }

    public static void CameraSwitch()
    {
        if (onCameraSwitch != null)
        {
            onCameraSwitch();
        }
    }

    public static void UpdateSpeedometer(float currentSpeed)
    {
        if (onSpeedUpdate != null)
        {
            onSpeedUpdate(currentSpeed);
        }
    }

    public static void UpdateEnergyMeter(float currentEnergy)
    {
        if (onEnergyUpdate != null)
        {
            onEnergyUpdate(currentEnergy);
        }
    }

    public static void ShowResults()
    {
        if (onShowResults != null)
        {
            onShowResults();
        }
    }

    public static void SwitchUI(UIType ui)
    {
        if (onSwitchUI != null)
        {
            onSwitchUI(ui);
        }
    }

    public static void PlayerSpawned()
    {
        if (onPlayerSpawned != null)
        {
            onPlayerSpawned();
        }
    }
}