using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private Space space = Space.World;

    //void OnMouseDrag()
    //{
    //    float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
    //    float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
    //    // select the axis by which you want to rotate the GameObject
    //    //transform.RotateAround(Vector3.down, XaxisRotation);

    //    objectToRotate.transform.Rotate(Vector3.down, XaxisRotation, space);
    //    objectToRotate.transform.Rotate(Vector3.right, YaxisRotation, space);

    //    //transform.RotateAround(Vector3.right, YaxisRotation);
    //}

    [SerializeField]
    private float PCRotationSpeed = 10f;

    [SerializeField]
    float MobileRotationSpeed = 0.4f;

    [SerializeField]
    public Camera cam;

    public event Action<bool> OnSwipeChanged;

    private void Reset()
    {
        cam = Camera.main;
    }

    bool isRotating;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * PCRotationSpeed;
            float rotY = Input.GetAxis("Mouse Y") * PCRotationSpeed;

            Vector3 right = Vector3.Cross(cam.transform.up, transform.position - cam.transform.position);
            Vector3 up = Vector3.Cross(transform.position - cam.transform.position, right);
            transform.rotation = Quaternion.AngleAxis(-rotX, up) * transform.rotation;
            transform.rotation = Quaternion.AngleAxis(rotY, right) * transform.rotation;

            if (!isRotating)
            {
                isRotating = true;
                OnSwipeChanged?.Invoke(isRotating);
            }
        }
        else
        {
            if (isRotating)
            {
                isRotating = false;
                OnSwipeChanged?.Invoke(isRotating);
            }
        }

        // get the user touch input
        foreach (Touch touch in Input.touches)
        {
            Ray camRay = cam.ScreenPointToRay(touch.position);
            RaycastHit raycastHit;
            if (Physics.Raycast(camRay, out raycastHit, 10))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    //Debug.Log("Touch phase began at: " + touch.position);
                    OnSwipeChanged?.Invoke(true);

                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    OnSwipeChanged?.Invoke(true);

                    transform.Rotate(touch.deltaPosition.y * MobileRotationSpeed,
                        -touch.deltaPosition.x * MobileRotationSpeed, 0, Space.World);
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    OnSwipeChanged?.Invoke(false);

                    //Debug.Log("Touch phase Ended");
                }
            }
        }
    }
}