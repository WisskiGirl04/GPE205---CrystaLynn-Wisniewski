using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    // Decimal variable for move speed
    public float moveSpeed;
    // Decimal variable for turning speed
    public float turnSpeed;
    // Variable to hold our Mover
    public Mover mover;
    // Variable to hold our turbo speed
    public float turboSpeed;
    // Variable to hold our Shooter component
    public Shooter shooter;
    // Variable for our bullet prefab
    public GameObject bulletPrefab;
    // Variable for our firing force
    public float fireForce;
    // Variable for our damage done
    public float damageDone;
    // Variable for how long the bullet exists if it doesn't collide
    public float bulletLifespan;
    // Variable for rate of fire
    public float fireRate;
    // Variable to hold NoiseMaker component
    public NoiseMaker noise;
    public float noiseMakerVolume;

    // Start is called before the first frame update
    public virtual void Start()
    {  
        mover = GetComponent<Mover>();
        shooter = GetComponent<Shooter>();
        noise = GetComponent<NoiseMaker>();
    }

    // Update is called once per frame
    public virtual void Update()
    { 
    }

    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void RotateClockwise();
    public abstract void RotateCounterClockwise();
    public abstract void Shoot();
    public abstract void RotateTowards(Vector3 targetPosition);
    public abstract void MakeNoise();
    public abstract void StopNoise();

}
