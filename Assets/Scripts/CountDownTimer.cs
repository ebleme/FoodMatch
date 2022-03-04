using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    [SerializeField]
    private Text timeText;

    private LevelDisplay levelDisplay;
    private PausePanel pausePanel;
    private MatchArea matchArea;


    public event Action OnTimeEnded;

    private float timeRemaining;
    private bool levelLoaded = false;
    private bool isPaused = false;

    private void Awake()
    {
        levelDisplay = FindObjectOfType<LevelDisplay>();
        levelDisplay.OnLevelChanged += LevelDisplay_OnLevelChanged;

        pausePanel = FindObjectOfType<PausePanel>();
        pausePanel.OnPaused += OnPaused;
        pausePanel.OnResume += PausePanel_OnResume;

        matchArea = FindObjectOfType<MatchArea>();
        matchArea.OnGameOver += OnPaused;
    }

    private void PausePanel_OnResume()
    {
        isPaused = false;
    }

    private void OnPaused()
    {
        isPaused = true;
    }

    private void LevelDisplay_OnLevelChanged(LevelData levelData)
    {
        timeRemaining = levelData.time;
        levelLoaded = true;
    }

    void Update()
    {
        if (!levelLoaded || isPaused)
            return;

        if (timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;

            DisplayTime();
        }
        else
        {
            // TODO: GameOver
            OnTimeEnded?.Invoke();
        }
    }

    private void DisplayTime()
    {
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        //float milliSeconds = (timeRemaining % 1) * 1000;

        //timeText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
