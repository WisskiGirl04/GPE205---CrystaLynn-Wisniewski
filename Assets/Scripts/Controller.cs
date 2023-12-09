using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Controller : MonoBehaviour
{
    //Variable to hold our Pawn
    public Pawn pawn;

    public float score;
    public float scoreToAdd;
    public float playersLives;

    public Text livesText;
    public Text scoreText;
    // Start is called before the first frame update
    public virtual void Start()
    {
        this.scoreText.text = this.score.ToString();
        if (pawn.GetComponent<PlayerController>() == true)
        {
            scoreText = pawn.GetComponentInChildren<Text>();
            Debug.Log(this.scoreText.name);
        }
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
        this.score += scoreToAdd;
        this.scoreText.text = score.ToString();
    }

}
