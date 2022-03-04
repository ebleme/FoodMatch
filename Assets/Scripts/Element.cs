using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element : MonoBehaviour
{
    public ElementData elementData;

    public int index;
    public bool isActive = true;
    public Vector3 initialPosition;

    private void Start()
    {
        if (elementData != null)
        {
            gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", elementData.texture);

            initialPosition = transform.position;
        }
    }

    public void SetElementData(ElementData data)
    {
        elementData = data;
        elementData.isActive = isActive;
        initialPosition = transform.position;

        gameObject.GetComponent<Renderer>().material.SetTexture("_MainTex", elementData.texture);
    }

    public ElementData GetElementData()
    {
        return elementData;
    }
}
