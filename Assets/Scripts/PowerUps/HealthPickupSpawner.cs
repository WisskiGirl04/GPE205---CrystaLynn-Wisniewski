using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupSpawner : MonoBehaviour
{
    public GameObject pickupPrefab;
    public float spawnDelay;
    private float nextSpawnTime;
    private Transform tf;
    private GameObject spawnedPickup;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        // If the health pickup is not already there then spawn one
        if(spawnedPickup == null)
        {
            if(Time.time >= nextSpawnTime)
            {
                // Spawn the pickup and set the next spawn time
                spawnedPickup = Instantiate(pickupPrefab, transform.position, Quaternion.identity) as GameObject;
                nextSpawnTime = Time.time + spawnDelay;
            }
        }
        else
        {
            // since the health pickup is already there postpone the spawn
            nextSpawnTime = Time.time + spawnDelay;
        }
    }
}
