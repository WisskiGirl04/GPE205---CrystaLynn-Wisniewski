using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public HealthPowerup powerup;
    public AudioClip pickupClip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        // Variable to store other object's PowerupManager (PowerUpController) - if it has one
        PowerUpManager powerupManager = other.GetComponent<PowerUpManager>();
        // If our variable is not equal to null (the other object has a PowerUpManager and it is stored)cv 
        if(powerupManager != null)
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            Debug.Log(other);
            Debug.Log(other.name);
            Vector3 playLocation = gameObject.transform.position;
            AudioClip clip = gameObject.GetComponent<AudioSource>().clip;
            Debug.Log("play called");
            if (other.GetComponent<Health>().currentHealth < other.GetComponent<Health>().maxHealth)
            {
                // Add the powerup
                powerupManager.Add(powerup);
            }
            while(audioSource.isPlaying == false)
            {
                // Destroy this pickup
                Destroy(this.gameObject);
            }
            // Destroy this pickup
            //Destroy(this.gameObject);
        }
    }

}
