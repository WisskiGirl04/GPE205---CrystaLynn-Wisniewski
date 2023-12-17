using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UI;
using UnityEditor;
using UnityEngine.Audio;
using System.Linq;

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

    public TankShooter tankShooter;

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
    public bool isMultiplayer;
    public int startingLives;
    public GameObject playerOneCont;
    public GameObject playerTwoCont;

    public List<GameObject> allObjects;
    public bool destroyAllObjects;

    public Camera cameraOne;
    public Camera cameraTwo;

    AudioSource currentAudio;

    // Game States
    public GameObject TitleScreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsScreenStateObject;
    public GameObject CreditsScreenStateObject;
    public GameObject GameplayStateObject;
    public GameObject GameOverScreenStateObject;

    public UnityEngine.UI.Toggle MapOfDay;
    public UnityEngine.UI.Toggle Multiplayer;
    public TextMeshProUGUI SeedTwo;
    public UnityEngine.UI.Slider SFXVolume;
    public UnityEngine.UI.Slider MusicVolume;
    public AudioMixer audioMixer;

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
        playersAmount = 0;
        mapGenerator = GetComponent<MapGenerator>();
        //mapGenerator.GenerateMap();
        DeactivateAllStates();
        ActivateTitleScreen();

    }

    public void Update()
    {
        // Temp code :
        // here so that player consistently respawns after death for now
        
        if (currentState == GameState.GameplayState)
        {
            if (aiList.Count < 4)
            {
                string[] survivingAI = { "AIAggressive", "AICowardly", "AIObservant", "AISurvivor" };
                foreach (AIController ai in aiList)
                {
                    if (ai.name == "AIAggressive Controller")
                    {
                        survivingAI[0] = "survivor";
                    }
                    if (ai.name == "AICowardly Controller")
                    {
                        survivingAI[1] = "survivor";
                    }
                    if (ai.name == "AIObservant Controller")
                    {
                        survivingAI[2] = "survivor";
                    }
                    if (ai.name == "AISurvivor Controller")
                    {
                        survivingAI[3] = "survivor";
                    }
                }
                foreach (string survivors in survivingAI)
                {
                    if (survivors == "AIAggressive")
                    {
                        GameManager.instance.SpawnAggressiveAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                    }
                    if (survivors == "AICowardly")
                    {
                        GameManager.instance.SpawnCowardlyAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                    }
                    if (survivors == "AIObservant")
                    {
                        GameManager.instance.SpawnObservantAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                    }
                    if (survivors == "AISurvivor")
                    {
                        GameManager.instance.SpawnSurvivorAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                    }
                }

            }

            if (isMultiplayer == false)
            {
                if (playersAmount < 1)
                {
                    if (playerOneCont != null && playerOneCont.GetComponent<PlayerController>().respawnsLeft == 0)
                    {
                        playerOneCont.gameObject.SetActive(false);
                    }
                    if (playerOneCont != null && playerOneCont.GetComponent<PlayerController>().respawnsLeft > 0)
                    {
                        playersAmount++;
                        SpawnPlayer(spawnPoints[Random.Range(0, spawnPoints.Length)]);
                    }

                }
            }
            if (isMultiplayer == true)
            {
                if (playerOneCont != null && playerOneCont.GetComponent<PlayerController>().respawnsLeft == 0)
                {
                    playerOneCont.gameObject.SetActive(false);
                    cameraTwo.rect = new Rect(0, 0, 1, 1);
                }
                if (playerTwoCont != null && playerTwoCont.GetComponent<PlayerController>().respawnsLeft == 0)
                {
                    playerTwoCont.gameObject.SetActive(false);
                    cameraOne.rect = new Rect(0, 0, 1, 1);
                }
                if (playersAmount < 2)
                {
                    if (playerOneCont.gameObject.activeInHierarchy == false)
                    {
                        if (playerTwoCont.GetComponent<PlayerController>().respawnsLeft == 1)
                        {
                            Debug.Log("GameOver!");
                            ActivateGameOverScreen();
                        }
                    }
                    if (playerTwoCont.gameObject.activeInHierarchy == false)
                    {
                        if (playerOneCont.GetComponent<PlayerController>().respawnsLeft == 1)
                        {
                            Debug.Log("GameOver!");
                            ActivateGameOverScreen();
                        }
                    }
                    if (playerOneCont != null && playerOneCont.GetComponent<PlayerController>().respawnsLeft > 0)
                    {
                        if (playerOneCont.GetComponent<PlayerController>().respawnsLeft > playerOneCont.GetComponent<PlayerController>().currentLives)
                        {
                            playersAmount++;
                            Debug.Log("calling respawn on playerOne");
                            SpawnPlayer(spawnPoints[Random.Range(0, spawnPoints.Length)]);
                        }
                    } 
                    if (playerTwoCont != null && playerTwoCont.GetComponent<PlayerController>().respawnsLeft > 0)
                    {
                        if (playerTwoCont.GetComponent<PlayerController>().respawnsLeft > playerTwoCont.GetComponent<PlayerController>().currentLives)
                        {
                            playersAmount++;
                            Debug.Log("calling respawn on playerTwo");
                            SpawnPlayer(spawnPoints[Random.Range(0, spawnPoints.Length)]);
                        }
                    }
                }
            }
        } 
        //Testing Scripting -- allObjects = GameObject.FindGameObjectsWithTag("MyGameObject");
        if (allObjects.Count <= 0)
        {
            foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("MyGameObject"))
            {
                allObjects.Add(Obj);
                //Debug.Log("game object " + Obj.name + " to allObjects list in Game Manager script.");
            }
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (currentState == GameState.GameplayState)
            {
                OptionsScreenStateObject.SetActive(true);
            }
            if (currentState != GameState.GameplayState)
            {
                ActivateOptionsScreen();
            }
        }
    }

    public void SetMapOfDay()
    {
        if(MapOfDay.isOn == false)
        {
            Debug.Log("MapOfDay toggle set to false");
            mapGenerator.isMapOfTheDay = false;
        }
        if(MapOfDay.isOn == true)
        {
            Debug.Log("MapOFDay toggle set to true");
            mapGenerator.isMapOfTheDay = true;
        }
    }

    public void SpawnPlayer(PawnSpawnPoint spawnPoint)
    {
        // Spawn the Player Controller object at (0,0,0) with no rotation
        GameObject playerOne = Instantiate(playerControllerPrefab, Vector3.zero,
            Quaternion.identity) as GameObject;
       
        // Spawn our Tank object
        GameObject tankOne = Instantiate(playerTankPawnPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        // Debug.Log("Player spawned at " + spawnPoint);
        //playersAmount++;

        Debug.Log("playeronecontroller t/f for null : " + playerOneCont);
        Debug.Log("playertwocontroller t/f for null : " + playerTwoCont);

        // Get the Player Controller component and Pawn component
        Controller controllerOne = playerOne.GetComponent<Controller>();
        Pawn pawnOne = tankOne.GetComponent<Pawn>();
        Debug.Log("players amount is : " + playersAmount);
        if (playerOneCont != null && playerTwoCont != null)
        {
            Destroy(playerOne.gameObject);
        }

        if (playersAmount == 1)
        {
            Debug.Log("spawn player one!");
            if (playerOneCont != null && playerOneCont.GetComponent<PlayerController>().respawnsLeft > 0)
            {
                Destroy(playerOne.gameObject);
                //playersAmount--;
                playerOne = playerOneCont;
                controllerOne = playerOne.GetComponent<Controller>();
                controllerOne.respawnsLeft--;
            }
            playerOneCont = playerOne;
            tankOne.GetComponentInChildren<Camera>().name = tankOne.GetComponentInChildren<Camera>().name + playersAmount;
            cameraOne = tankOne.GetComponentInChildren<Camera>();
            Debug.Log(cameraOne.name);
        }

        if (playersAmount == 2)
        {
            Debug.Log("spawn player two!");
            if (playerTwoCont != null)
            {
                if (playerOneCont.GetComponent<PlayerController>().currentLives < playerOneCont.GetComponent<PlayerController>().respawnsLeft)
                {
                    Debug.Log("respawn player one!");
                    Destroy(playerOne.gameObject);
                    //playersAmount--;
                    playerOne = playerOneCont;
                    controllerOne = playerOne.GetComponent<Controller>();
                    tankOne.GetComponentInChildren<Camera>().name = tankOne.GetComponentInChildren<Camera>().name + "1";
                    cameraOne = tankOne.GetComponentInChildren<Camera>();
                    cameraOne.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1); 
                    controllerOne.respawnsLeft--;
                    if (cameraTwo != null)
                    {
                        cameraOne.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);

                        cameraTwo.GetComponent<Camera>().rect = new Rect(0.5f, 0, 1, 1);
                    }
                    if (cameraTwo == null)
                    {

                        cameraOne.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
                    }
                }
                if (playerTwoCont.GetComponent<PlayerController>().currentLives < playerTwoCont.GetComponent<PlayerController>().respawnsLeft && playerTwoCont.GetComponent<PlayerController>().respawnsLeft > 0)
                {
                    Debug.Log("respawn player two!");
                    //Destroy(playerOne.gameObject);
                    playerOne = playerTwoCont;
                    controllerOne = playerOne.GetComponent<Controller>();
                    tankOne.GetComponentInChildren<Camera>().name = tankOne.GetComponentInChildren<Camera>().name + playersAmount;
                    cameraTwo = tankOne.GetComponentInChildren<Camera>();
                    if (cameraOne != null)
                    {
                        cameraOne.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);

                        cameraTwo.GetComponent<Camera>().rect = new Rect(0.5f, 0, 1, 1);
                    }
                    if (cameraOne == null)
                    {

                        cameraTwo.GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
                    }
                    //cameraTwo.GetComponent<Camera>().rect = new Rect(0.5f, 0, 1, 1);
                    TextMeshProUGUI[] tankTextMeshArray = tankOne.GetComponentsInChildren<TextMeshProUGUI>();
                    UnityEngine.UI.Image[] panelImage = tankOne.GetComponentsInChildren<UnityEngine.UI.Image>();
                    foreach (UnityEngine.UI.Image child in panelImage)
                    {
                        if (child.name == "Panel")
                        {
                            Debug.Log(child.rectTransform.anchoredPosition);
                            child.rectTransform.anchoredPosition = new Vector2(165, 228);
                        }
                    }
                    foreach (TextMeshProUGUI child in tankTextMeshArray)
                    {
                        Debug.Log(child.name);
                        Vector2 originalTransform = child.rectTransform.anchoredPosition;
                        child.rectTransform.anchoredPosition = new Vector2(531, originalTransform.y);
                    }
                    controllerOne.respawnsLeft--;
                }
                if (playerOne == playerOneCont)
                {
                    if (playerOneCont != null && playerOneCont.GetComponent<PlayerController>().respawnsLeft == 0)
                    {
                        Destroy(tankOne.gameObject);
                    }
                }
                if (playerOne == playerTwoCont)
                {
                    if (playerTwoCont != null && playerTwoCont.GetComponent<PlayerController>().respawnsLeft == 0)
                    {
                        Destroy(tankOne.gameObject);
                    }
                }
            }
            if (playerTwoCont == null)
            {
                tankOne.GetComponentInChildren<Camera>().name = tankOne.GetComponentInChildren<Camera>().name + playersAmount;
                cameraTwo = tankOne.GetComponentInChildren<Camera>();
                Debug.Log(cameraTwo.name);
                cameraOne.GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
                cameraTwo.GetComponent<Camera>().rect = new Rect(0.5f, 0, 1, 1);

                TextMeshProUGUI[] tankTextMeshArray = tankOne.GetComponentsInChildren<TextMeshProUGUI>();
                UnityEngine.UI.Image[] panelImage = tankOne.GetComponentsInChildren<UnityEngine.UI.Image>();
                foreach (UnityEngine.UI.Image child in panelImage)
                {
                    if (child.name == "Panel")
                    {
                        Debug.Log(child.rectTransform.anchoredPosition);
                        child.rectTransform.anchoredPosition = new Vector2(165, 228);
                    }
                }
                foreach (TextMeshProUGUI child in tankTextMeshArray)
                {
                    Debug.Log(child.name);
                    Vector2 originalTransform = child.rectTransform.anchoredPosition;
                    child.rectTransform.anchoredPosition = new Vector2(531, originalTransform.y);
                }
                playerTwoCont = playerOne;
            }
        }

        controllerOne.pawn = pawnOne;
        pawnOne.controller = controllerOne;
        // Should rename the playerOne variable
        tankOne.AddComponent<NoiseMaker>();
        pawnOne.noise = tankOne.GetComponent<NoiseMaker>();
        pawnOne.noiseMakerVolume = 3;

        tankOne.AddComponent<PowerUpManager>();

        playerOne.name = "PlayerController";
        tankOne.name = "PlayerPawn";
        //playerOne.GetComponent<PlayerController>().SetScoreText();

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
        //Debug.Log("SpawnObservantAI called");
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
        //Debug.Log("SpawnSurvivorAI called");
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
        AudioSource[] sources = gameObject.GetComponentsInChildren<AudioSource>();
        foreach (AudioSource child in sources)
        {
            Debug.Log("audio source children " + child.clip.name);
            if (child.clip.name == "Epic Menu")
            {
                currentAudio = child;
                child.Play();

            }
        }
    }
    public void ActivateMainMenuScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        // Activate the main menu screen
        MainMenuStateObject.SetActive(true);
        // Do Main menu
        currentState = GameState.MainMenuState;
        AudioSource[] sources = gameObject.GetComponents<AudioSource>();
        destroyAllObjects = true;
    }
    public void ActivateOptionsScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        // Activate options screen
        OptionsScreenStateObject.SetActive(true);
        // Do Options screen
        currentState = GameState.OptionsScreenState;

        if (MapOfDay.isOn == true)
        {
            foreach (GameObject textAreaChild in TextAreaChildren)
            {
                if (textAreaChild.name == "Seed Text")
                {
                    textAreaChild.SetActive(false);
                    //SeedTwo.text = "0";
                    gameObject.GetComponent<MapGenerator>().isMapOfTheDay = true;
                }
            }
        }  
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
        //destroyAllObjects = true;
        //Debug.Log(destroyAllObjects);

        // Activate the Gameplay state
        GameplayStateObject.SetActive(true);
        // Do gameplay state
        //Debug.Log (currentAudio.name + "is the currentAudio ");
        if (currentAudio != null)
        {
            Debug.Log(currentAudio.clip.name + "is the currentAudio ");
            Debug.Log("currentAudio != null");
            if (currentAudio.clip.name == "Epic Menu")
            {
                currentAudio.Stop();
            }
            AudioSource[] sources = gameObject.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource child in sources)
            {
                Debug.Log("audio source children " + child.clip.name);
                if (child.clip.name == "Game Music")
                {
                    currentAudio = child;
                    child.Play();

                }
            }
        }

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
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;    
        Application.Quit();
    }

}
