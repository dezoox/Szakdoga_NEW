using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public Material basicMaterial;
    public Material hurtMaterial;
    Renderer rend;

    GameObject player;

    [SerializeField]
    private float enemyHealth;
    private float enemyMaxHealth = 100;
    [SerializeField]
    private int enemyDamage;
    private float timeBetweenAttacks = 3f;
    private float timer = 0;
    public GameObject dropHealth;
    [SerializeField]
    private GameObject dropHealthPotion;
    public GameObject healthBar;
    public Slider slider;

    private int expreienceReward = 5;

    void Start()
    {
        enemyHealth = enemyMaxHealth;
        enemyDamage = 17;
        rend = GetComponent<Renderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    void Update()
    {
        slider.value = getHealthValue();
        slider.transform.LookAt(player.transform);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenAttacks)
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.DamagePlayer(enemyDamage);
                    Debug.Log("sebződött a player" + enemyDamage);
                }
                timer = 0.0f;
            }
        }

    }

    public void DamageEnemy(float damage)
    {
        enemyHealth -= damage;
        if (rend != null)
        {
            StartCoroutine(ChangeMaterial());
        }
        if (enemyHealth <= 0)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = 1.5f;

            //Instantiate(dropHealth, spawnPosition, Quaternion.identity);
            Instantiate(dropHealthPotion, spawnPosition, Quaternion.identity);
            Player temp = player.GetComponent<Player>();
            if (temp != null)
            {
                temp.GetExperience(expreienceReward);
            }

            Destroy(this.gameObject);
        }
    }

    IEnumerator ChangeMaterial()
    {
        rend.material = hurtMaterial;
        yield return new WaitForSeconds(1.0f);
        rend.material = basicMaterial;
    }

    private float getHealthValue()
    {
        return enemyHealth / enemyMaxHealth;
    }
}
