using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelSaver : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;

    private Vector3 limits;
    private List<ElementData> elementDatas;

    ElementSpawnerEditor elementSpawnerEditor;
    LevelEditor levelEditor;

    public event Action OnLevelSaved;

    private void Awake()
    {
        levelEditor = FindObjectOfType<LevelEditor>();
        levelEditor.OnEditModeActive += LevelEditor_OnEditModeActive;

        elementSpawnerEditor = FindObjectOfType<ElementSpawnerEditor>();
        elementSpawnerEditor.OnElementDataChange += ElementSpawnerEditor_OnElementDataChange;
    }

    private void LevelEditor_OnEditModeActive(LevelData obj)
    {
        levelData = obj;
    }

    private void ElementSpawnerEditor_OnElementDataChange(List<ElementData> value, Vector3 limits)
    {
        elementDatas = value;
        this.limits = limits;
    }

    public void Save()
    {

        var level = GetLevelIfExistResources();

        if (level == null)
        {
            LevelData asset = ScriptableObject.CreateInstance<LevelData>();

            AssetDatabase.CreateAsset(asset, "Assets/Resources/Levels/" + levelEditor.newLevelNumber + ".asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;

            asset.cubeLimits = limits;
            asset.elementDatas = elementDatas;
            asset.level = levelEditor.newLevelNumber;
            asset.time = levelEditor.time;

            EditorUtility.SetDirty(asset);
        }
        else
        {
            level.cubeLimits = limits;
            level.elementDatas = elementDatas;
            level.level = levelEditor.newLevelNumber;
            level.time = levelEditor.time;
            
            EditorUtility.SetDirty(level);
        }

        OnLevelSaved?.Invoke();
    }


    private LevelData? GetLevelIfExistResources()
    {
        var datas = Resources.FindObjectsOfTypeAll<LevelData>();

        foreach (var item in datas)
        {
            if (item.level == levelEditor.newLevelNumber)
            {
                return item;
            }
        }

        return null;
    }
}
