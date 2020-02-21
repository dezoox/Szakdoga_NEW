using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Material basicMaterial;
    public Material hurtMaterial;
    private float speed = 5.0f;

    private float timeBetweenAttacks = 2.5f;
    private float timer = 0;

    private bool isCameraRotates = false;
    private float mouseSpeed = 50.0f;
    Renderer rend;
    [SerializeField]
    private int damageDealt;

    //HP and MANA system
    public float healthPoints;
    public float maxHealth = 100f;
    public float maxMana = 100;
    [SerializeField]
    private float manaPoints; 
    public Transform Healthbar;
    public Transform ManaBar;

    private float manaRegeneration = 1.0f / 60f;

    void Start()
    {
        healthPoints = maxHealth;
        manaPoints = maxMana;
        damageDealt = 10;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        getMana(manaRegeneration);

        transform.position += Input.GetAxis("Horizontal") * transform.right * speed * Time.deltaTime;
        transform.position += Input.GetAxis("Vertical") * transform.forward * speed * Time.deltaTime;

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
            SpendMana(25);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            getMana(10);
        }
    }
    public void DamagePlayer(int damageAmount)
    {
        healthPoints -= damageAmount;
        RescaleHealthBar();
        if (rend != null)
        {
            StartCoroutine(ChangeMaterial());
        }
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
            RescaleManaBar();
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
    private void getMana(float amount)
    {
        manaPoints += amount;
        RescaleManaBar();
        if(manaPoints > maxMana)
        {
            manaPoints = maxMana;
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
                    enemy.DamageEnemy(damageDealt);
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
                    enemy.DamageEnemy(damageDealt);
                }
                timer = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Heal")
        {
            if (healthPoints != maxHealth)
            {
                Destroy(other.gameObject);
                Heal(70);
            }
        }
    }

    void Heal(int amount)
    {
        healthPoints += amount;
        if (healthPoints > maxHealth)
        {
            healthPoints = maxHealth;
        }
        RescaleHealthBar();
    }

    private void RescaleHealthBar()
    {
        Healthbar.transform.localScale = new Vector3(healthPoints / maxHealth, 1.0f, 1.0f);
    }
    private void RescaleManaBar()
    {
        ManaBar.transform.localScale = new Vector3(manaPoints / maxMana, 1.0f, 1.0f);
    }
    IEnumerator ChangeMaterial()
    {
        rend.material = hurtMaterial;
        yield return new WaitForSeconds(1.0f);
        rend.material = basicMaterial;
    }
}
