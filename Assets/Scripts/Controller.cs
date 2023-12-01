using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    //Variable to hold our Pawn
    public Pawn pawn;

    public float score;
    public float scoreToAdd;
    // Start is called before the first frame update
    public virtual void Start()
    { 
    }

    // Update is called once per frame
    public virtual void Update()
    {
    }

    // Our child classes used to have to override the way the process input
    public virtual void ProcessInputs()
    {
    }

    public virtual void AddToScore(float scoreToAdd)
    {
        score += scoreToAdd;
    }

}
