using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class Health : MonoBehaviour
{
    // Variable to hold current Health
    public float currentHealth;
    // Variable to hold max amount of Health
    public float maxHealth;

    private float scoreToAdd;

    public Image healthCircle;

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
        Debug.Log(amount + " damage done to " + this.gameObject.name + " by " + source.name);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        this.healthCircle.fillAmount = this.currentHealth / this.maxHealth;
        if (currentHealth <= 0)
        {
            Die(source);
        }
    }

    public void Heal(float amount, Pawn source)
    {
        currentHealth = currentHealth + amount;
        Debug.Log(amount + " healed for " + gameObject.name + " by " + source.name);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        this.healthCircle.fillAmount = this.currentHealth / this.maxHealth;
        if (source.GetComponent<PlayerController>() != null) { }
    }

    public void Die (Pawn source)
    {
        Debug.Log("Uh oh!");
        if (source.controller != null)
        {
            scoreToAdd = source.controller.scoreToAdd;
            source.controller.AddToScore(scoreToAdd);
            Debug.Log(gameObject.name);
            Debug.Log(this.gameObject.name);
            Debug.Log(gameObject.GetComponent<Pawn>().controller.name);
            if (this.gameObject.GetComponent<Pawn>().controller.name == "PlayerController")
            {
                GameManager.instance.playersAmount--;
                Debug.Log("Die called, players amount is " + GameManager.instance.playersAmount);
                GameManager.instance.ActivateGameOverScreen();
        //        Destroy(this.gameObject.GetComponent<Controller>());
        //        Destroy(this.gameObject);

            }
            Debug.Log("Destroy controller next");
            Destroy(this.gameObject.GetComponent<Controller>());
            Debug.Log("Destroy gameObject next");
            Destroy(this.gameObject);
        }
    }

}
