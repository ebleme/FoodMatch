using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Level Designer da Camerayý hareket ettirmek için kullanýlýr
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float rotateAmount = 1f;

    [SerializeField]
    private float rotateSpeed = 1f;

    [SerializeField]
    private float scrollSpeed = 1f;

    [SerializeField]
    private float scrollWidth = 1f;

    [SerializeField]
    private float maxCameraHeight = 15f;

    [SerializeField]
    private float minCameraHeight = 0f;

    void Update()
    {
        MoveCamera();
        RotateCamera();
    }

    private void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        //detect rotation amount if ALT is being held and the Right mouse button is down
        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
        {
            destination.x -= Input.GetAxis("Mouse Y") * rotateAmount;
            destination.y += Input.GetAxis("Mouse X") * rotateAmount;
        }

        //if a change in position is detected perform the necessary update
        if (destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * rotateSpeed);
        }
    }

    private void MoveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = new Vector3(0, 0, 0);

        //Move the GameObject
        if (Input.GetKey("a"))
        {
            movement.x -= scrollSpeed;
        }
        if (Input.GetKey("s"))
        {
            movement.y -= scrollSpeed;

        }
        if (Input.GetKey("d"))
        {
            movement.x += scrollSpeed;
        }
        if (Input.GetKey("w"))
        {

            movement.y += scrollSpeed;
        }


        movement = Camera.main.transform.TransformDirection(movement);
        movement.z = 0;

        // zoom
        movement.z += scrollSpeed * Input.GetAxis("Mouse ScrollWheel");

        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;

        if (destination.y > maxCameraHeight)
        {
            destination.y = maxCameraHeight;
        }
        else if (destination.y < minCameraHeight)
        {
            destination.y = minCameraHeight;
        }

        if (destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * scrollSpeed);
        }
    }
}