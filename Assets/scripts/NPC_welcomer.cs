using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC_welcomer : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private GameObject welcomeText;
    [SerializeField]
    private GameObject welcomeText2;
    [SerializeField]
    private GameObject level3_text;
    [SerializeField]
    private GameObject level2Congrat_text;
    [SerializeField]
    private GameObject boostPickedUpText;
    [SerializeField]
    private GameObject hasWonText;
    private GameObject boss;
    private float welcomeTextTimer = 0.0f;

    private GameObject NPCWelcomer_canvas;
    private bool haveToRotate = false;
    private float timer = 0.0f;
    private bool asd = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        boss = GameObject.FindGameObjectWithTag("Boss");
        boss.SetActive(false);
        ActivateTextForSeconds(welcomeText, 4.0f);
        NPCWelcomer_canvas = GameObject.FindGameObjectWithTag("NPCWelcomer_canvas");
    }


    void Update()
    {
        if (haveToRotate)
        {
            float rotationSpeed = 50f;
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.Euler(0, 90, 0), rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (welcomeText.activeInHierarchy)
            {
                welcomeText.SetActive(false);
            }
            if (player.PlayerLevel == 1 && !player.IsBoostPickedUp)
            {
                ActivateTextForSeconds(welcomeText2, 30.0f);
            }

            if (player.PlayerLevel == 2 && !player.IsBoostPickedUp)
            {
                ActivateTextForSeconds(level2Congrat_text, 30.0f);
            }
            if (player.PlayerLevel == 3 && !player.IsBoostPickedUp)
            {
                ActivateTextForSeconds(level3_text, 30.0f);
            }
            if (player.IsBoostPickedUp && !player.HasKilledBoss)
            {
                ActivateTextForSeconds(boostPickedUpText, 30f);
                boss.SetActive(true);
            }
            if (player.HasKilledBoss)
            {
                ActivateTextForSeconds(hasWonText, 30f);
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Vector3.Distance(other.transform.position, this.transform.position) > 1f)
            {
                transform.LookAt(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            DeactivateAllTexts();
        }
    }

    private void ActivateTextForSeconds(GameObject text, float time)
    {
        StartCoroutine(CoroutineActivateTextForSeconds(text, time));
    }

    private IEnumerator CoroutineActivateTextForSeconds(GameObject textToActivate, float time)
    {
        textToActivate.SetActive(true);
        yield return new WaitForSeconds(time);
        textToActivate.SetActive(false);
    }

    private void DeactivateAllTexts()
    {
        foreach (Text item in NPCWelcomer_canvas.GetComponentsInChildren<Text>())
        {
            item.gameObject.SetActive(false);
        }
    }
}
