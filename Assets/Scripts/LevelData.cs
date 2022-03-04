using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "My Scriptables/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField]
    public Vector3 cubeLimits;

    [SerializeField]
    public List<ElementData> elementDatas;

    [SerializeField]
    public int time;
    
    [SerializeField]
    public int level;

    private void OnValidate()
    {
        AssetDatabase.SaveAssets();
    }

}
