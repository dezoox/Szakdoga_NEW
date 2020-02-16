using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Vector3 cameraOffset;
    [Range(0.01f, 1.0f)]
    public float smoothFactor = 0.5f;
    private bool isLookingAtPlayer = false;

    private void Start()
    {
        cameraOffset = transform.position - player.position;
    }
    private void LateUpdate()
    {
        Vector3 newPosition = player.position + cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, smoothFactor);

        if (isLookingAtPlayer)
        {
            transform.LookAt(player);
        }
    }

}
