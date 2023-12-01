using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TankPawn : Pawn
{

    private Vector3 cameraPosition;
    private Quaternion cameraRotation;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        
        if (gameObject.name != "SurvivorAI" && gameObject.name != "ObservantAI" && gameObject.name != "CowardlyAI" && gameObject.name != "AggressiveAI")
        {
            Debug.Log("'tankPawn in player controller :" + gameObject.name);
            GameObject cameraOne = FindObjectOfType<Camera>().gameObject;
            float positionX = gameObject.transform.position.x;
            float positionY = gameObject.transform.position.y;
            positionY = positionY + 50;
            float positionZ = gameObject.transform.position.z;
            cameraPosition = new Vector3(positionX, positionY, positionZ);

            float rotationW = gameObject.transform.rotation.w;
            float rotationY = gameObject.transform.rotation.y;
            float rotationZ = gameObject.transform.rotation.z;
            float rotationX = 90.0f;
            cameraRotation = new Quaternion(rotationX, rotationY, rotationZ, rotationW);
            Debug.Log("x: " + cameraRotation.x + ", y: " + cameraRotation.y + " z: " + cameraRotation.z + " w: " + cameraRotation.w);


            // Connect them by making the players pawn the camera's parent object
            cameraOne.transform.parent = gameObject.transform;
            cameraOne.transform.position = cameraPosition;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
    }

    public override void MoveForward()
    {
        mover.Move(transform.forward, moveSpeed);
    }

    public override void MoveBackward()
    {
        mover.Move(transform.forward, -moveSpeed);
    }

    public override void RotateClockwise()
    {
        mover.Rotate(turnSpeed);
    }

    public override void RotateCounterClockwise()
    {
        mover.Rotate(-turnSpeed);
    }

    public override void Shoot()
    {
        shooter.Shoot(bulletPrefab, fireForce, damageDone, bulletLifespan, fireRate);
    }

    public override void RotateTowards(Vector3 targetPosition)
    {
        // Find the vector to our target
        Vector3 vectorToTarget = targetPosition - transform.position;
        // Find the rotation to look at the target down the vector
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        // Rotate closer to that vector, but rotate by turnSpeed per Frame
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    public override void MakeNoise()
    {
        if(noise != null)
        {
            noise.volumeDistance = noiseMakerVolume;
        }
    }

    public override void StopNoise()
    {
        if(noise != null)
        {
            noise.volumeDistance = 0;
        }
    }

}