using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    Room currentRoom;


    //string[,] boardData;


    Count wallCount = new Count(5, 9);
    //int Count foodCount(1, 5);
    public GameObject exit;
    public Tile[] floorTiles;
    public Tile[] wallTiles;
    //public GameObject[] enemyTiles;
    public Tile[] outerWallTiles;

    public GameObject upperDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;

    public Tilemap tilemap;
    

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
        currentRoom = new Room(collumns, rows);

        //Make some kind of raised area
        //int numSplats = Random.Range(minSplats, maxSplats);
        

        currentRoom.CarveRoom(minSplats, maxSplats, minSplatSize, maxSplatSize, rows, collumns);
        currentRoom.GenerateDoors(rows, collumns);

        MakeBoard();
    }

    void MakeBoard()
    {
        for (int x = 0; x < collumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                //GameObject toInstantiate;

                if (currentRoom.GetTile(x, y) == "floor")
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
                    //toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }
                else if (currentRoom.GetTile(x, y) == "obstacle")
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), wallTiles[Random.Range(0, wallTiles.Length)]);
                    //toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                }
                else if (currentRoom.GetTile(x, y) == "upperDoor")
                {
                    GameObject instance = Instantiate(upperDoor, new Vector3(x+0.5f, y+0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                    //tilemap.SetTile(new Vector3Int(x, y, 0), upperDoor);
                }
                else if (currentRoom.GetTile(x, y) == "leftDoor")
                {
                    GameObject instance = Instantiate(leftDoor, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                }
                else if (currentRoom.GetTile(x, y) == "rightDoor")
                {
                    GameObject instance = Instantiate(rightDoor, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), outerWallTiles[Random.Range(0, outerWallTiles.Length)]);
                    //toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                //GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                //instance.transform.SetParent(boardHolder);
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
        //LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        //int enemyCount = (int)Mathf.Log(level, 2f);
        //LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(collumns-1, rows-1, 0f), Quaternion.identity);
    }

    [ContextMenu("Paint")]
    void Paint()
    {

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
