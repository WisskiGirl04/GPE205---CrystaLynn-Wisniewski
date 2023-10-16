using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Awake is called when the object is created - before Start even runs
    private void Awake()
    {
        // Check if this is the first instance
        if (instance == null)
        {
            // If true then store this instance
            instance = this;
            // Don't destroy it when we load a new scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If false and this is not the first, destroy this gameObject
            Destroy(gameObject);
        }
        
    }


    // Prefabs
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;

}
