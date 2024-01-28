using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    Room[,] gameMap = new Room[ProceduralConstants.MAX_MAP_HORIZ, ProceduralConstants.MAX_MAP_VERT];
    public static Vector2Int mapCoordinates = new Vector2Int(ProceduralConstants.MAX_MAP_HORIZ / 2, ProceduralConstants.MAX_MAP_VERT / 2);


    //string[,] boardData;

    private GameObject playerRef;


    Count wallCount = new Count(5, 9);
    //int Count foodCount(1, 5);
    //public GameObject exit;
    public Tile[] floorTiles;
    public Tile[] wallTiles;
    //public GameObject[] enemyTiles;
    public Tile[] outerWallTiles;

    public GameObject upperDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject bottomDoor;

    public Tilemap tilemap;
    

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    private int roomsVisited = 1;
    private float chanceShop = ProceduralConstants.BEGIINING_CHANCE_OF_SHOP;

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
        if (gameMap[mapCoordinates.x, mapCoordinates.y] == null)
        {
            bool spawningShop = CheckIfShop();
            if (spawningShop == false) 
            {
                currentRoom = new Room(collumns, rows);
                currentRoom.CarveRoom(minSplats, maxSplats, minSplatSize, maxSplatSize, rows, collumns);
                currentRoom.GenerateDoors(rows, collumns);
                gameMap[mapCoordinates.x, mapCoordinates.y] = currentRoom;
                roomsVisited++;
            }
            else
            {
                Debug.Log("MAKE SHOP");
            }
            


            
            
        }
        else
        {
            currentRoom = gameMap[mapCoordinates.x, mapCoordinates.y];
        }

        //Debug.Log("Creating map for: " + mapCoordinates);

        //Make some kind of raised area
        //int numSplats = Random.Range(minSplats, maxSplats);

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
                else if (currentRoom.GetTile(x, y) == "bottomDoor")
                {
                    GameObject instance = Instantiate(bottomDoor, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
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

    public void SetupScene()
    {
        BoardSetup();
        InitializeList();
        //LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        //int enemyCount = (int)Mathf.Log(level, 2f);
        //LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        //Instantiate(exit, new Vector3(collumns-1, rows-1, 0f), Quaternion.identity);
    }

    public void MoveToNewRoom(Door.Direction direction)
    {
        //Debug.Log("Leaving room from: " + direction.ToString());
        Destroy(boardHolder.gameObject);
        tilemap.ClearAllTiles();

        if (direction == Door.Direction.left)
        {
            mapCoordinates.x--;
        }
        else if (direction == Door.Direction.right)
        {
            mapCoordinates.x++;
        }
        else if (direction == Door.Direction.up)
        {
            mapCoordinates.y++;
        }
        else if (direction == Door.Direction.down)
        {
            mapCoordinates.y--;
        }

        

        SetupScene();
        SpawnPlayer(direction);
    }

    void SpawnPlayer(Door.Direction direction)
    {
        Vector2 spawnPoint = Vector2.zero;
        if (direction == Door.Direction.left)
        {
            spawnPoint = new Vector2(currentRoom.rightDoorPos.x - GameConstants.PLAYER_SPAWN_OFFSET, currentRoom.rightDoorPos.y);
            /*if (currentRoom.GetTile(new Vector2Int((int)spawnPoint.x, (int)spawnPoint.y)) != "floor")
            {
                Debug.Log("FIND NEW SPAWN POINT");
                spawnPoint = currentRoom.GetValidFloatPosition(spawnPoint);
            }
            playerRef.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);*/
        }
        else if (direction == Door.Direction.right)
        {
            spawnPoint = new Vector2(currentRoom.leftDoorPos.x + GameConstants.PLAYER_SPAWN_OFFSET, currentRoom.leftDoorPos.y);
            
        }
        else if (direction == Door.Direction.up)
        {
            spawnPoint = new Vector2(currentRoom.bottomDoorPos.x, currentRoom.bottomDoorPos.y + GameConstants.PLAYER_SPAWN_OFFSET);
            //playerRef.transform.position = new Vector3(currentRoom.bottomDoorPos.x, currentRoom.bottomDoorPos.y + GameConstants.PLAYER_SPAWN_OFFSET, 0);
        }
        else if (direction == Door.Direction.down)
        {
            spawnPoint = new Vector2(currentRoom.topDoorPos.x, currentRoom.topDoorPos.y - GameConstants.PLAYER_SPAWN_OFFSET);
            //playerRef.transform.position = new Vector3(currentRoom.topDoorPos.x, currentRoom.topDoorPos.y - GameConstants.PLAYER_SPAWN_OFFSET, 0);
        }

        if (currentRoom.GetTile(new Vector2Int((int)spawnPoint.x, (int)spawnPoint.y)) != "floor")
        {
            Debug.Log("FIND NEW SPAWN POINT");
            spawnPoint = currentRoom.GetValidFloatPosition(spawnPoint);
        }
        playerRef.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
    }

    bool CheckIfShop()
    {
        if (roomsVisited > ProceduralConstants.MIN_ROOMS_BEFORE_SHOP)
        {
            float randomizer = Random.Range(0f, 1f);
            if (randomizer < chanceShop)
            {
                Debug.Log("Spawn Shop");
                chanceShop = ProceduralConstants.BEGIINING_CHANCE_OF_SHOP;
                roomsVisited = 1;
                return true;
            }
            else
            {
                Debug.Log(randomizer + ": No Shop from chance: " + chanceShop);
                chanceShop += ProceduralConstants.CHANCE_OF_SHOP_INCREMENT;
                return false;

            }
        }

        return false;
    }


    // Start is called before the first frame update
    void Awake()
    {
        playerRef = Object.FindObjectOfType<PlayerController>().gameObject;
        SetupScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
