using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField]
    private IntegerSO currentLevel;

    [SerializeField]
    private LevelData[] levels;

    public int elementCount;

    public event Action<LevelData> OnLevelChanged;


    private LevelData GetLevelFromResources()
    {
        return Resources.Load<LevelData>("Levels/" + currentLevel.Value);
    }

    public int GetCurentLevel()
    {
        return currentLevel.Value;
    }

    public void Loadlevel()
    {
        var data = GetLevelFromResources();

        elementCount = data.elementDatas.Count(p => p.isActive);

        OnLevelChanged?.Invoke(data);

        FindObjectOfType<ElementSpawner>().DrawElements(data);

    }

    public void Next()
    {
        currentLevel.Value += 1;

        EditorUtility.SetDirty(currentLevel);
    }

    public int GetElementCount()
    {
        return elementCount;
    }
}
