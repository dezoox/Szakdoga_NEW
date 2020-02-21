﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Material basicMaterial;
    public Material hurtMaterial;
    Renderer rend;

    [SerializeField]
    private int health;
    private int maxHealth = 100;
    [SerializeField]
    private int damageAmount;
    private float timeBetweenAttacks = 2.6f;
    private float timer = 0;
    public GameObject dropHealth;
    void Start()
    {
        health = maxHealth;
        damageAmount = 17;
        rend = GetComponent<Renderer>();
    }


    void Update()
    {

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

    public void DamageEnemy(int damage)
    {
        health -= damage;
        if (rend != null)
        {
            StartCoroutine(ChangeMaterial());
        }
        if (health <= 0)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 1.5f;

            Instantiate(dropHealth, spawnPosition, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    IEnumerator ChangeMaterial()
    {
        rend.material = hurtMaterial;
        yield return new WaitForSeconds(1.0f);
        rend.material = basicMaterial;
    }
}
