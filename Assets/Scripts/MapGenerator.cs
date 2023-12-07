using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] gridPrefabs;
    public int rows;
    public int collums;
    public float roomWidth = 50.0f;
    public float roomHeight = 50.0f;
    private Room[,] grid;
    public int mapSeed;
    public bool isMapOfTheDay;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Returns a random room
    public GameObject RandomRoomPrefab()
    {
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }
    
    public void GenerateMap()
    {
        if (isMapOfTheDay)
        {
            mapSeed = DateToInt(DateTime.Now.Date);
            Debug.Log("Date to int is.. " + DateTime.Now.Date);
            Debug.Log("DateTime.Now is.. " + DateTime.Now);
            Debug.Log("is map of the day.. " + mapSeed);
        }
        if (isMapOfTheDay == false)
        {
            // Set our map seed
            if (mapSeed == 0)
            {
                //UnityEngine.Random.InitState(mapSeed);
#pragma warning disable CS0618 // Type or member is obsolete
                mapSeed = UnityEngine.Random.seed;
#pragma warning restore CS0618 // Type or member is obsolete
                Debug.Log("map seed is... " + mapSeed);
            }
            else
            {
                //UnityEngine.Random.InitState(mapSeed);
                Debug.Log("map seed is... " + mapSeed);
            }
        }
        // Set our map seed
        UnityEngine.Random.InitState(mapSeed);
        // Clear out the grid - "column" is our X, "row" is our Y
        grid = new Room[collums, rows];

        // For each grid row...
        for (int currentRow = 0; currentRow < rows; currentRow++)
        {
            // for each column in that row
            for (int currentCol = 0; currentCol < collums; currentCol++)
            {
                // Figure out the location and save that location in new local variables 
                float xPosition = roomWidth * currentCol;
                float zPosition = roomHeight * currentRow;
#pragma warning disable IDE0090 // Use 'new(...)'
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);
#pragma warning restore IDE0090 // Use 'new(...)'

                // Create/spawn a new grid/tile at the appropriate location
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                // Set its parent
                tempRoomObj.transform.parent = this.transform;

                // Give it a meaningful name
                tempRoomObj.name = "Room_" + currentCol + "," + currentRow;

                // Get the room object
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                // Open the doors
                //Rows
                // If we are on the bottom row, open the north door
                if (currentRow == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (currentRow == rows - 1)
                {
                    // Otherwise, if we are on the top row, open the south door
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    // Otherwise, we are in the middle, so open both doors
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }
                //Collums
                if(currentCol == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (currentCol == collums - 1)
                {
                    tempRoom.doorWest.SetActive(false);
                }
                else
                {
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }


                // Save it to the grid array
                grid[currentCol, currentRow] = tempRoom;
            }
        }    
    }
    public int DateToInt(DateTime dateToUse)
    {
        // Add our date up and return it
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }
}
