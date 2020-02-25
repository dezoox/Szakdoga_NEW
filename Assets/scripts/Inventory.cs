using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Item> items = new List<Item>();

    [SerializeField]
    private List<Image> itemSlots;

    void Start()
    {
        //itemSlots = fillItemSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Item item)
    {
        items.Add(item);
    }

    public void RemoveItem(Item removableItem)
    {
        items.Remove(removableItem);
    }

    public List<Item> GetItems()
    {
        return items;
    }
    //private void displayItems()
    //{
    //    foreach (Image itemSlots in collection)
    //    {

    //    }
    //}
    //private List<Image> fillItemSlots()
    //{
    //    List<Image> temp = new List<Image>();
    //    foreach (Image itemSlot in )
    //    {

    //    }

    //    return temp;
    //}
}
