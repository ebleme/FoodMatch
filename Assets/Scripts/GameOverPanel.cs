using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject body;

    [SerializeField]
    private Text starCount;

    private MatchArea matchArea;
    private CountDownTimer countDownTimer;

    private void Awake()
    {
        matchArea = FindObjectOfType<MatchArea>();
        matchArea.OnGameOver += OnGameOver;

        countDownTimer = FindObjectOfType<CountDownTimer>();
        countDownTimer.OnTimeEnded += OnGameOver;
    }

    private void OnGameOver()
    {
        Show();
    }

    public void Show()
    {
        body.SetActive(true);
        starCount.text = FindObjectOfType<Stars>().GetStarCount().ToString();
    }

    public void Close()
    {
        body.SetActive(false);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
