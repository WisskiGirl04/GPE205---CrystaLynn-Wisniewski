using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform playerSpawnTransform;

    // List that holds the player or players
    public List<PlayerController> playersList;

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

    // Start is called before the first frame update
    private void Start()
    {
        // Temp Code - spawn player as soon as the GameManager starts
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        // Spawn the Player Controller at (0,0,0) with no rotation
        GameObject playerOne = Instantiate(playerControllerPrefab, Vector3.zero,
            Quaternion.identity) as GameObject;

        // Spawn our Tank and connect the player's controller
        GameObject tankOne = Instantiate(tankPawnPrefab, playerSpawnTransform.position,
            playerSpawnTransform.rotation) as GameObject;

        // Get the Player Controller component and Pawn component
        Controller controllerOne = playerOne.GetComponent<Controller>();
        Pawn pawnOne = tankOne.GetComponent<Pawn>();

        // Should rename the playerOne variable
        tankOne.AddComponent<NoiseMaker>();
        pawnOne.noise = tankOne.GetComponent<NoiseMaker>();
        pawnOne.noiseMakerVolume = 3;

        tankOne.AddComponent<PowerUpManager>();

        // Connect the components
        controllerOne.pawn = pawnOne;
    }

    // Prefabs
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;

}
