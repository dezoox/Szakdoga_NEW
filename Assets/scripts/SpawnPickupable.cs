using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPickupable : MonoBehaviour
{
    public GameObject item;
    private Transform player;
    private float spawnDistance = 3f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void SpawnDroppedItem()
    {
        Vector3 spawnPosition = player.position + player.forward*spawnDistance;

        Instantiate(item, spawnPosition, Quaternion.identity);
    }
}
