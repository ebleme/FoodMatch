using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject cubePrefab;

    [SerializeField]
    private Vector3 targetPosition;

    private Vector3 offset;
    LevelData levelData;

    private void Start()
    {
        offset = new Vector3(1.5f, 1.5f, 1.5f);
        targetPosition = new Vector3(0, 2, 0);
    }

    public void DrawElements(LevelData currentLevelData)
    {
        levelData = currentLevelData;

        // Var olanlarý yok et
        foreach (Transform child in gameObject.transform)
            Destroy(child.gameObject);

        // Rotasyonel Parent scale ayarla
        gameObject.transform.localScale = levelData.cubeLimits + offset;

        //Vector3 startPos = new Vector3(transform.localScale.x / 2 * -1, 0, 0);

        Spawn();

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        transform.DOMove(targetPosition, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            Debug.Log("Düþtü");
        });
    }

    private void Spawn()
    {
        // Spawn
        int i = -1;

        for (int x = 0; x < levelData.cubeLimits.x; x++)
        {
            for (int y = 0; y < levelData.cubeLimits.y; y++)
            {
                for (int z = 0; z < levelData.cubeLimits.z; z++)
                {
                    i++;

                    if (!levelData.elementDatas[i].isActive)
                        continue;

                    GameObject go = Instantiate(cubePrefab, gameObject.transform, true);
                    go.transform.position = new Vector3(x - levelData.cubeLimits.x / 2, y - levelData.cubeLimits.y / 2 + transform.position.y, z - levelData.cubeLimits.z / 2);
                    go.GetComponent<Element>().SetElementData(levelData.elementDatas[i]);
                }
            }
        }
    }
}
