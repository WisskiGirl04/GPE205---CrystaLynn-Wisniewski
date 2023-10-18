using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{
    // Variable to hold the amount of damage to do
    public float damageDone;
    // Variable to hold the owner of this component
    public Pawn owner;

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
        // Get the Health component from the object that we are hitting and damaging
        Health otherHealth = other.gameObject.GetComponent<Health>();
        // Only do damage if the other object has health
        if (otherHealth != null)
        {
            // Damage
            otherHealth.TakeDamage(damageDone, owner);
        }
        // Destroy the object that's supposed to do damage
        // Whether it actually did damage to a Health component or not
        Destroy(gameObject);
    }

}
