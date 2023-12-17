using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static GameManager;

public class GamePlayState : MonoBehaviour
{
    private bool isPaused;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    public void Start()
    {
        isPaused = false;
        if (GameManager.instance.currentState != GameManager.GameState.GameplayState && GameManager.instance.destroyAllObjects == true)
        {
            Camera tempCamera = FindObjectOfType<Camera>();
            Debug.Log(tempCamera.name);
            if (tempCamera.name == "Camera for display renderer")
            {
                //tempCamera = Camera.main;
                Destroy(tempCamera);
            }
            Debug.Log("calling foreach loop for destroying objects. ");
            foreach (GameObject Obj in GameManager.instance.allObjects)
            {
                //Debug.Log("Objects in allObjects according to GamePlayState : " + Obj.name);
                Destroy(Obj);
                GameManager.instance.allObjects.Remove(Obj);
            }
            //Debug.Log("Objects in allObjects : " + GameManager.instance.allObjects.Count);
            GameManager.instance.destroyAllObjects = false;
            GameManager.instance.mapGenerator.GenerateMap();
            GameManager.instance.currentState = GameManager.GameState.GameplayState;

            if (GameManager.instance.currentState == GameState.GameplayState && GameManager.instance.destroyAllObjects == false)
            {
                GameManager.instance.spawnPoints = FindObjectsOfType<PawnSpawnPoint>();
                /*       foreach (PawnSpawnPoint spawnP in GameManager.instance.spawnPoints)
                       {
                           Debug.Log(spawnP.gameObject.name);
                       }  */
                //Debug.Log(GameManager.instance.spawnPoints.Length);

                if (GameManager.instance.currentState == GameState.GameplayState)
                {
                    if (GameManager.instance.playersAmount <= 0)
                    {
                        GameManager.instance.playersAmount++;
                        GameManager.instance.SpawnPlayer(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                        if (GameManager.instance.isMultiplayer == true)
                        {
                            GameManager.instance.playersAmount++;
                            GameManager.instance.SpawnPlayer(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                        }
                    }
                }
                GameManager.instance.SpawnAggressiveAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                GameManager.instance.SpawnCowardlyAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                GameManager.instance.SpawnObservantAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                GameManager.instance.SpawnSurvivorAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        AIController.AIState stateHolder;
        stateHolder = AIController.AIState.Patrol;
        if (GameManager.instance.OptionsScreenStateObject.activeInHierarchy == true)
        {
            Debug.Log("paused");
            isPaused = true;
            foreach (AIController ai in GameManager.instance.aiList)
            {
                Debug.Log(ai.gameObject.name);
                Debug.Log("setting ai state");
                stateHolder = ai.currentState;
                ai.currentState = AIController.AIState.Idle;
            }
        }
        if (GameManager.instance.OptionsScreenStateObject.activeInHierarchy == false)
        {
            //Debug.Log("playing");
            if (isPaused == true)
            {
                foreach (AIController ai in GameManager.instance.aiList)
                {
                    ai.currentState = stateHolder;
                }
            }
            isPaused = false;
        }

        /*
        if (GameManager.instance.currentState != GameManager.GameState.GameplayState && GameManager.instance.destroyAllObjects == false)
        {
            GameManager.instance.mapGenerator.GenerateMap();
        } */
        // ^^ not sure why this is here. triple check this
    }
}
   