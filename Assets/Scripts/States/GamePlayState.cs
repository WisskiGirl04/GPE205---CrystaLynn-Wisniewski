using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using static GameManager;

public class GamePlayState : MonoBehaviour
{
    public bool isPaused;
    public AIController.AIState stateHolder;

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

            if (GameManager.instance.currentState == GameState.GameplayState && GameManager.instance.destroyAllObjects == false )
            {
                GameManager.instance.spawnPoints = FindObjectsOfType<PawnSpawnPoint>();
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
        if (GameManager.instance.destroyAllObjects == true)
        {
            if (GameManager.instance.isMultiplayer == false)
            {
                if (GameManager.instance.playerOneCont.gameObject.activeInHierarchy == true)
                {
                    GameManager.instance.playerOneCont.GetComponent<PlayerController>().score = 0;
                    GameManager.instance.playerOneCont.GetComponent<PlayerController>().scoreText.text = GameManager.instance.playerOneCont.GetComponent<PlayerController>().score.ToString();
                    GameManager.instance.playerOneCont.GetComponent<PlayerController>().currentLives = GameManager.instance.startingLives;
                    GameManager.instance.playerOneCont.GetComponent<PlayerController>().livesText.text = GameManager.instance.playerOneCont.GetComponent<PlayerController>().currentLives.ToString();
                }
            }
            if (GameManager.instance.isMultiplayer == true)
            {
                if (GameManager.instance.playerOneCont.gameObject.activeInHierarchy == true)
                {
                    GameManager.instance.playerOneCont.GetComponent<PlayerController>().score = 0;
                    GameManager.instance.playerOneCont.GetComponent<PlayerController>().scoreText.text = GameManager.instance.playerOneCont.GetComponent<PlayerController>().score.ToString();
                    GameManager.instance.playerOneCont.GetComponent<PlayerController>().currentLives = GameManager.instance.startingLives;
                    GameManager.instance.playerOneCont.GetComponent<PlayerController>().livesText.text = GameManager.instance.playerOneCont.GetComponent<PlayerController>().currentLives.ToString();
                }
                if (GameManager.instance.playerTwoCont.gameObject.activeInHierarchy == true)
                {
                    GameManager.instance.playerTwoCont.GetComponent<PlayerController>().score = 0;
                    GameManager.instance.playerTwoCont.GetComponent<PlayerController>().scoreText.text = GameManager.instance.playerTwoCont.GetComponent<PlayerController>().score.ToString();
                    GameManager.instance.playerTwoCont.GetComponent<PlayerController>().currentLives = GameManager.instance.startingLives;
                    GameManager.instance.playerTwoCont.GetComponent<PlayerController>().livesText.text = GameManager.instance.playerTwoCont.GetComponent<PlayerController>().currentLives.ToString();
                }
                GameManager.instance.destroyAllObjects = false;
            }
        }
        
    }
}
   