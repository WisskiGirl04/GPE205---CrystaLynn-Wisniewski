using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public HealthPowerup powerup;
    public AudioClip pickupClip;
    public AudioSource audioSource;
    private float destroyDelay;
    private float destroyingTime;
    private float ogTriggerCount;
    private float nextTriggerCount;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        destroyDelay = audioSource.clip.length / 3f;
        ogTriggerCount = 0;
        nextTriggerCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

        audioSource = gameObject.GetComponent<AudioSource>();
        if (nextTriggerCount != 0 && ogTriggerCount < nextTriggerCount)
        {
            if (Time.time >= destroyingTime)
            {
                Debug.Log("Time is " + Time.time + " and Destroying time!");
                ogTriggerCount++;
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Variable to store other object's PowerupManager (PowerUpController) - if it has one
        PowerUpManager powerupManager = other.GetComponent<PowerUpManager>();
        Debug.Log("on trigger hit the time is " + Time.time);
        nextTriggerCount++;

        destroyingTime = Time.time + destroyDelay;
        // If our variable is not equal to null (the other object has a PowerUpManager and it is stored)cv 
        if (powerupManager != null)
        {
            audioSource.Play();
            if (other.GetComponent<Health>().currentHealth < other.GetComponent<Health>().maxHealth)
            {
                // Add the powerup
                powerupManager.Add(powerup);
            }
        }
    }
}
