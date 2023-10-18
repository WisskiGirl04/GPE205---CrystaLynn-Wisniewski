using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Variable to hold current Health
    public float currentHealth;
    // Variable to hold max amount of Health
    public float maxHealth; 

    // Start is called before the first frame update
    void Start()
    {
        // 'Clamp' the current Health so 0 <= currentHealth >= maxHealth
        currentHealth = Mathf.Clamp (currentHealth, 0, maxHealth);
        // Set current Health to max Health
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void TakeDamage(float amount, Pawn source)
    {
        currentHealth = currentHealth - amount;
        Debug.Log(amount + " damage done to " + gameObject.name + " by " + source.name);
        if (currentHealth <= 0)
        {
            Die(source);
        }
    }

    public void Heal(float amount, Pawn source)
    {
        currentHealth = currentHealth + amount;
        Debug.Log(amount + " healed for " + gameObject.name + " by " + source.name);
    }

    public void Die (Pawn source)
    {
        Debug.Log("Uh oh!");
        Destroy(gameObject);
    }

}