using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController characterController;
    private float gravity = 9.81f;
    [SerializeField]
    private float speed = 3.5f;
    private float timeBetweenAttacks = 2.5f;
    private float timer = 0;

    //HP system
    public int healthPoints;
    [SerializeField]
    private int manaPoints;
    [SerializeField]
    private int damageDealt;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        healthPoints = 100;
        manaPoints = 100;
        damageDealt = 10;
    }

    void Update()
    {
        Movement();
    }
    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * speed;
        velocity.y -= gravity;  //apply gravity
        characterController.Move(velocity * Time.deltaTime);
    }

    public void DamagePlayer(int damageAmount)
    {
        healthPoints -= damageAmount;
        if (healthPoints < 0)
        {
            healthPoints = 0;
            Die();
        }
    }
    public void SpendMana(int spentMana)
    {
        if (hasEnoughMana(spentMana))
        {
            manaPoints -= spentMana;
            if (manaPoints < 0)
            {
                manaPoints = 0;
            }
        }
        else
        {
            Debug.Log("Nincs elég mana");
        }
    }

    private bool hasEnoughMana(int manaCost)
    {
        return manaPoints - manaCost > 0;
    }
    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenAttacks)
            {
                Enemy enemy = other.GetComponent<Enemy>();
                enemy.DamageEnemy(damageDealt);
                timer = 0.0f;
            }
        }
    }
}
