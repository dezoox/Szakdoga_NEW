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
    private GameObject welcomeText3;
    [SerializeField]
    private GameObject startingText;
    [SerializeField]
    private GameObject level2Congrat_text;
    private float welcomeTextTimer = 0.0f;

    void Start()
    {
        welcomeText.SetActive(true);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }


    void Update()
    {
        welcomeTextTimer += Time.deltaTime;
        if (welcomeTextTimer > 4.0f)
        {
            welcomeText.SetActive(false);
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
            if (player.PlayerLevel == 1)
            {
                startingText.SetActive(true);
            }
            if (player.PlayerLevel == 2)
            {
                level2Congrat_text.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.LookAt(other.gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            startingText.SetActive(false);
            level2Congrat_text.SetActive(false);
        }
    }

}
