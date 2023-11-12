using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform playerSpawnTransform;

    // Prefabs
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;

    // AI Personality prefabs
    public GameObject aiAggresivePrefab;
    public GameObject aiCowardlyPrefab;
    public GameObject aiObservantPrefab;
    public GameObject aiSurvivorPrefab;

    // List that holds the player or players and AI enemies
    public List<PlayerController> playersList;
    public List<AIController> aiList;

    public int playersAmount;

    public PawnSpawnPoint[] spawnPoints;

    public MapGenerator mapGenerator;

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
        //SpawnPlayer();
        mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.GenerateMap();

        spawnPoints = FindObjectsOfType<PawnSpawnPoint>();
        foreach (PawnSpawnPoint spawnP in spawnPoints)
        {
            Debug.Log(spawnP.gameObject.name);
        }
        Debug.Log(spawnPoints.Length);
        SpawnPlayer(spawnPoints[Random.Range(0, spawnPoints.Length)]);

        SpawnAggressiveAI(spawnPoints[Random.Range(0, spawnPoints.Length)]);
        SpawnCowardlyAI(spawnPoints[Random.Range(0, spawnPoints.Length)]);
        SpawnObservantAI(spawnPoints[Random.Range(0, spawnPoints.Length)]);
        SpawnSurvivorAI(spawnPoints[Random.Range(0, spawnPoints.Length)]);


    }

    public void Update()
    {
        if(playersAmount <= 0)
        {
            SpawnPlayer(spawnPoints[Random.Range(0, spawnPoints.Length)]);
        }
    }

    public void SpawnPlayer(PawnSpawnPoint spawnPoint)
    {
        // Spawn the Player Controller at (0,0,0) with no rotation
        GameObject playerOne = Instantiate(playerControllerPrefab, Vector3.zero,
            Quaternion.identity) as GameObject;

        // Spawn our Tank and connect the player's controller
        GameObject tankOne = Instantiate(tankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        // Debug.Log("Player spawned at " + spawnPoint);
        playersAmount++;
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

    public void SpawnAggressiveAI(PawnSpawnPoint spawnPoint)
    {
        // Spawn the AI Controller at (0,0,0) with no rotation
        GameObject newAIContObj = Instantiate(aiAggresivePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        
        // Spawn the Pawn 
        GameObject newAIPawnObj = Instantiate(tankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        // Debug.Log("Aggressive spawned at " + spawnPoint);

        // Attach needed components and connect the controller to pawn
        Controller newController = newAIContObj.GetComponent<Controller>();
        Pawn newPawn = newAIPawnObj.GetComponent<Pawn>();

        newAIPawnObj.AddComponent<PowerUpManager>();
        newController.pawn = newPawn;

        newAIPawnObj.name = "AggressiveAI";

        newAIContObj.GetComponent<AIController>().patrolPoints[0] = spawnPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[1] = spawnPoint.nextWayPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[2] = spawnPoint.nextWayPoint.nextWayPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[3] = spawnPoint.nextWayPoint.nextWayPoint.nextWayPoint.transform;
    }
    public void SpawnCowardlyAI(PawnSpawnPoint spawnPoint)
    {
        // Spawn the AI Controller at (0,0,0) with no rotation
        GameObject newAIContObj = Instantiate(aiCowardlyPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        // Spawn the Pawn 
        GameObject newAIPawnObj = Instantiate(tankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        // Debug.Log("Coward spawned at " + spawnPoint);
        // Attach needed components and connect the controller to pawn

        Controller newController = newAIContObj.GetComponent<Controller>();
        Pawn newPawn = newAIPawnObj.GetComponent<Pawn>();

        newAIPawnObj.AddComponent<PowerUpManager>();
        newController.pawn = newPawn;

        newAIPawnObj.name = "CowardlyAI";

        newAIContObj.GetComponent<AIController>().patrolPoints[0] = spawnPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[1] = spawnPoint.nextWayPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[2] = spawnPoint.nextWayPoint.nextWayPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[3] = spawnPoint.nextWayPoint.nextWayPoint.nextWayPoint.transform;
    }
    public void SpawnObservantAI(PawnSpawnPoint spawnPoint)
    {
        Debug.Log("SpawnObservantAI called");
        // Spawn the AI Controller at (0,0,0) with no rotation
        GameObject newAIContObj = Instantiate(aiObservantPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        // Spawn the Pawn 
        GameObject newAIPawnObj = Instantiate(tankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        // Debug.Log("Observant spawned at " + spawnPoint);
        // Attach needed components and connect the controller to pawn

        Controller newController = newAIContObj.GetComponent<Controller>();
        Pawn newPawn = newAIPawnObj.GetComponent<Pawn>();

        newAIPawnObj.AddComponent<PowerUpManager>();
        newController.pawn = newPawn;

        newAIPawnObj.name = "ObservantAI";

        newAIContObj.GetComponent<AIController>().patrolPoints[0] = spawnPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[1] = spawnPoint.nextWayPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[2] = spawnPoint.nextWayPoint.nextWayPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[3] = spawnPoint.nextWayPoint.nextWayPoint.nextWayPoint.transform;
    }
    public void SpawnSurvivorAI(PawnSpawnPoint spawnPoint)
    {
        Debug.Log("SpawnSurvivorAI called");
        // Spawn the AI Controller at (0,0,0) with no rotation
        GameObject newAIContObj = Instantiate(aiSurvivorPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        // Spawn the Pawn 
        GameObject newAIPawnObj = Instantiate(tankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        //Debug.Log("Survivor spawned at " + spawnPoint);
        // Attach needed components and connect the controller to pawn

        Controller newController = newAIContObj.GetComponent<Controller>();
        Pawn newPawn = newAIPawnObj.GetComponent<Pawn>();

        newAIPawnObj.AddComponent<PowerUpManager>();
        newController.pawn = newPawn;

        newAIPawnObj.name = "SurvivorAI";

        newAIContObj.GetComponent<AIController>().patrolPoints[0] = spawnPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[1] = spawnPoint.nextWayPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[2] = spawnPoint.nextWayPoint.nextWayPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[3] = spawnPoint.nextWayPoint.nextWayPoint.nextWayPoint.transform;
    }
}
