using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    //starting with min of 5 walls per level and max of 9 walls
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for(int x = 1; x < columns - 1; x++)
        {
            for(int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for(int x = -1; x < columns + 1; x++)
        {
            for(int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                //check if we are at our boundry walls
                if(x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                //place the floor at coordinates, with no rotaion
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                //setting parent so we don't have objects in root
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    //spawn tile at position
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        //controls how many of a given object we spawn
        int objectCount = Random.Range(minimum, maximum + 1);
        //spawn # of objects
        for(int i = 0; i < objectCount; i++)
        {
            //choose random position
            Vector3 randomPosition = RandomPosition();
            //choose random tile
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            //put tile at position
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }

    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        //scale difficulty by level
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
