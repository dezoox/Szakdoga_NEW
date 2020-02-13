using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int health;
    [SerializeField]
    private int damageAmount;
    private float timeBetweenAttacks = 2.6f;
    private float timer = 0;
    void Start()
    {
        health = 100;
        damageAmount = 17;
    }

    
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenAttacks)
            {
                Player player = other.GetComponent<Player>();
                player.DamagePlayer(damageAmount);
                timer = 0.0f;
            }
        }
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
