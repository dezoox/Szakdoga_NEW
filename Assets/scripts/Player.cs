using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Material basicMaterial;
    public Material hurtMaterial;

    //Player stats
    private float movementSpeed = 5.0f;
    private float attackSpeed = 2.5f;
    private float playerDamage = 10f;
    private int playerManaRegeneration = 1 / 60;

    private float timer = 0;
    private bool isCameraRotates = false;
    private float mouseSpeed = 50.0f;
    Renderer rend;

    public Ranged_attack rangedAttack;


    //HP and MANA system
    [SerializeField]
    private int playerHealth;
    [SerializeField]
    private int playerMaxHealth = 100;
    [SerializeField]
    private int playerMaxMana = 100;
    [SerializeField]
    private int playerMana;
    [SerializeField]
    private Transform Healthbar;
    [SerializeField]
    private Transform ManaBar;

    //Leveling system
    [SerializeField]
    private int playerExperiencePoints;
    [SerializeField]
    private int playerLevel;
    [SerializeField]
    private int playerExperienceNeeded;
    [SerializeField]
    private Slider playerExperienceBar;
    [SerializeField]
    private Text playerCurrentLevelText;

    [SerializeField]
    private GameObject playerStats;

    [SerializeField]
    private Inventory inventory;

    void Start()
    {
        playerHealth = playerMaxHealth;
        playerMana = playerMaxMana;
        rend = GetComponent<Renderer>();

        setPlayerStartingExp();
        updatePlayerStats();
    }

    void Update()
    {
        getMana(playerManaRegeneration);

        transform.position += Input.GetAxis("Horizontal") * transform.right * movementSpeed * Time.deltaTime;
        transform.position += Input.GetAxis("Vertical") * transform.forward * movementSpeed * Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.C))
        {
            playerStats.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            playerStats.SetActive(false);
        }

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

            if (hasEnoughMana(rangedAttack.ManaCost))
            {
                Instantiate(rangedAttack, spawnPosition, Quaternion.identity);
                SpendMana(rangedAttack.ManaCost);
            }
        }
    }
    public void DamagePlayer(int damageAmount)
    {
        playerHealth -= damageAmount;
        rescaleHealthBar();
        if (rend != null)
        {
            StartCoroutine(ChangeMaterial());
        }
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Destroy(this.gameObject);
        }
    }
    public void SpendMana(int spentMana)
    {
        if (hasEnoughMana(spentMana))
        {
            playerMana -= spentMana;
            rescaleManaBar();
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

    private bool hasEnoughMana(int manaCost)
    {
        return playerMana - manaCost > 0;
    }
    private void getMana(int amount)
    {
        playerMana += amount;
        if (playerMana > playerMaxMana)
        {
            playerMana = playerMaxMana;
        }
        rescaleManaBar();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            timer += Time.deltaTime;
            if (timer > attackSpeed)
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
            if (timer > attackSpeed)
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
            heal(other, false);
        }
        else if (other.tag == "HealthPotion")
        {
            heal(other, true);
        }
    }
    private void heal(Collider other, bool isPotion)
    {
        dynamic heal;
        if (isPotion)
        {
            heal = other.GetComponent<HealthPotion>();
        }
        else
        {
            heal = other.GetComponent<Heal>();
        }
        if (playerHealth < playerMaxHealth && heal != null)
        {
            addHealthToPlayer(heal.HealAmount);
            Destroy(other.gameObject);
        }
    }

    private void addHealthToPlayer(int amount)
    {
        playerHealth += amount;
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
        rescaleHealthBar();
    }

    private void rescaleHealthBar()
    {
        Healthbar.transform.localScale = new Vector3((float)playerHealth / (float)playerMaxHealth, 1.0f, 1.0f);
    }
    private void rescaleManaBar()
    {
        ManaBar.transform.localScale = new Vector3((float)playerMana / (float)playerMaxMana, 1.0f, 1.0f);
    }
    IEnumerator ChangeMaterial()
    {
        rend.material = hurtMaterial;
        yield return new WaitForSeconds(1.0f);
        rend.material = basicMaterial;
    }

    public void GetExperience(int amount)
    {
        playerExperiencePoints += amount;
        playerExperienceBar.value = playerExperiencePoints;

        if (playerExperiencePoints >= playerExperienceNeeded)
        {
            levelUp();
        }
    }
    private void levelUp()
    {
        playerExperiencePoints = 0;
        playerExperienceBar.value = 0;
        playerExperienceNeeded += 5;
        playerExperienceBar.maxValue = playerExperienceNeeded;

        playerLevel += 1;
        playerCurrentLevelText.text = "Level: " + playerLevel.ToString();

        playerMaxHealth += 10;
        rescaleHealthBar();
        playerMaxMana += 10;
        rescaleManaBar();

        updatePlayerStats();
    }

    private void setPlayerStartingExp()
    {
        playerExperiencePoints = 0;
        playerLevel = 1;
        playerExperienceNeeded = 10;
        playerExperienceBar.value = playerExperiencePoints;
        playerExperienceBar.maxValue = playerExperienceNeeded;
        playerCurrentLevelText.text = "Level: " + playerLevel;
    }
    private void updatePlayerStats()
    {
        playerStats.SetActive(true);
        findAndWriteText("stat_manaRegeneration", playerManaRegeneration.ToString());
        findAndWriteText("stat_movementSpeed", movementSpeed.ToString());
        findAndWriteText("stat_damage", playerDamage.ToString());
        findAndWriteText("stat_attackSpeed", attackSpeed.ToString());
        findAndWriteText("stat_maxMana", playerMaxMana.ToString());
        findAndWriteText("stat_maxHealth", playerMaxHealth.ToString());
        playerStats.SetActive(false);
    }

    private void findAndWriteText(string name, string text)
    {
        Text temp = GameObject.Find(name).GetComponent<Text>();
        temp.text = text;
    }
}
