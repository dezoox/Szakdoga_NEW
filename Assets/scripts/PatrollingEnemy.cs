using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PatrollingEnemy : MonoBehaviour
{
    private float movementSpeed = 2.0f;
    private float rotationSpeed = 100f;
    private float rotationWaitTime = 0.5f;

    

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    void Start()
    {

    }
    void Update()
    {
        if (!isWandering)
        {
            StartCoroutine(Wander());
        }
        if (isRotatingRight)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        }
        if (isRotatingLeft)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
        }
        if (isWalking)
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }
    }

    private IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 3);
        int rotateLeftOrRight = Random.Range(1, 3);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 5);

        isWandering = true;
        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotationWaitTime);
        if(rotateLeftOrRight == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
        }
        if(rotateLeftOrRight == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
        }
        isWandering = false;
    }
}
