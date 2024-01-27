using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Count
{
    public int minimum;
    public int maximum;
    public Count(int min, int max)
    {
        minimum = min;
        maximum = max;
    }
}

public class BoardManager : MonoBehaviour
{


    [SerializeField] int collumns = 8;
    [SerializeField] int rows = 8;
    //[SerializeField] int buffer = 3;
    [SerializeField] int minSplats = 1;
    [SerializeField] int maxSplats = 3;
    [SerializeField] int minSplatSize = 4;
    [SerializeField] int maxSplatSize = 8;


    //[SerializeField] float bufferChanceWall;

    //[SerializeField] float perlinResolution;

    //Change to enum
    string[,] boardData;


    Count wallCount = new Count(5, 9);
    //int Count foodCount(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    //public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 1; x < collumns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    /*void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < collumns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == collumns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                else if (x < buffer || collumns - x < buffer)
                {
                    float perlinOffset = Random.Range(0.0f, 100.0f);
                    /*Debug.Log(bufferChanceWall + " vs " + Mathf.PerlinNoise(((float)x + perlinOffset) / perlinResolution, ((float)y) + perlinOffset)/perlinResolution);
                    if (bufferChanceWall > Mathf.PerlinNoise(((float)x + perlinOffset) / perlinResolution, ((float)y) + perlinOffset) / perlinResolution)
                    {
                        toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    }

                   
                }
                else
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }*/

    void BoardSetup()
    {
        boardHolder = new GameObject("BoardParentObj").transform;
        boardData = new string[collumns, rows];

        for (int x = 0; x < collumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                boardData[x,y] = "wall";
            }
        }
        
        // Make some kind of raised area
        int numSplats = Random.Range(minSplats, maxSplats);
        for (int i = 0; i < numSplats; i++)
        {
            int range = Random.Range(minSplatSize, maxSplatSize);
            int y = Random.Range(range, rows - range);
            int x = Random.Range(range, collumns - range);

            Carve(x, y, range);
        }

        MakeBoard();
    }

    void Carve(int x, int y, int range)
    {
        for (int dx = -range; dx < range-1; dx++)
        {
            for (int dy = -range; dy < range - 1; dy++)
            {
                if ((dx-range)*(dx-range) + (dy-range)*(dy-range) <= range*range)
                {
                    boardData[x + dx, y + dy] = "floor";
                }
            }

            /*int StartY = (int)(y - Mathf.Sqrt(Mathf.Abs((range + dx - (-range) * (range - dx + (-range))))));
            int EndY = (int)(y + Mathf.Sqrt(Mathf.Abs((range + dx - (-range) * (range - dx + (-range))))));
            for (int dy = StartY; dy < EndY; dy++)
            {
                boardData[x + dx, dy] = "floor";
            }*/
        }
    }

    void MakeBoard()
    {
        for (int x = 0; x < collumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject toInstantiate;

                if (boardData[x,y] == "floor")
                {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }
                else
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
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

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate (tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        //int enemyCount = (int)Mathf.Log(level, 2f);
        //LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(collumns-1, rows-1, 0f), Quaternion.identity);
    }


    // Start is called before the first frame update
    void Start()
    {
        SetupScene(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
