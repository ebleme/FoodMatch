using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SuccessPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject body;

    [SerializeField]
    private Text starCount;


    private MatchArea matchArea;
    private LevelDisplay levelDisplay;

    int pointedElementCount; // patlamýþ ve puan haline dönüþmüþ elementsayýsý

    private void Awake()
    {
        matchArea = FindObjectOfType<MatchArea>();
        levelDisplay = FindObjectOfType<LevelDisplay>();
    }

    private void Start()
    {
        matchArea.OnPointGathered += MatchArea_OnPointGathered;
    }

    private void MatchArea_OnPointGathered(Vector3 obj)
    {
        pointedElementCount += 3;

        if (pointedElementCount >= levelDisplay.GetElementCount())
        {
            Show();
        }
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

    // paneldeki Next butonuna basýnca sonraki seviyeye geçilir
    public void Next()
    {
        FindObjectOfType<LevelDisplay>().Next();

        Retry();
    }
}
