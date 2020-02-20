﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PatrollingEnemy : MonoBehaviour
{
    public Material basicMaterial;
    public Material hurtMaterial;
    Renderer rend;

    private float movementSpeed = 3.0f;
    private float rotationSpeed = 100f;
    private float rotationWaitTime = 0.5f;

    [SerializeField]
    private bool isWandering = false;

    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;


    private GameObject player;
    private float recognizeRange = 10.0f;
    private float timer;
    private float timeBetweenAttacks = 2.6f;
    [SerializeField]
    private int health;
    private int maxHealth = 100;
    [SerializeField]
    private int damageAmount;

    void Start()
    {
        rend = GetComponent<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        health = maxHealth;
        damageAmount = 17;
    }
    void Update()
    {
        if (RecognizePlayer())
        {
            isWandering = false;
            ChasePlayer();
        }
        else
        {
            if(!isWandering)
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
        if (rotateLeftOrRight == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
        }
        if (rotateLeftOrRight == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
        }
        isWandering = false;
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        if (rend != null)
        {
            StartCoroutine(ChangeMaterial());
        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator ChangeMaterial()
    {
        rend.material = hurtMaterial;
        yield return new WaitForSeconds(1.0f);
        rend.material = basicMaterial;
    }

    private void ChasePlayer()
    {
        Vector3 direction = player.transform.position - this.transform.position;
        direction.y = 0;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

        if (direction.magnitude > 2)
        {
            this.transform.Translate(0, 0, 0.05f);
        }
    }

    private bool RecognizePlayer()
    {
        return Vector3.Distance(player.transform.position, this.transform.position) < recognizeRange;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenAttacks)
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.DamagePlayer(damageAmount);
                }
                timer = 0.0f;
            }
        }
    }
}
