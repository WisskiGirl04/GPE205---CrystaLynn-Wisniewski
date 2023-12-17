using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Controller : MonoBehaviour
{
    //Variable to hold our Pawn
    public Pawn pawn;

    public float score;
    public float scoreToAdd;

    //public int startingLives;
    public float currentLives;
    public float respawnsLeft;

    public Text livesText;
    public Text scoreText;
    // Start is called before the first frame update
    public virtual void Start()
    {
        respawnsLeft = GameManager.instance.startingLives;
        if (pawn.controller.name == "PlayerController")
        {
            Debug.Log("Called in controller");
            foreach (Text text in pawn.GetComponentsInChildren<Text>())
            {
                if (text.name == "Score")
                {
                    scoreText = text;
                    Debug.Log(this.scoreText.name);
                }
                if (text.name == "Lives")
                {
                    currentLives = GameManager.instance.startingLives;
                    livesText = text;
                    this.livesText.text = currentLives.ToString();
                }
            }
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
        if (pawn.controller.name == "PlayerController")
        {
            this.score += scoreToAdd;
            this.scoreText.text = score.ToString();
        }
    }
    public virtual void SubtractFromLives()
    {
        if (pawn.controller.name == "PlayerController")
        {
            this.currentLives--;
            this.livesText.text = currentLives.ToString();
        }
    }
}
