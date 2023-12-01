using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTestTransitions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTransitionInputs();
    }

    public void ProcessTransitionInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GameManager.instance.ActivateTitleScreen();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameManager.instance.ActivateMainMenuScreen();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.ActivateGameplayState();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GameManager.instance.ActivateOptionsScreen();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GameManager.instance.ActivateCreditsScreen();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameManager.instance.ActivateGameOverScreen();
        }
    }
}
