using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject body;

    [SerializeField]
    private Text levelValue;


    LevelDisplay levelDisplay;

    private void Awake()
    {
        levelDisplay = FindObjectOfType<LevelDisplay>();
    }

    private void Start()
    {
        Show();
    }

    public void Show()
    {
        body.SetActive(true);
        levelValue.text = levelDisplay.GetCurentLevel().ToString();
    }

    public void Close()
    {
        body.SetActive(false);
    }

    public void StartGame()
    {
        levelDisplay.Loadlevel();
        
        Close();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Next()
    {
        FindObjectOfType<LevelDisplay>().Next();
    }
}
