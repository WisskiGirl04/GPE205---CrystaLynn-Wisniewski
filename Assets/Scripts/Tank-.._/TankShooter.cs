using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TankShooter : Shooter
{
    public Transform firepointTransform;
    private float nextTimeCanShoot;

    // Start is called before the first frame update
    public override void Start()
    {
        nextTimeCanShoot = Time.time;
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    public override void Shoot(GameObject bulletPrefab, float fireForce, float damageDone, float lifespan, float fireRate)
    {
        float secondsPerShot;
        float shotsPerSecond;

        shotsPerSecond = fireRate;
        secondsPerShot = 1 / shotsPerSecond;

        if (nextTimeCanShoot <= Time.time)
        {
            Debug.Log("Shooting time!");
            // Instantiate the new bullet
            GameObject newBullet = Instantiate(bulletPrefab, firepointTransform.position, firepointTransform.rotation) as GameObject;
            gameObject.GetComponent<AudioSource>().Play();
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
            nextTimeCanShoot = Time.time + secondsPerShot;
        }
    }

}
