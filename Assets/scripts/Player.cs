using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private CharacterController characterController;
    private float gravity = 20f;
    public Transform cameraParent;
    Vector2 rotation = Vector2.zero;
    Vector3 movement = Vector3.zero;
    private float clampYRange = 27;

    public Material basicMaterial;
    public Material hurtMaterial;
    private GameObject boostLight;
    [SerializeField]
    private bool isBoostPickedUp = false;
    public bool IsBoostPickedUp
    {
        get
        {
            return isBoostPickedUp;
        }
    }
    [SerializeField]
    private bool hasKilledBoss = false;
    public bool HasKilledBoss
    {
        get
        {
            return hasKilledBoss;
        }
        set
        {
            hasKilledBoss = value;
        }
    }
    private float timer = 0;
    private bool isCameraRotates = false;
    private float mouseSpeed = 10.0f;
    Renderer rend;

    public Ranged_attack rangedAttack;

    //Player stats
    private float movementSpeed = 10.0f;
    private float attackSpeed = 3f;
    private float playerDamage = 20f;
    private float playerManaRegeneration = 0.1f;
    private float playerHealthRegen = 0.05f;

    //HP and MANA system
    [SerializeField]
    private float playerHealth;
    public float PlayerHealth
    {
        get
        {
            return playerHealth;
        }
    }
    [SerializeField]
    private int playerMaxHealth = 100;
    public int PlayerMaxHealth
    {
        get
        {
            return playerMaxHealth;
        }
    }
    [SerializeField]
    private int playerMaxMana = 100;
    [SerializeField]
    private float playerMana;
    [SerializeField]
    private Transform Healthbar;
    [SerializeField]
    private Transform ManaBar;

    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text manaText;

    //Leveling system
    [SerializeField]
    private int playerExperiencePoints;
    [SerializeField]
    private int playerLevel;
    public int PlayerLevel
    {
        get
        {
            return playerLevel;
        }
    }

    [SerializeField]
    private int playerExperienceNeeded;
    [SerializeField]
    private Slider playerExperienceBar;
    [SerializeField]
    private Text playerCurrentLevelText;

    [SerializeField]
    private GameObject playerStats;

    private Image canRangedAttack;
    private Color canRangedAttackColor = Color.green;
    private Color canNotRangedAttackColor = Color.gray;

    private Vector3 spawnPosition = new Vector3(142.3f,1f,627.4f);

    void Start()
    {
        rotation.y = transform.eulerAngles.y;
        characterController = GetComponent<CharacterController>();
        transform.position = spawnPosition;
        boostLight = GameObject.Find("Boost_Light");
        playerHealth = playerMaxHealth;
        playerMana = playerMaxMana;
        rend = GetComponent<Renderer>();
        canRangedAttack = GameObject.FindGameObjectWithTag("RangedAttackIcon").GetComponent<Image>();
        canRangedAttack.color = canRangedAttackColor;
        setPlayerStartingExp();
        updatePlayerStats();
    }

    void Update()
    {
        getMana(playerManaRegeneration);
        addHealthToPlayer(playerHealthRegen, null);

        DisplayManaAndHealth();

        Movement();
        
        if (hasEnoughMana(rangedAttack.ManaCost))
        {
            canRangedAttack.color = canRangedAttackColor;
        }
        else
        {
            canRangedAttack.color = canNotRangedAttackColor;
        }
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
            CameraRotation();
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
            spawnPosition.y += 0.5f;

            if (hasEnoughMana(rangedAttack.ManaCost))
            {
                Instantiate(rangedAttack, spawnPosition, Quaternion.identity);
                SpendMana(rangedAttack.ManaCost);
            }
        }
    }
    private void Movement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float Xspeed = movementSpeed * Input.GetAxis("Vertical");
        float Yspeed = movementSpeed * Input.GetAxis("Horizontal");
        Yspeed /= 2;
        movement = (forward * Xspeed) + (right * Yspeed);

        movement.y -= gravity * Time.deltaTime;
        characterController.Move(movement * Time.deltaTime);

    }
    private void CameraRotation()
    {
        rotation.y += Input.GetAxis("Mouse X") * mouseSpeed;
        rotation.x -= Input.GetAxis("Mouse Y") * mouseSpeed;
        rotation.x = Mathf.Clamp(rotation.x, -clampYRange, clampYRange);
        cameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, rotation.y);
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
        if(other.tag == "Boss")
        {
            timer += Time.deltaTime;
            if (timer > attackSpeed)
            {
                BOSS boss = other.GetComponent<BOSS>();
                if (boss != null)
                {
                    boss.DamageEnemy(playerDamage);
                }
                timer = 0.0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Heal")
        {
            heal(collision.collider, false);
        }
        if (collision.gameObject.tag == "HealthPotion")
        {
            heal(collision.collider, true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Boost")
        {
            if (playerLevel > 2)
            {
                isBoostPickedUp = true;
                boostLight.SetActive(false);
                Destroy(other.gameObject);
                playerMaxHealth += 400;
                addHealthToPlayer(400, null);
                playerMaxMana += 400;
                getMana(400);
                playerDamage = 90f;
                updatePlayerStats();
            }
        }
    }


    private void heal(Collider other, bool isPotion)
    {
        dynamic heal = null;
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
            addHealthToPlayer(heal.HealAmount, other);
        }
    }

    private void addHealthToPlayer(float amount, Collider other)
    {
        if (other != null)
        {
            Destroy(other.gameObject);
        }
        playerHealth += amount;
        if (playerHealth > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;
        }
        rescaleHealthBar();
    }

    public void HealFromInventory(bool isPotion, GameObject potionImage)
    {
        if (playerHealth < playerMaxHealth)
        {
            if (isPotion)
            {
                playerHealth += 25;
                if (playerHealth > playerMaxHealth)
                {
                    playerHealth = playerMaxHealth;
                }
                rescaleHealthBar();
            }
            else if (!isPotion)
            {
                playerHealth += 70;
                if (playerHealth > playerMaxHealth)
                {
                    playerHealth = playerMaxHealth;
                }
                rescaleHealthBar();
            }
            Destroy(potionImage);
        }
    }
    private void rescaleHealthBar()
    {
        Healthbar.transform.localScale = new Vector3(playerHealth / playerMaxHealth, 1.0f, 1.0f);
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
    private void SpendMana(int spentMana)
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
    private void getMana(float amount)
    {
        playerMana += amount;
        if (playerMana > playerMaxMana)
        {
            playerMana = playerMaxMana;
        }
        rescaleManaBar();
    }


    private void rescaleManaBar()
    {
        ManaBar.transform.localScale = new Vector3(playerMana / playerMaxMana, 1.0f, 1.0f);
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
        playerHealth = playerMaxHealth;
        playerMaxMana += 10;
        playerMana = playerMaxMana;

        updatePlayerStats();
    }

    private void setPlayerStartingExp()
    {
        playerExperiencePoints = 0;
        playerLevel = 1;
        playerExperienceNeeded = 5;
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
        rescaleHealthBar();
        rescaleManaBar();
        playerStats.SetActive(false);
    }

    private void findAndWriteText(string name, string text)
    {
        Text temp = GameObject.Find(name).GetComponent<Text>();
        temp.text = text;
    }

    private void DisplayManaAndHealth()
    {
        healthText.text = (int)playerHealth + "/" + playerMaxHealth;
        manaText.text = (int)playerMana + "/" + playerMaxMana;
    }
}
