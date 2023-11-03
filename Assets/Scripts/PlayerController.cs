using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rotateClockwiseKey;
    public KeyCode rotateCounterClockwiseKey;
    public KeyCode turboKey;
    public KeyCode shootKey;

    // Start is called before the first frame update
    public override void Start()
    {
        // If(when) we have a GameManager
        if (GameManager.instance != null)
        {
            // And we have a player(s) list
            if (GameManager.instance.playersList != null)
            {
                // Add the PlayerControllerObject that is being created to the list
                GameManager.instance.playersList.Add(this);
            }
        }
        // Run the base (parent) class's Start
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    { 
        // Process our Keyboard Inputs
        ProcessInputs();

        // Run the base (parent) class's Update
        base.Update();
    }

    public override void ProcessInputs()
    {
        if (Input.GetKey(turboKey))
        {
            pawn.moveSpeed = pawn.turboSpeed;
        }
        if (Input.GetKey(moveForwardKey)) 
        {
            pawn.MoveForward();
            pawn.MakeNoise();
        }

        if (Input.GetKey(moveBackwardKey))
        {
            pawn.MoveBackward();
            pawn.MakeNoise();   
        }

        if (Input.GetKey(rotateClockwiseKey))
        {
            pawn.RotateClockwise();
            pawn.MakeNoise();
        }

        if (Input.GetKey(rotateCounterClockwiseKey))
        {
            pawn.RotateCounterClockwise();
            pawn.MakeNoise();
        }

        if (Input.GetKeyDown(shootKey))
        {
            pawn.Shoot();
            pawn.MakeNoise();
        }

        if(!Input.GetKey(moveForwardKey) && !Input.GetKey(moveBackwardKey) && !Input.GetKey(rotateClockwiseKey) && !Input.GetKey(rotateCounterClockwiseKey) && !Input.GetKeyDown(shootKey))
        {
            pawn.StopNoise();
        }

    }

    public void OnDestroy()
    {
        // If we have a GameManager
        if (GameManager.instance != null)
        {
            // And it has a player(s) list
            if (GameManager.instance.playersList != null)
            {
                // Unregister with the list
                GameManager.instance.playersList.Remove(this);
            }
        }
    }
}
