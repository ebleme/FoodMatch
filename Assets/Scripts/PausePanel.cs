using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject body;

    public event Action OnPaused;
    public event Action OnResume;

    public void Show()
    {
        body.SetActive(true);
        OnPaused?.Invoke();
    }

    public void Close()
    {
        body.SetActive(false);
        OnResume?.Invoke();
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
