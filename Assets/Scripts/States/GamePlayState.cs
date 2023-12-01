using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static GameManager;

public class GamePlayState : MonoBehaviour
{
    private void Awake()
    {

    }
    // Start is called before the first frame update
    public void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentState != GameManager.GameState.GameplayState && GameManager.instance.destroyAllObjects == true)
        {
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
            
            if (GameManager.instance.currentState == GameState.GameplayState)
            {
                GameManager.instance.spawnPoints = FindObjectsOfType<PawnSpawnPoint>();
         /*       foreach (PawnSpawnPoint spawnP in GameManager.instance.spawnPoints)
                {
                    Debug.Log(spawnP.gameObject.name);
                }  */
                Debug.Log(GameManager.instance.spawnPoints.Length);

                if (GameManager.instance.currentState == GameState.GameplayState)
                {
                    if (GameManager.instance.playersAmount <= 0)
                    {
                        GameManager.instance.SpawnPlayer(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                    }
                }
                GameManager.instance.SpawnAggressiveAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                GameManager.instance.SpawnCowardlyAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                GameManager.instance.SpawnObservantAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
                GameManager.instance.SpawnSurvivorAI(GameManager.instance.spawnPoints[Random.Range(0, GameManager.instance.spawnPoints.Length)]);
            }
        }
        /*
        if (GameManager.instance.currentState != GameManager.GameState.GameplayState && GameManager.instance.destroyAllObjects == false)
        {
            GameManager.instance.mapGenerator.GenerateMap();
        } */
        // ^^ not sure why this is here. triple check this
    }
}
   