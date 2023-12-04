using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UIElements;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public enum GameState { TitleScreenState, MainMenuState, OptionsScreenState, CreditsScreenState, GameplayState, GameOverState }
    public GameState currentState;

    public static GameManager instance;
    public Transform playerSpawnTransform;

    // Prefabs
    public GameObject playerControllerPrefab;
    public GameObject playerTankPawnPrefab;
    public GameObject aiTankPawnPrefab;

    TankShooter tankShooter;

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

    public List<GameObject> allObjects;
    public bool destroyAllObjects;

    // Game States
    public GameObject TitleScreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsScreenStateObject;
    public GameObject CreditsScreenStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverScreenStateObject;

    public UnityEngine.UI.Toggle MapOfDay;
    public Text Seed;
    public UnityEngine.UI.Slider SFXVolume;
    public UnityEngine.UI.Slider MusicVolume;
    public InputField InputField;
    public InputFieldEditor InputFieldEditor;
    public InputEvent InputEvent;

    public GameObject[] MapOfDayToggleChildren;
    public GameObject[] SeedInputChildren;
    public GameObject[] TextAreaChildren;

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
        mapGenerator = GetComponent<MapGenerator>();
        //mapGenerator.GenerateMap();
        DeactivateAllStates();
        ActivateTitleScreen();

    }

    public void Update()
    {
        // Temp code :
        // here so that player consistently respawns after death for now
        /*
        if (currentState == GameState.GameplayState)
        {
            if (playersAmount <= 0)
            {
                SpawnPlayer(spawnPoints[Random.Range(0, spawnPoints.Length)]);
            }
        } */
        //Testing Scripting -- allObjects = GameObject.FindGameObjectsWithTag("MyGameObject");
        if (allObjects.Count <= 0)
        {
            foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("MyGameObject"))
            {
                allObjects.Add(Obj);
                Debug.Log("game object " + Obj.name + " to allObjects list in Game Manager script.");
            }
        }
        string seedPlaceholder = "0";
        //Seed.text = seedPlaceholder;
        if (MapOfDay != null)
        {
            foreach (GameObject toggleChild in MapOfDayToggleChildren)
            {
                if (toggleChild.GetComponent<Text>() != null)
                {
                    Debug.Log("Toggle children that are Text: " + toggleChild.GetComponentInChildren<Text>().name);
                }
                Debug.Log(toggleChild.name);
                if (toggleChild.name == "Random x Player Seed Input")
                {
                    foreach (GameObject inputFieldChild in SeedInputChildren)
                    {
                        if (inputFieldChild.GetComponent<Text>() != null)
                        {
                            Debug.Log("Input children that are Text: " + inputFieldChild.GetComponent<Text>().name);
                        }
                        Debug.Log(inputFieldChild.name);
                        if (inputFieldChild.name == "Text Area")
                        {
                            Debug.Log(inputFieldChild.GetComponent<Text>());
                            //Debug.Log(inputFieldChild.GetComponent<Text>().name);
                            //Seed = inputFieldChild.GetComponent<Text>();
                            if (Seed == null)
                            {
                                Debug.Log("seed is still null???");
                            }
                            foreach (GameObject textAreaChild in TextAreaChildren)
                            {
                                if("Text Area children that are Text: " + textAreaChild.GetComponentInChildren<Text>() != null)
                                {
                                    Debug.Log(textAreaChild.GetComponentInChildren<Text>().name);
                                }
                                Debug.Log(textAreaChild.name);
                                if (textAreaChild.name == "Seed Text")
                                {
                                    //Debug.Log(textAreaChild.GetComponent<Text>().text);
                                    //Debug.Log(textAreaChild.GetComponent<Text>());
                                    //Debug.Log(textAreaChild.GetComponent<Text>().name);
                                    //Seed = textAreaChild.GetComponent<Text>();
                                    if(Seed == null)
                                    {
                                        Debug.Log("seed is still null???");
                                    }
                                    //this.Seed.text = seedPlaceholder;
                                   // Debug.Log(Seed.name);
                                }
                            }
                        }
                    }
                }
            }
         //       Debug.Log(gameObject.GetComponentInChildren<TMP_InputField>().name);
        }
        //Seed.text
    }


    public void SpawnPlayer(PawnSpawnPoint spawnPoint)
    {
        // Spawn the Player Controller object at (0,0,0) with no rotation
        GameObject playerOne = Instantiate(playerControllerPrefab, Vector3.zero,
            Quaternion.identity) as GameObject;

        // Spawn our Tank object
        GameObject tankOne = Instantiate(playerTankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        // Debug.Log("Player spawned at " + spawnPoint); 
        playersAmount++;

        // Get the Player Controller component and Pawn component
        Controller controllerOne = playerOne.GetComponent<Controller>();
        Pawn pawnOne = tankOne.GetComponent<Pawn>();

        controllerOne.pawn = pawnOne;
        pawnOne.controller = controllerOne;

        // Should rename the playerOne variable
        tankOne.AddComponent<NoiseMaker>();
        pawnOne.noise = tankOne.GetComponent<NoiseMaker>();
        pawnOne.noiseMakerVolume = 3;

        tankOne.AddComponent<PowerUpManager>();

        playerOne.name = "PlayerController";
        tankOne.name = "PlayerPawn";

        // Connect the components
        controllerOne.pawn = pawnOne;
    }

    public void SpawnAggressiveAI(PawnSpawnPoint spawnPoint)
    {
        // Spawn the AI Controller at (0,0,0) with no rotation
        GameObject newAIContObj = Instantiate(aiAggresivePrefab, Vector3.zero, Quaternion.identity) as GameObject;
        
        // Spawn the Pawn 
        GameObject newAIPawnObj = Instantiate(aiTankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        // Debug.Log("Aggressive spawned at " + spawnPoint);

        // Attach needed components and connect the controller to pawn
        Controller newController = newAIContObj.GetComponent<Controller>();
        Pawn newPawn = newAIPawnObj.GetComponent<Pawn>();

        newAIPawnObj.AddComponent<PowerUpManager>();
        newController.pawn = newPawn;
        newPawn.controller = newController;

        newAIPawnObj.name = "AggressiveAI";
        newAIContObj.name = "AIAggressiveController";

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
        GameObject newAIPawnObj = Instantiate(aiTankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        // Debug.Log("Coward spawned at " + spawnPoint);
        // Attach needed components and connect the controller to pawn

        Controller newController = newAIContObj.GetComponent<Controller>();
        Pawn newPawn = newAIPawnObj.GetComponent<Pawn>();

        newAIPawnObj.AddComponent<PowerUpManager>();
        newController.pawn = newPawn;
        newPawn.controller = newController;

        newAIPawnObj.name = "CowardlyAI";
        newAIContObj.name = "AICowardlyController";

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
        GameObject newAIPawnObj = Instantiate(aiTankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        // Debug.Log("Observant spawned at " + spawnPoint);
        // Attach needed components and connect the controller to pawn

        Controller newController = newAIContObj.GetComponent<Controller>();
        Pawn newPawn = newAIPawnObj.GetComponent<Pawn>();

        newAIPawnObj.AddComponent<PowerUpManager>();
        newController.pawn = newPawn;
        newPawn.controller = newController;

        newAIPawnObj.name = "ObservantAI";
        newAIContObj.name = "AIObservantController";

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
        GameObject newAIPawnObj = Instantiate(aiTankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        //Debug.Log("Survivor spawned at " + spawnPoint);
        // Attach needed components and connect the controller to pawn

        Controller newController = newAIContObj.GetComponent<Controller>();
        Pawn newPawn = newAIPawnObj.GetComponent<Pawn>();

        newAIPawnObj.AddComponent<PowerUpManager>();
        newController.pawn = newPawn;
        newPawn.controller = newController;

        newAIPawnObj.name = "SurvivorAI";
        newAIContObj.name = "AISurvivorController";

        newAIContObj.GetComponent<AIController>().patrolPoints[0] = spawnPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[1] = spawnPoint.nextWayPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[2] = spawnPoint.nextWayPoint.nextWayPoint.transform;
        newAIContObj.GetComponent<AIController>().patrolPoints[3] = spawnPoint.nextWayPoint.nextWayPoint.nextWayPoint.transform;
    }

    //Helper function, deactivate all states
    private void DeactivateAllStates()
    {
        // Deactivate all Game States
        TitleScreenStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsScreenStateObject.SetActive(false);
        CreditsScreenStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
        GameOverScreenStateObject.SetActive(false);
    }

    public void ActivateTitleScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        // Activate the title screen
        TitleScreenStateObject.SetActive(true);
        // Do whatever needs to be done when the title screen starts.
        currentState = GameState.TitleScreenState;

    }
    public void ActivateMainMenuScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        // Activate the main menu screen
        MainMenuStateObject.SetActive(true);
        // Do Main menu
        currentState = GameState.MainMenuState;
    }
    public void ActivateOptionsScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        // Activate options screen
        OptionsScreenStateObject.SetActive(true);
        // Do Options screen
        currentState = GameState.OptionsScreenState;
    }
    public void ActivateCreditsScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        // activate credits screen
        CreditsScreenStateObject.SetActive(true);
        // do credits screen
        currentState = GameState.CreditsScreenState;
    }
    public void ActivateGameplayState()
    {
        // Deactivate all states
        DeactivateAllStates();
        destroyAllObjects = true;
        Debug.Log(destroyAllObjects);

        // Activate the Gameplay state
        GameplayStateObject.SetActive(true);
        // Do gameplay state
        //currentState = GameState.GameplayState;
    }
    public void ActivateGameOverScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        // Activate Game over screen
        GameOverScreenStateObject.SetActive(true);
        // Do game over screen
        currentState = GameState.GameOverState;
    }
}
