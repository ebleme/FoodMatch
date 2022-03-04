using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchArea : MonoBehaviour
{
    [SerializeField]
    Transform[] slots;

    ElementClickHandler elementClickHandler;

    // 0 dan 6 ye kadar
    List<GameObject> elementsOnSlots;

    // Tiplere göre listelenmesi
    Dictionary<ElementTypes, List<ElementData>> elementDataDictionary;

    // Rollback Power Up butonu için
    Queue<GameObject> elementHistory = new Queue<GameObject>();

    // Vector3 pozisyonu
    public event Action<Vector3> OnPointGathered;
    public event Action OnGameOver;


    bool exploding = false;

    private void Awake()
    {
        elementDataDictionary = new Dictionary<ElementTypes, List<ElementData>>();
        elementsOnSlots = new List<GameObject>();

        elementClickHandler = FindObjectOfType<ElementClickHandler>();
        elementClickHandler.OnElementSelected += ElementClickHandler_OnElementSelected;
    }

    // Bir element seçildiðinde
    private void ElementClickHandler_OnElementSelected(GameObject obj)
    {
        obj.GetComponent<BoxCollider>().enabled = false;
        obj.GetComponent<QuickOutline>().enabled = true;

        ElementData data = obj.GetComponent<Element>().GetElementData();

        int slotIndex = DetermineSlot(data);

        ShiftObJectsRightOnSlots(slotIndex, obj);

        MoveToSlot(obj, slotIndex);

        AddToDictionary(data);

        //ExplodeIfTriple();

        StartCoroutine(CheckIfGameOver());

        AddToHistory(obj);
    }

    private IEnumerator CheckIfGameOver()
    {
        yield return new WaitForSeconds(1);

        if (!exploding && elementsOnSlots.Count >= slots.Length)
            OnGameOver?.Invoke();
    }

    private void ExplodeIfTriple()
    {
        int index = -1;
        foreach (var item in elementDataDictionary)
        {
            if (item.Value.Count == 3)
            {
                index++;
                exploding = true;
                Explode(item, index);
                break;
            }

            index += item.Value.Count;
        }
    }

    // Gathers elements together and explodes them
    private void Explode(KeyValuePair<ElementTypes, List<ElementData>> item, int startingIndex)
    {
        var shouldDelete = elementsOnSlots.Where(p => p.GetComponent<Element>().elementData.elementType == item.Key).ToList();

        Sequence sequence = DOTween.Sequence();

        shouldDelete[0].transform.DOMove(shouldDelete[1].transform.position, .3f).SetDelay(.3f);
        shouldDelete[2].transform.DOMove(shouldDelete[1].transform.position, .4f).SetDelay(.3f).OnComplete(() =>
        {
            OnPointGathered?.Invoke(shouldDelete[1].transform.position);

            foreach (var item in shouldDelete)
            {
                elementsOnSlots.Remove(item);
                Destroy(item);
            }

            RemoveFromDictionary(item.Key);

            ShiftRemainingElementsToLeft();

            exploding = false;

        });
    }


    // Patlama yaþandýktan sonra kalan elementleri sola kaydýrýr
    private void ShiftRemainingElementsToLeft()
    {
        for (int i = 0; i < elementsOnSlots.Count; i++)
            elementsOnSlots[i].transform.DOMove(slots[i].position, .3f);
    }


    // hangi slota ekleneceðini belirler
    private int DetermineSlot(ElementData data)
    {
        bool exist = false;
        for (int i = 0; i < elementsOnSlots.Count; i++)
        {
            if (elementsOnSlots[i] == null)
                return i;

            if (elementsOnSlots[i].GetComponent<Element>().elementData.elementType == data.elementType)
            {
                exist = true;
                continue;
            }

            if (exist)
            {
                return i;
            }
        }

        return elementsOnSlots.Count;
    }


    // Dictionary e ekler
    private void AddToDictionary(ElementData data)
    {
        if (elementDataDictionary.ContainsKey(data.elementType))
        {
            var list = elementDataDictionary[data.elementType];

            if (list == null)
                list = new List<ElementData>();

            list.Add(data);

            elementDataDictionary[data.elementType] = list;
        }
        else
        {
            elementDataDictionary[data.elementType] = new List<ElementData>() { data };

        }
    }


    // Dictionary den siler
    private void RemoveFromDictionary(ElementTypes type)
    {
        if (elementDataDictionary.ContainsKey(type))
            elementDataDictionary.Remove(type);
    }


    private void MoveToSlot(GameObject obj, int slotIndex)
    {
        try
        {
            Vector3 target = slots[slotIndex].position;
            obj.GetComponent<QuickOutline>().enabled = false;
            obj.transform.parent = gameObject.transform;

            obj.transform.DORotate(Vector3.zero, .2f);
            obj.transform.DOMove(target, .3f).OnComplete(() =>
            {
                //  Eðer üçlemiþse Slota gittikten sonra patlat
                ExplodeIfTriple();
            });
        }
        catch
        {
        }
    }


    /// Shift elements to right if there is any in given slotIndex
    private void ShiftObJectsRightOnSlots(int slotIndex, GameObject obj)
    {
        try
        {
            elementsOnSlots.Insert(slotIndex, obj);

            for (int i = slotIndex + 1; i < elementsOnSlots.Count; i++)
                elementsOnSlots[i].transform.DOMove(slots[i].position, .3f);

        }
        catch
        {
        }
    }


    private void AddToHistory(GameObject obj)
    {
        elementHistory.Enqueue(obj);
    }


#warning RolBakc olayýna bak sadece patlamamýþ olanlar rollback olabilsin
    // UI daki Rollback butonundan tetiklenir
    public void RollBackPower()
    {
        if (elementHistory.Count <= 0)
            return;

        GameObject obj = elementHistory.Dequeue();

        obj.SetActive(true);

        obj.GetComponent<BoxCollider>().enabled = true;
        obj.transform.parent = FindObjectOfType<ElementSpawner>().transform;
        obj.transform.DORotate(Vector3.zero, .5f);
        obj.transform.DOMove(obj.GetComponent<Element>().initialPosition, .5f);
    }
}
