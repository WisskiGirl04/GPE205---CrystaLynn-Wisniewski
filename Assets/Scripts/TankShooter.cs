using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooter : Shooter
{
    public Transform firepointTransform;

    // Start is called before the first frame update
    public override void Start()
    {
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    public override void Shoot(GameObject bulletPrefab, float fireForce, float damageDone, float lifespan)
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
    }
}
