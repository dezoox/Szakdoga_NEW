using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalCameraLooking : MonoBehaviour
{
    private float mouseSpeed = 50f;
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            float rotationX = -Input.GetAxis("Mouse Y");
            transform.Rotate(rotationX * mouseSpeed * Time.deltaTime, 0, 0);
        }
    }
}
