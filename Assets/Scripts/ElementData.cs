using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElementData
{
    public bool isActive;
    public Texture texture;
    public Vector3 position;
    public ElementTypes elementType;

    public ElementData()
    {
        isActive = false;
    }
}
