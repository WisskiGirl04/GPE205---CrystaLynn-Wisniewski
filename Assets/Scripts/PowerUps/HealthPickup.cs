using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public HealthPowerup powerup;
    public AudioClip pickupClip;
    public AudioSource audioSource;

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


        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
        Debug.Log(audioSource.isPlaying);

        if (audioSource.isPlaying == false)
        {
            Debug.Log(audioSource.isPlaying);
        }
        // If our variable is not equal to null (the other object has a PowerUpManager and it is stored)cv 
        if (powerupManager != null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
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
            audioSource.Play();
            Debug.Log(audioSource.isPlaying);
            if (audioSource.isPlaying == false)
            {
                Debug.Log(audioSource.isPlaying);
            }
            //StartCoroutine(WaitTillClipEnd());
            float clipLength = audioSource.clip.length;
            
                // Destroy this pickup
                Destroy(this.gameObject);
            // Destroy this pickup
            //Destroy(this.gameObject);
        }
    }

    IEnumerator WaitTillClipEnd()
    {
        Debug.Log("WaitTillClipEnd called");
        yield return new WaitWhile(() => audioSource.isPlaying);
    }
}
