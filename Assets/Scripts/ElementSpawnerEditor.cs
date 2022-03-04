using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElementSpawnerEditor : MonoBehaviour
{
    [SerializeField]
    private GameObject cubePrefab;

    [SerializeField]
    private Material disabledMaterial;

    [SerializeField]
    [Tooltip("Elemenlerin türü vb bilgileri tutan nesneler")]
    private List<ElementData> elements;

    private Vector3 offset = new Vector3(.5f, .5f, .5f);

    public List<Element> spawnedElements;
    public List<ElementData> spawnedElementDatas; // Elementlerin types, textures

    // ElementType, Count
    public Dictionary<ElementTypes, int> dicElementTypes = new Dictionary<ElementTypes, int>();

    public int count;

    Vector3 limits;

    private List<ElementData> determinedElementData; // Son aþamada seçilmiþ ve listelenmiþ datalar.

    public event Action<int> OnCountChange;
    public event Action<List<ElementData>, Vector3> OnElementDataChange;

    private void Awake()
    {
        FindObjectOfType<ElementViewChanger>().OnElementViewChanged += ElementSpawnerEditor_OnElementViewChanged;
    }

    private void ElementSpawnerEditor_OnElementViewChanged(Element obj)
    {
        spawnedElements.Where(p => p.index == obj.index).SingleOrDefault().isActive = obj.isActive;

        count = spawnedElements.Count(p => p.isActive);
        OnCountChange?.Invoke(count);
    }

    // Eðer Edit modunda ise
    public void SpawnElements(LevelData levelData)
    {
        limits = levelData.cubeLimits;

        spawnedElements = new List<Element>();

        count = (int)limits.x * (int)limits.y * (int)limits.z;

        // Rotasyonel Parent scale ayarla
        gameObject.transform.position = Vector3.one + offset;
        gameObject.transform.localScale = limits + limits + offset;

        // Var olanlarý yok et
        foreach (Transform child in gameObject.transform)
            Destroy(child.gameObject);


        // Spawn
        int i = -1;
        for (int x = 0; x < limits.x; x++)
        {
            for (int y = 0; y < limits.y; y++)
            {
                for (int z = 0; z < limits.z; z++)
                {
                    i++;
                    GameObject go = Instantiate(cubePrefab, gameObject.transform, true);
                    go.transform.position = new Vector3(x, y, z) + new Vector3(x, y, z);
                    go.GetComponent<Element>().index = i;
                    go.GetComponent<Element>().isActive = levelData.elementDatas[i].isActive;
                    go.GetComponent<Element>().SetElementData(levelData.elementDatas[i]);

                    if (!levelData.elementDatas[i].isActive)
                        go.GetComponent<MeshRenderer>().material = disabledMaterial;


                    spawnedElements.Add(go.GetComponent<Element>());
                }
            }
        }

        OnCountChange?.Invoke(count);
    }


    public void SpawnElements(Vector3 spawnLimits)
    {
        limits = spawnLimits;

        spawnedElements = new List<Element>();

        count = (int)limits.x * (int)limits.y * (int)limits.z;

        // Rotasyonel Parent scale ayarla
        gameObject.transform.position = Vector3.one + offset;
        gameObject.transform.localScale = limits + limits + offset;

        // Var olanlarý yok et
        foreach (Transform child in gameObject.transform)
            Destroy(child.gameObject);


        // Spawn
        int i = -1;
        for (int x = 0; x < limits.x; x++)
        {
            for (int y = 0; y < limits.y; y++)
            {
                for (int z = 0; z < limits.z; z++)
                {
                    i++;
                    GameObject go = Instantiate(cubePrefab, gameObject.transform, true);
                    go.transform.position = new Vector3(x, y, z) + new Vector3(x, y, z);
                    go.GetComponent<Element>().index = i;

                    spawnedElements.Add(go.GetComponent<Element>());
                }
            }
        }

        OnCountChange?.Invoke(count);
    }

    public void SetElementDatas()
    {
        dicElementTypes.Clear();

        count = spawnedElements.Count(p => p.isActive);
        OnCountChange?.Invoke(count);

        PrepareElementTypeDictionary();

        SetElementDataToCubeObjects();

        SetLastVersionOfElementData();
    }

    private void SetLastVersionOfElementData()
    {
        determinedElementData = new List<ElementData>();

        foreach (var item in spawnedElements)
            determinedElementData.Add(item.GetElementData());

        OnElementDataChange?.Invoke(determinedElementData, limits);
    }

    private void SetElementDataToCubeObjects()
    {
        // Dictionary kullanýlarak Elementlerin görünümleri dolduruluyor.

        spawnedElementDatas = new List<ElementData>();

        foreach (var item in dicElementTypes)
        {
            for (int j = 0; j < item.Value; j++)
            {
                spawnedElementDatas.Add(elements.Where(p => p.elementType == item.Key).SingleOrDefault());
            }
        }

        // elementler küplere rastgele daðýtýlýyor
        foreach (var item in spawnedElements)
        {
            if (!item.isActive)
            {
                item.SetElementData(new ElementData());
                continue;
            }

            if (spawnedElementDatas.Count <= 0)
                break;

            int rndIndex = UnityEngine.Random.Range(0, spawnedElementDatas.Count - 1);
            var rndElement = spawnedElementDatas[rndIndex];
            item.SetElementData(rndElement);
            spawnedElementDatas.RemoveAt(rndIndex);
        }
    }

    private void PrepareElementTypeDictionary()
    {
        // Dictionary set ediliyor

        // Her seferinde avacodo ve ilk sýradakilerin fazla olmasýný engellemek için listeyi kardýk
        List<ElementData> shuffledList = elements.Where(p => p.isActive).OrderBy(a => UnityEngine.Random.value).ToList();

        foreach (var item in shuffledList)
            if (!dicElementTypes.ContainsKey(item.elementType))
                dicElementTypes.Add(item.elementType, 0);


        // Dictionary dolduruluyor
        int i = 0;
        while (dicElementTypes.Values.Sum() + 3 <= count)
        {
            i++;

            foreach (var item in shuffledList)
            {
                dicElementTypes[item.elementType] = 3 * i;

                if (dicElementTypes.Values.Sum() + 3 > count)
                {
                    break;
                }
            }
        }
    }
}
