using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    float timer = 0.0f;
    Vector3 rotation = new Vector3(0, 45, 0);
    private float healAmount = 70f;

    void Update()
    {
        timer += Time.deltaTime;
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
    public float getHealAmount()
    {
        return this.healAmount;
    }
}
