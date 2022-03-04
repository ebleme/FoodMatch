using System;
using UnityEngine;

/// <summary>
/// Editör sahnesinde elementin görünürlüðünü düzenler
/// </summary>
public class ElementViewChanger : MonoBehaviour
{
    [SerializeField]
    private Material normalMaterial;

    [SerializeField]
    private Material selectedElementMaterial;

    [SerializeField]
    private Material disabledMaterial;

    ElementClickHandler clickHandler;

    GameObject selectedElement;

    public event Action<Element> OnElementViewChanged;

    private void Awake()
    {
        clickHandler = FindObjectOfType<ElementClickHandler>();
        clickHandler.OnElementSelected += ClickHandlerEditor_OnElementSelected;
    }

    private void ClickHandlerEditor_OnElementSelected(GameObject obj)
    {
        if (selectedElement != null)
            selectedElement.GetComponent<MeshRenderer>().material = selectedElement.GetComponent<Element>().isActive ? normalMaterial : disabledMaterial;

        selectedElement = obj;
        selectedElement.GetComponent<MeshRenderer>().material = selectedElementMaterial;
    }

    public void Enable()
    {
        var element = selectedElement.GetComponent<Element>();
        element.isActive = true;

        selectedElement.GetComponent<MeshRenderer>().material = normalMaterial;

        OnElementViewChanged(element);
    }

    public void Disable()
    {
        selectedElement.GetComponent<Element>().isActive = false;
        selectedElement.GetComponent<MeshRenderer>().material = disabledMaterial;

        OnElementViewChanged(selectedElement.GetComponent<Element>());
    }
}
