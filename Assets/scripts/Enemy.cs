﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Material basicMaterial;
    public Material hurtMaterial;
    Renderer rend;

    GameObject player;

    [SerializeField]
    private float enemyHealth;
    private float enemyMaxHealth = 100;
    [SerializeField]
    private int enemyDamage;
    private float timeBetweenAttacks = 1.7f;
    private float timer = 0;
    public GameObject dropHealth;
    [SerializeField]
    private GameObject dropHealthPotion;
    public GameObject healthBar;
    public Slider slider;

    private int expreienceReward = 5;
    private Vector3 standingPosition;

    void Start()
    {
        
        enemyHealth = enemyMaxHealth;
        enemyDamage = 17;
        rend = GetComponent<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        slider.value = getHealthValue();
        slider.transform.LookAt(player.transform);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            standingPosition = transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(player.transform.position), 0.1f);
            DisableMovement();
            timer += Time.deltaTime;
            if (timer > timeBetweenAttacks)
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.DamagePlayer(enemyDamage);
                }
                timer = 0.0f;
            }
        }

    }

    public void DamageEnemy(float damage)
    {
        enemyHealth -= damage;
        if (rend != null)
        {
            StartCoroutine(ChangeMaterial());
        }
        if (enemyHealth <= 0)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 1.5f;
            
            Player temp = player.GetComponent<Player>();
            if (temp != null)
            {
                temp.GetExperience(expreienceReward);
            }
            Destroy(this.gameObject);
            Instantiate(dropHealthPotion, spawnPosition, Quaternion.identity);
        }
    }

    IEnumerator ChangeMaterial()
    {
        rend.material = hurtMaterial;
        yield return new WaitForSeconds(1.0f);
        rend.material = basicMaterial;
    }

    private float getHealthValue()
    {
        return enemyHealth / enemyMaxHealth;
    }

    /// <summary>
    /// Sets the position of the Enemy to the original position where it stood in every frame.
    /// This is needed so the colliding object cannot push it away in fight.
    /// </summary>
    private void DisableMovement()
    {
        transform.position = standingPosition;
    }
}
