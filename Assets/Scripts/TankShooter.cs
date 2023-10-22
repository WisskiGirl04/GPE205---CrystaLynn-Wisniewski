using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
    public Transform firepointTransform;
    public float timerDelay = 1.0f;
    public float timeUntilNextEvent;
    public int currentShots;

    // Start is called before the first frame update
    public override void Start()
    {
        timeUntilNextEvent = timerDelay;
    }

    // Update is called once per frame
    public override void Update(int maxShots, int currentShots)
    {
        timeUntilNextEvent -= Time.deltaTime;
        if (timeUntilNextEvent <= 0 && currentShots < maxShots)
        {
            Debug.Log("Reloading!");
            currentShots++;
            timeUntilNextEvent = timerDelay;
        }
    }

    public override void Shoot(GameObject bulletPrefab, float fireForce, float damageDone, float lifespan, int maxShots, int currentShots)
    {
        if (currentShots < 0)
        {
            // Instantiate the new bullet
            GameObject newBullet = Instantiate(bulletPrefab, firepointTransform.position, firepointTransform.rotation) as GameObject;
            // Get the DamageOnHit component from the new bullet
            DamageOnHit doh = newBullet.GetComponent<DamageOnHit>();
            // Check for if the newBullet has the component
            if (doh != null)
            {
                // If it does set the damageDone in the DamageOnHit component to the value passed in
                doh.damageDone = damageDone;
                // then set the owner to the pawn that shot this bullet, if there is one (otherwise, owner is empty and null).
                doh.owner = GetComponent<Pawn>();
            }
            // Get the rigidbody component of the new bullet
            Rigidbody rb = newBullet.GetComponent<Rigidbody>();
            // If it has one and isn't null
            if (rb != null)
            {
                // then AddForce to make it move forward
                rb.AddForce(firepointTransform.forward * fireForce);
            }
            // Destroy it after a set time
            Destroy(newBullet, lifespan);
            currentShots--;
        }
    }
}
