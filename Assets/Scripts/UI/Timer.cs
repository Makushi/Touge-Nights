using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerTxt = null;

    private float timer = 0.0f;
    private bool isRunning = false;
    private void OnEnable()
    {
        EventManager.onGameStarted += ResetTimer;
        EventManager.onLapCompleted += StopTimer;
        EventManager.onRetry += ResetTimer;
    }

    private void OnDisable()
    {
        EventManager.onGameStarted -= ResetTimer;
        EventManager.onLapCompleted -= StopTimer;
        EventManager.onRetry += ResetTimer;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        timer += Time.deltaTime;
        timerTxt.text = FormatTimer();
    }

    private void StopTimer()
    {
        isRunning = false;
        GameController.Instance.SaveTime(timerTxt.text);
    }

    private void ResetTimer()
    {
        timer = 0.0f;
        isRunning = true;
    }

    private string FormatTimer()
    {
        string elapsedTime = "";

        int minutes = (int)Math.Round(timer / 60);
        int seconds = (int)Math.Round(timer % 60);
        int miliseconds = (int)Math.Round((timer * 100) % 100);

        elapsedTime = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + miliseconds.ToString("00");
        return elapsedTime;
    }
}
