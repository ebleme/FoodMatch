using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    [SerializeField]
    private Text level;

    LevelDisplay levelDisplay;

    private void Awake()
    {
        levelDisplay = FindObjectOfType<LevelDisplay>();
        levelDisplay.OnLevelChanged += LevelDisplay_OnLevelChanged;
    }

    private void LevelDisplay_OnLevelChanged(LevelData levelData)
    {
        level.text = levelData.level.ToString();
    }
}
