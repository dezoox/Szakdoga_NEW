using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Conditions
    int condIdle = 0;
    int condWalk = 1;
    int condAttack = 2;
    int condDamaged = 3;
    #endregion
    #region PauseGame
    [SerializeField]
    private GameObject pauseCanvas;
    public bool isPauseScreenActive;
    #endregion

    #region Player and camera movement
    private Vector3 spawnPosition = new Vector3(142.3f, 1f, 627.4f);
    private CharacterController characterController;
    private float gravity = 200f;
    public Transform cameraParent;
    Vector2 rotation = Vector2.zero;
    Vector3 movement = Vector3.zero;
    private float clampRange = 27;
    private float timer = 0;
    private bool isCameraRotates = false;
    private float mouseSpeed = 50.0f;
    private float movementSpeed = 5.0f;
    #endregion

    #region Player Stats
    [SerializeField]
    private GameObject playerStats;
    private float attackSpeed = 1f;
    private float playerDamage = 20f;
    private float playerManaRegeneration = 0.25f;
    private float playerHealthRegeneration = 0.05f;
    private float playerManaRegenerationGrowAmount = .05f;
    private float playerHealthRegenerationGrowAmount = .02f;
    #endregion

    #region Player health and mana system
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
    #endregion

    #region Player Level System
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
    private Text playerCurrentExpText;
    #endregion

    #region Ranged Attack
    private Vector3 rangedAttackSpawnPosition = Vector3.zero;
    public Ranged_attack rangedAttack;
    private Image canRangedAttack;
    private Color canRangedAttackColor = Color.green;
    private Color canNotRangedAttackColor = Color.gray;

    #endregion

    #region Endgame Content
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
    private GameObject boostLight;
    [SerializeField]
    private GameObject DeathScreen;
    #endregion

    #region Animation
    private Animator animator;
    #endregion

    void Start()
    {
        FindComponents();
        rotation.y = transform.eulerAngles.y;
        transform.position = spawnPosition;

        setPlayerStartingStats();
        updatePlayerStatsUI();
    }
    private void FindComponents()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        boostLight = GameObject.Find("Boost_Light");
        canRangedAttack = GameObject.FindGameObjectWithTag("RangedAttackIcon").GetComponent<Image>();
    }

    void Update()
    {
        displayManaAndHealth();
        Movement();
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetInteger("condition", condWalk);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetInteger("condition", condIdle);
        }

        decideRangedAttackIconColor();
        showPlayerStatsIfKeyPressed(KeyCode.C);
        moveCameraIfButtonPressed(1);

        if (!isPauseScreenActive)
        {
            shootIfKeyDown(KeyCode.Alpha1);
            regenerateManaAndHealth();
        }
        isPauseScreenActive = pauseCanvas.activeInHierarchy;
        togglePauseGameOnKeypress(KeyCode.Escape);
    }
    private void Movement()
    {

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float Xspeed = movementSpeed * Input.GetAxis("Vertical");
        float Yspeed = movementSpeed * Input.GetAxis("Horizontal");
        movement = (forward * Xspeed) + (right * Yspeed);
        movement.y -= gravity * Time.deltaTime;

        characterController.Move(movement * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        string tag = other.tag;
        if (tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            timer += Time.deltaTime;
            if (timer > attackSpeed)
            {
                if (enemy != null)
                {
                    animator.SetInteger("condition", condAttack);
                    enemy.DamageEnemy(playerDamage);
                }

                timer = 0.0f;
            }
        }
        if (tag == "PatrollingEnemy")
        {
            PatrollingEnemy enemy = other.GetComponent<PatrollingEnemy>();
            timer += Time.deltaTime;
            if (timer > attackSpeed)
            {
                if (enemy != null)
                {
                    animator.SetInteger("condition", condAttack);
                    enemy.DamageEnemy(playerDamage);
                }

                timer = 0.0f;
            }
        }
        if (tag == "Boss")
        {
            timer += Time.deltaTime;
            BOSS boss = other.GetComponent<BOSS>();
            if (timer > attackSpeed)
            {
                if (boss != null)
                {
                    animator.SetInteger("condition", condAttack);
                    boss.DamageEnemy(playerDamage);
                }

                timer = 0.0f;
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;

        if (tag == "Heal")
        {
            heal(other, false);
        }
        if (tag == "Boost")
        {
            if (playerLevel > 2)
            {
                isBoostPickedUp = true;
                boostLight.SetActive(false);
                Destroy(other.gameObject);
                playerMaxHealth += 400;
                addHealthToPlayer(400, null);
                playerMaxMana += 200;
                getMana(200);
                playerDamage = 90f;
                updatePlayerStatsUI();
            }
        }
        if (tag == "HealthPotion")
        {
            heal(other, true);
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

    public void DamagePlayer(int damageAmount)
    {
        animator.SetInteger("condition", condDamaged);
        playerHealth -= damageAmount;
        rescaleHealthBar();

        if (playerHealth < 1)
        {
            setDefaultHealthAndMana();
            Time.timeScale = 0; // Pause the game
            DeathScreen.SetActive(true);
        }
    }

    private void spendMana(int spentMana)
    {
        playerMana -= spentMana;
        rescaleManaBar();
        if (playerMana < 0)
        {
            playerMana = 0;
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

    private void rescaleHealthBar()
    {
        Healthbar.transform.localScale = new Vector3(playerHealth / playerMaxHealth, 1.0f, 1.0f);
    }

    public void GetExperience(int amount)
    {
        playerExperiencePoints += amount;
        playerExperienceBar.value = playerExperiencePoints;
        updatePlayerExperienceAmountText();

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
        playerCurrentExpText.text = playerExperiencePoints + "/" + playerExperienceNeeded;
        updatePlayerExperienceAmountText();

        playerLevel += 1;
        playerCurrentLevelText.text = "Level: " + playerLevel.ToString();

        playerMaxHealth += 10;
        playerHealth = playerMaxHealth;
        playerMaxMana += 10;
        playerMana = playerMaxMana;

        playerManaRegeneration += playerManaRegenerationGrowAmount;
        playerHealthRegeneration += playerHealthRegenerationGrowAmount;


        updatePlayerStatsUI();
    }

    private void setPlayerStartingStats()
    {
        setPlayerStartingHealthAndMana();
        setPlayerStartingExp();
    }

    private void setPlayerStartingHealthAndMana()
    {
        playerHealth = playerMaxHealth;
        playerMana = playerMaxMana;
    }

    private void setPlayerStartingExp()
    {
        playerExperiencePoints = 0;
        playerLevel = 1;
        playerExperienceNeeded = 5;
        playerExperienceBar.value = playerExperiencePoints;
        playerExperienceBar.maxValue = playerExperienceNeeded;
        playerCurrentLevelText.text = "Level: " + playerLevel;

        updatePlayerExperienceAmountText();
    }

    private void updatePlayerExperienceAmountText()
    {
        playerCurrentExpText.text = playerExperiencePoints + "/" + playerExperienceNeeded;
    }

    private void updatePlayerStatsUI()
    {
        playerStats.SetActive(true);
        findAndWriteText("stat_manaRegeneration", playerManaRegeneration.ToString());
        findAndWriteText("stat_movementSpeed", movementSpeed.ToString());
        findAndWriteText("stat_damage", playerDamage.ToString());
        findAndWriteText("stat_attackSpeed", attackSpeed.ToString());
        findAndWriteText("stat_maxMana", playerMaxMana.ToString());
        findAndWriteText("stat_maxHealth", playerMaxHealth.ToString());
        findAndWriteText("stat_healthRegeneration", playerHealthRegeneration.ToString());
        rescaleHealthBar();
        rescaleManaBar();
        playerStats.SetActive(false);
    }

    private void findAndWriteText(string name, string text)
    {
        Text temp = GameObject.Find(name).GetComponent<Text>();
        temp.text = text;
    }

    private void displayManaAndHealth()
    {
        healthText.text = (int)playerHealth + "/" + playerMaxHealth;
        manaText.text = (int)playerMana + "/" + playerMaxMana;
    }

    private void regenerateManaAndHealth()
    {
        getMana(playerManaRegeneration);
        addHealthToPlayer(playerHealthRegeneration, null);
    }

    private void decideRangedAttackIconColor()
    {
        if (hasEnoughMana(rangedAttack.ManaCost))
        {
            canRangedAttack.color = canRangedAttackColor;
        }
        else
        {
            canRangedAttack.color = canNotRangedAttackColor;
        }
    }

    private void showPlayerStatsIfKeyPressed(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            playerStats.SetActive(true);
        }
        else if (Input.GetKeyUp(key))
        {
            playerStats.SetActive(false);
        }
    }

    private void moveCameraIfButtonPressed(int buttonNumber)
    {
        if (isCameraRotates)
        {
            cameraRotation();
        }
        if (Input.GetMouseButtonDown(buttonNumber))
        {
            isCameraRotates = true;
        }
        if (Input.GetMouseButtonUp(buttonNumber))
        {
            isCameraRotates = false;
        }
    }

    private void cameraRotation()
    {
        rotation.y += Input.GetAxis("Mouse X") * mouseSpeed * Time.deltaTime;
        rotation.x -= Input.GetAxis("Mouse Y") * mouseSpeed * Time.deltaTime;
        rotation.x = Mathf.Clamp(rotation.x, -clampRange, clampRange);
        cameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
        transform.eulerAngles = new Vector2(0, rotation.y);
    }

    private void shootIfKeyDown(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            rangedAttackSpawnPosition = transform.position;
            rangedAttackSpawnPosition.y += 0.5f;

            if (hasEnoughMana(rangedAttack.ManaCost))
            {
                Instantiate(rangedAttack, rangedAttackSpawnPosition, Quaternion.identity);
                spendMana(rangedAttack.ManaCost);
            }
        }
    }

    private void setDefaultHealthAndMana()
    {
        playerHealth = 0.0f;
        playerMana = 0.0f;

        playerHealthRegeneration = 0.0f;
        playerManaRegeneration = 0.0f;
    }

    private void togglePauseGameOnKeypress(KeyCode key)
    {
        if (Input.GetKeyDown(key))
        {
            Time.timeScale = isPauseScreenActive ? 1f : 0f;
            isPauseScreenActive = !isPauseScreenActive;
            pauseCanvas.SetActive(isPauseScreenActive);
        }
    }
}
