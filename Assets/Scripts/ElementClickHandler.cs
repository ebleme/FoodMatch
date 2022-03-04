using System;
using UnityEngine;

public class ElementClickHandler : MonoBehaviour
{
    [SerializeField]
    private float rayMaxDistance = 10;

    public event Action<GameObject> OnElementSelected;

    ObjectRotator rotator;
    bool isRotating = false;

    private void Awake()
    {
        rotator = FindObjectOfType<ObjectRotator>();
        rotator.OnSwipeChanged += Rotator_OnSwipeChanged;
    }

    private void Rotator_OnSwipeChanged(bool val)
    {
        isRotating = val;
    }

    void Update()
    {
        if (isRotating)
            return;

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonDown(0))
        {
            int layerMask = 1 << 3;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, rayMaxDistance, layerMask))
                if (hit.transform.CompareTag(Constants.ElementTag))
                    OnElementSelected?.Invoke(hit.transform.gameObject);
        }

        foreach (Touch touch in Input.touches)
        {
            Ray camRay = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit raycastHit;

            if (Physics.Raycast(camRay, out raycastHit, rayMaxDistance))
                if (raycastHit.transform.CompareTag(Constants.ElementTag))
                    OnElementSelected?.Invoke(raycastHit.transform.gameObject);
        }
    }
}
