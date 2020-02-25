using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalCameraLooking : MonoBehaviour
{
    private float mouseSpeed = 50f;
    float direction = 0f;

    void Start()
    {
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            direction = -Input.GetAxis("Mouse Y");
            transform.Rotate(direction * mouseSpeed * Time.deltaTime, 0, 0);
        }
    }
}
