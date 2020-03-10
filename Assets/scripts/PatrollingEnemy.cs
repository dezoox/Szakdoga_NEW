using Assets.scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PatrollingEnemy : MonoBehaviour, IEnemy
{
    public Material basicMaterial;
    public Material hurtMaterial;
    Renderer rend;
    private NavMeshAgent nav;
    Vector3 playerDestination;
    Vector3 playerDirection;

    [SerializeField]
    private float movementSpeed = 3.0f;
    private float rotationSpeed = 100f;
    private float rotationWaitTime = 0.5f;

    [SerializeField]
    private bool isWandering = false;
    [SerializeField]
    private bool isRotatingLeft = false;
    [SerializeField]
    private bool isRotatingRight = false;
    [SerializeField]
    private bool isWalking = false;


    private GameObject player;
    private float recognizeRange = 10.0f;
    private float timer;
    private float timeBetweenAttacks = 1.7f;
    [SerializeField]
    private float enemyHealth;
    private float enemyMaxHealth = 100;
    [SerializeField]
    private int enemyDamage;

    public GameObject dropHealth;
    public GameObject healthBar;
    public Slider slider;

    private int expreienceReward = 5;

    void Start()
    {
        rend = GetComponent<Renderer>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = enemyMaxHealth;
        enemyDamage = 17;
    }
    void Update()
    {
        slider.value = getHealthValue();
        slider.transform.LookAt(player.transform);

        if (RecognizePlayer())
        {
            nav.isStopped = false;
            isWandering = false;
            isWalking = false;
            isRotatingLeft = false;
            isRotatingRight = false;
            ChasePlayer();
        }
        else
        {
            nav.isStopped = true;
            if (!isWandering)
            {
                StartCoroutine(Wander());
            }
            if (isRotatingRight)
            {
                transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
            }
            if (isRotatingLeft)
            {
                transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
            }
            if (isWalking)
            {
                transform.position += transform.forward * movementSpeed * Time.deltaTime;
            }
        }
    }

    private IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 3);
        int rotateLeftOrRight = Random.Range(1, 3);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 5);

        isWandering = true;
        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotationWaitTime);
        if (rotateLeftOrRight == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
        }
        if (rotateLeftOrRight == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
        }
        isWandering = false;
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
            Destroy(this.gameObject);

            Player temp = player.GetComponent<Player>();
            if (temp != null)
            {
                temp.GetExperience(expreienceReward);
            }
            Instantiate(dropHealth, spawnPosition, Quaternion.identity);
        }
    }

    private IEnumerator ChangeMaterial()
    {
        rend.material = hurtMaterial;
        yield return new WaitForSeconds(1.0f);
        rend.material = basicMaterial;
    }

    private void ChasePlayer()
    {
        playerDirection = player.transform.position - this.transform.position;
        playerDirection.y = 0;
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(playerDirection), 0.1f);

        float distanceBetweenPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        playerDestination = player.transform.position;
        nav.SetDestination(playerDestination);
    }

    private bool RecognizePlayer()
    {
        return Vector3.Distance(player.transform.position, this.transform.position) < recognizeRange;
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
                }
                timer = 0.0f;
            }
        }
    }

    private float getHealthValue()
    {
        return enemyHealth / enemyMaxHealth;
    }
}
