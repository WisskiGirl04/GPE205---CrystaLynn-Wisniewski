using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public List<PowerUp> powerUpsList;
    private List<PowerUp> removePowerUpsQueue;

    // Start is called before the first frame update
    void Start()
    {
        powerUpsList = new List<PowerUp>();
        // Add our remove list
        removePowerUpsQueue = new List<PowerUp>();
    }

    // Update is called once per frame
    void Update()
    {
        DecrementPowerupTimers();
    }
    private void LateUpdate()
    {
        ApplyRemovePowerUpsQueue();
    }

    public void Add(PowerUp powerupToAdd)
    {
        powerupToAdd.Apply(this);
        // Save the powerup to the list
        powerUpsList.Add(powerupToAdd);
    }
    public void Remove(PowerUp powerupToRemove)
    {
        powerupToRemove.Remove(this);
        // Add the powerup to our remove powerup queue
        removePowerUpsQueue.Add(powerupToRemove);
    }
    public void DecrementPowerupTimers()
    {
        // One at a time, for each powerup variable which are the objects in our list, we will do our loop body
        foreach(PowerUp powerUp in powerUpsList)
        {
            //  Subtract the time it took to draw the frame from the duration
            powerUp.duration -= Time.deltaTime;
            // If the time is upm remove the powerup
            if (powerUp.duration <= 0)
            {
                Remove(powerUp);
            }
        }
    }
    private void ApplyRemovePowerUpsQueue()
    {
        // Now that we are sure we are not iteration through "powerups", remove the powerups that are in our temp list
        foreach (PowerUp powerUp in removePowerUpsQueue)
        {
            powerUpsList.Remove(powerUp);
        }
        // Reset our temp list
        removePowerUpsQueue.Clear();
    }


}
