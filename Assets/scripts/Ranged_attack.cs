using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged_attack : MonoBehaviour
{
    private float speed = 4.0f;
    public float manaCost = 75f;
    private int damage = 55;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.Translate( player.transform.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
            }
        }
        if (other.tag == "PatrollingEnemy")
        {
            PatrollingEnemy enemy = other.GetComponent<PatrollingEnemy>();
            if(enemy != null)
            {
                enemy.DamageEnemy(damage);
            }
        }
    }
}
