using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Material basicMaterial;
    public Material hurtMaterial;
    private float movementSpeed = 5.0f;

    private float timeBetweenAttacks = 2.5f;
    private float timer = 0;

    private bool isCameraRotates = false;
    private float mouseSpeed = 50.0f;
    Renderer rend;
    [SerializeField]
    private int playerDamage;

    //HP and MANA system
    public float playerHealth;
    public float playerMaxHealth = 100f;
    public float playerMaxMana = 100;
    public float playerMana;
    public Transform Healthbar;
    public Transform ManaBar;

    private float playerManaRegeneration = 1.0f / 60f;


    public Ranged_attack rangedAttack;
    void Start()
    {
        playerHealth = playerMaxHealth;
        playerMana = playerMaxMana;
        playerDamage = 10;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        getMana(playerManaRegeneration);

        transform.position += Input.GetAxis("Horizontal") * transform.right * movementSpeed * Time.deltaTime;
        transform.position += Input.GetAxis("Vertical") * transform.forward * movementSpeed * Time.deltaTime;

        if (isCameraRotates)
        {
            transform.Rotate(transform.up, Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime);
        }
        if (Input.GetMouseButtonDown(1))
        {
            isCameraRotates = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            isCameraRotates = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            DamagePlayer(25);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            getMana(10);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 1.5f;

            if (hasEnoughMana(rangedAttack.manaCost))
            {
                Instantiate(rangedAttack, spawnPosition, Quaternion.identity);
                SpendMana(rangedAttack.manaCost);
            }
        }
    }
    public void DamagePlayer(int damageAmount)
    {
        playerHealth -= damageAmount;
        RescaleHealthBar();
        if (rend != null)
        {
            StartCoroutine(ChangeMaterial());
        }
        if (playerHealth < 0)
        {
            playerHealth = 0;
            Destroy(this.gameObject);
        }
    }
    public void SpendMana(float spentMana)
    {
        if (hasEnoughMana(spentMana))
        {
            playerMana -= spentMana;
            RescaleManaBar();
            if (playerMana < 0)
            {
                playerMana = 0;
            }
        }
        else
        {
            Debug.Log("Nincs elég mana");
        }
    }

    private bool hasEnoughMana(float manaCost)
    {
        return playerMana - manaCost > 0;
    }
    private void getMana(float amount)
    {
        playerMana += amount;
        RescaleManaBar();
        if (playerMana > playerMaxMana)
        {
            playerMana = playerMaxMana;
        }
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
                    enemy.DamageEnemy(playerDamage);
                }
                timer = 0.0f;
            }
        }
        if (other.tag == "PatrollingEnemy")
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenAttacks)
            {
                PatrollingEnemy enemy = other.GetComponent<PatrollingEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(playerDamage);
                }
                timer = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Heal")
        {
            if (playerHealth < playerMaxHealth)
            {
                Heal(70);
                Destroy(other.gameObject);
            }
        }
    }

    private void Heal(int amount)
    {
        playerHealth += amount;
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
        RescaleHealthBar();
    }

    private void RescaleHealthBar()
    {
        Healthbar.transform.localScale = new Vector3(playerHealth / playerMaxHealth, 1.0f, 1.0f);
    }
    private void RescaleManaBar()
    {
        ManaBar.transform.localScale = new Vector3(playerMana / playerMaxMana, 1.0f, 1.0f);
    }
    IEnumerator ChangeMaterial()
    {
        rend.material = hurtMaterial;
        yield return new WaitForSeconds(1.0f);
        rend.material = basicMaterial;
    }
}
