using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 input;
    private float speed = 5.0f;

    private CharacterController characterController;
    private CameraController cameraController;
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
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
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
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);

        Vector3 forwardDirection = Camera.main.transform.forward;
        Vector3 rightDirection = Camera.main.transform.right;
        forwardDirection.y = 0;
        rightDirection.y = 0;
        forwardDirection = forwardDirection.normalized;
        rightDirection = rightDirection.normalized;

        transform.position += (forwardDirection * input.y + rightDirection * input.x) * Time.deltaTime * speed;
    }

    public void DamagePlayer(int damageAmount)
    {
        healthPoints -= damageAmount;
        if (healthPoints < 0)
        {
            healthPoints = 0;
            Destroy(this.gameObject);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenAttacks)
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(damageDealt);
                }
                timer = 0.0f;
            }
        }
    }
}
