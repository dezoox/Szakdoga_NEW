using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField]
    private int indexOfSlot;
    private HealthPotion healthPotion;
    private Transform playerTransform;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    private void Update()
    {
        if(transform.childCount <= 0)
        {
            inventory.isFull[indexOfSlot] = false;
        }
    }
    public void DropItem()
    {
        
        foreach (Transform child in transform)
        {
            child.GetComponent<SpawnPickupable>().SpawnDroppedItem();
            GameObject.Destroy(child.gameObject);
        }
    }
}
