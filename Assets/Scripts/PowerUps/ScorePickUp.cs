using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePickUp : MonoBehaviour
{
    public ScorePowerUp powerup;


    // Start is called before the first frame update
    void Start()
    {
    }
    void Update()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        // Variable to store other object's PowerupManager (PowerUpController) - if it has one
        PowerUpManager powerupManager = other.GetComponent<PowerUpManager>();
        // If our variable is not equal to null (the other object has a PowerUpManager and it is stored)
        if (powerupManager != null)
        {
            // Add the powerup
            powerupManager.Add(powerup);

            // Destroy this pickup
            Destroy(this.gameObject);
        }
    }
}
