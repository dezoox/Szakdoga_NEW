using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealFromInventory : MonoBehaviour
{
    public Button button;
    private Player player;

    void Start()
    {
        button.onClick.AddListener(consumePotion);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void consumePotion()
    {
        player.HealFromInventory(true, this.gameObject);
    }
        
}
