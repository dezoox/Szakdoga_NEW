using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged_attack : MonoBehaviour
{
    private float speed = 4.0f;
    private float manaCost = 75f;
    public float ManaCost
    {
        get
        {
            return manaCost;
        }
    }

    private int damage = 55;
    private GameObject player;
    private Vector3 direction;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        direction = player.transform.forward;
    }

    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
                Destroy(this.gameObject);
            }
        }
        if (other.tag == "PatrollingEnemy")
        {
            PatrollingEnemy enemy = other.GetComponent<PatrollingEnemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
                Destroy(this.gameObject);
            }
        }
    }
}
