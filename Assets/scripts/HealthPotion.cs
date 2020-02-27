using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPotion : MonoBehaviour
{
    float timer = 0.0f;
    float destroyTimer = 0.0f;
    float timeWhenDestroy = 15f;

    private int healAmount = 25;
    public int HealAmount
    {
        get
        {
            return healAmount;
        }
    }

    Vector3 rotation = new Vector3(0, 45, 0);
    void Update()
    {
        timer += Time.deltaTime;
        destroyTimer += Time.deltaTime;
        if (destroyTimer > timeWhenDestroy)
        {
            Destroy(this.gameObject);
        }
        transform.Rotate(rotation * Time.deltaTime);
        if (timer < 1.0f)
        {
            transform.Translate(Vector3.up * Time.deltaTime);
        }
        if (timer > 1.0f)
        {
            transform.Translate(-Vector3.up * Time.deltaTime);
        }
        if (timer > 2.0f)
        {
            timer = 0.0f;
        }
    }
}
