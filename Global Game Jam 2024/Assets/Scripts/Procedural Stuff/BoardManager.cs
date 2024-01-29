using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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
    [HideInInspector] public Room currentRoom;
    Room[,] gameMap = new Room[ProceduralConstants.MAX_MAP_HORIZ, ProceduralConstants.MAX_MAP_VERT];
    public static Vector2Int mapCoordinates = new Vector2Int(ProceduralConstants.MAX_MAP_HORIZ / 2, -1);


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

    public Tilemap floorMap;
    public Tilemap wallMap;


    private Transform boardHolder;
    private Transform doorHolder;
    public static Transform foodHolder;
    public Transform foodHolderObj;
    private List<Vector3> gridPositions = new List<Vector3>();

    private int roomCountShop = 1;
    private float chanceShop = ProceduralConstants.BEGIINING_CHANCE_OF_SHOP;
    
    public GameObject shopObj;
    [SerializeField] private GameObject shopUpDoor;
    [SerializeField] private GameObject shopDownDoor;
    [SerializeField] private GameObject shopLeftDoor;
    [SerializeField] private GameObject shopRightDoor;

    private int roomCountBoss = 1;
    private float chanceBoss = ProceduralConstants.BEGIINING_CHANCE_OF_SHOP;

    public GameObject bossObj;
    [SerializeField] private GameObject bossUpDoor;
    [SerializeField] private GameObject bossDownDoor;
    [SerializeField] private GameObject bossLeftDoor;
    [SerializeField] private GameObject bossRightDoor;

    public GameObject hubObj;

    [SerializeField] private GameObject blender;
    [SerializeField] private GameObject oven;
    [SerializeField] private GameObject fryer;
    [SerializeField] private GameObject plate;

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private recipeManager recipeGenerator;


    GameObject[] currDoors = new GameObject[4];

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
        doorHolder = new GameObject("DoorHolderObj").transform;
        recipeGenerator.PickNewRecipe();
        if (gameMap[mapCoordinates.x, mapCoordinates.y] == null)
        {
            bool spawningShop = CheckIfShop();
            bool spawningBoss = CheckIfBoss();
            if (spawningBoss == true)
            {
                //Debug.Log("Spawn Boss");
                currentRoom = new Room(ProceduralConstants.FAKE_BOSS_SIZE, ProceduralConstants.FAKE_BOSS_SIZE);
                GameManager.state = GameState.InBossArea;
            }
            else if (spawningShop == false) 
            {
                ToggleDoors(false);
                currentRoom = new Room(collumns, rows);
                currentRoom.CarveRoom(minSplats, maxSplats, minSplatSize, maxSplatSize, rows, collumns);
                currentRoom.GenerateDoors(rows, collumns);
                currentRoom.GenerateCookware();
                //gameMap[mapCoordinates.x, mapCoordinates.y] = currentRoom;
                roomCountShop++;
                roomCountBoss++;

                GameManager.state = GameState.InCombatArea;
                recipeGenerator.PickNewRecipe();

                InvokeRepeating("BeginSpawn", 0.0f, GameConstants.SPAWN_DELAY);
            }
            else
            {
                //Debug.Log("Spawn Shop 2");
                currentRoom = new Room(ProceduralConstants.FAKE_SHOP_SIZE, ProceduralConstants.FAKE_SHOP_SIZE);
                //gameMap[mapCoordinates.x, mapCoordinates.y] = currentRoom;
                GameManager.state = GameState.InPassiveArea;
            }

            gameMap[mapCoordinates.x, mapCoordinates.y] = currentRoom;
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
        if (currentRoom.isShop)
        {
            //Debug.Log("Spawn Shop 3");
            MakeShop();
            return;
        }
        else if (currentRoom.isBoss)
        {
            MakeBoss();
            return;
        }

        for (int x = 0; x < collumns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                //GameObject toInstantiate;

                if (currentRoom.GetTile(x, y) == "floor")
                {
                    floorMap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
                    //toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                }
                else if (currentRoom.GetTile(x, y) == "obstacle")
                {
                    wallMap.SetTile(new Vector3Int(x, y, 0), wallTiles[Random.Range(0, wallTiles.Length)]);
                    //toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                }
                else if (currentRoom.GetTile(x, y) == "upperDoor")
                {
                    GameObject instance = Instantiate(upperDoor, new Vector3(x+0.5f, y+0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(doorHolder);
                    floorMap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
                    //floorMap.SetTile(new Vector3Int(x, y, 0), upperDoor);
                    currDoors[0] = instance;
                }
                else if (currentRoom.GetTile(x, y) == "leftDoor")
                {
                    GameObject instance = Instantiate(leftDoor, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(doorHolder);
                    floorMap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
                    currDoors[1] = instance;
                }
                else if (currentRoom.GetTile(x, y) == "rightDoor")
                {
                    GameObject instance = Instantiate(rightDoor, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(doorHolder);
                    floorMap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
                    currDoors[2] = instance;
                }
                else if (currentRoom.GetTile(x, y) == "bottomDoor")
                {
                    GameObject instance = Instantiate(bottomDoor, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(doorHolder);
                    floorMap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
                    currDoors[3] = instance;
                }
                else if (currentRoom.GetTile(x, y) == "blender")
                {
                    GameObject instance = Instantiate(blender, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                    floorMap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
                }
                else if (currentRoom.GetTile(x, y) == "oven")
                {
                    GameObject instance = Instantiate(oven, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                    floorMap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
                }
                else if (currentRoom.GetTile(x, y) == "fryer")
                {
                    GameObject instance = Instantiate(fryer, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                    floorMap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
                }
                else if (currentRoom.GetTile(x, y) == "plate")
                {
                    GameObject instance = Instantiate(plate, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity) as GameObject;
                    instance.transform.SetParent(boardHolder);
                    floorMap.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length)]);
                }
                else
                {
                    wallMap.SetTile(new Vector3Int(x, y, 0), outerWallTiles[Random.Range(0, outerWallTiles.Length)]);
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
        ToggleDoors(false);
        //Debug.Log("Leaving room from: " + direction.ToString());
        Destroy(boardHolder.gameObject);
        Destroy(foodHolder.gameObject);
        if (doorHolder != null)
        {
            Destroy(doorHolder.gameObject);
        }
        floorMap.ClearAllTiles();
        wallMap.ClearAllTiles();

        enemySpawner.ResetEnemies();

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

        if (currentRoom.isShop == true)
        {
            shopObj.SetActive(false);
            shopUpDoor.SetActive(true);
            shopDownDoor.SetActive(true);
            shopLeftDoor.SetActive(true);
            shopRightDoor.SetActive(true);
        }
        else if (currentRoom.isBoss == true)
        {
            bossObj.SetActive(false);
            bossUpDoor.SetActive(true);
            bossDownDoor.SetActive(true);
            bossLeftDoor.SetActive(true);
            bossRightDoor.SetActive(true);
        }
        else if (currentRoom.isHub == true)
        {
            hubObj.SetActive(false);
        }

        

        SetupScene();
        SpawnPlayer(direction);
        GameManager.roomCount++;
        GameManager.enemiesKilledInRoom = 0;

        foodHolder = new GameObject("FoodParentObj").transform;

        //Debug.Log("Load: " + mapCoordinates);
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

        if (currentRoom.isShop == false && currentRoom.isBoss == false)
        {
            if (currentRoom.GetTile(new Vector2Int((int)spawnPoint.x, (int)spawnPoint.y)) != "floor")
            {
                //Debug.Log("FIND NEW SPAWN POINT");
                spawnPoint = currentRoom.GetValidFloatPosition(spawnPoint);
            }
        }

        
        playerRef.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, 0);
    }

    bool CheckIfShop()
    {
        if (roomCountShop > ProceduralConstants.MIN_ROOMS_BEFORE_SHOP)
        {
            float randomizer = Random.Range(0f, 1f);
            if (randomizer < chanceShop)
            {
                //Debug.Log("Spawn Shop");
                chanceShop = ProceduralConstants.BEGIINING_CHANCE_OF_SHOP;
                roomCountShop = 1;
                return true;
            }
            else
            {
                //Debug.Log(randomizer + ": No Shop from chance: " + chanceShop);
                chanceShop += ProceduralConstants.CHANCE_OF_SHOP_INCREMENT;
                return false;

            }
        }

        return false;
    }

    void MakeShop()
    {
        //Debug.Log("Spawn Shop 4");
        shopObj.SetActive(true);

        if (mapCoordinates.y >= ProceduralConstants.MAX_MAP_VERT - 1)
        {
            //Disable upDoor
            shopUpDoor.SetActive(false);
        }

        if (mapCoordinates.y <= 0)
        {
            //disable downDoor
            shopDownDoor.SetActive(false);
        }

        if (mapCoordinates.x >= ProceduralConstants.MAX_MAP_HORIZ - 1)
        {
            //disable rightDoor
            shopRightDoor.SetActive(false);
        }

        if (mapCoordinates.x <= 0)
        {
            //disable leftDoor
            shopLeftDoor.SetActive(false);
        }

        //GameManager.topDoorLocked = false;
        //GameManager.bottomDoorLocked = false;
        //GameManager.leftDoorLocked = false;
        //GameManager.rightDoorLocked = false;
    }

    void MakeBoss()
    {
        bossObj.SetActive(true);

        if (mapCoordinates.y >= ProceduralConstants.MAX_MAP_VERT - 1)
        {
            //Disable upDoor
            bossUpDoor.SetActive(false);
        }

        if (mapCoordinates.y <= 0)
        {
            //disable downDoor
            bossDownDoor.SetActive(false);
        }

        if (mapCoordinates.x >= ProceduralConstants.MAX_MAP_HORIZ - 1)
        {
            //disable rightDoor
            bossRightDoor.SetActive(false);
        }

        if (mapCoordinates.x <= 0)
        {
            //disable leftDoor
            bossLeftDoor.SetActive(false);
        }

        //GameManager.topDoorLocked = false;
        //GameManager.bottomDoorLocked = false;
        //GameManager.leftDoorLocked = false;
        //GameManager.rightDoorLocked = false;
    }

    void MakeHub()
    {
        hubObj.SetActive(true);

        boardHolder = new GameObject("BoardParentObj").transform;
        currentRoom = new Room(ProceduralConstants.FAKE_HUB_SIZE, ProceduralConstants.FAKE_HUB_SIZE);

        //GameManager.topDoorLocked = false;
        //GameManager.bottomDoorLocked = false;
        //GameManager.leftDoorLocked = false;
        //GameManager.rightDoorLocked = false;
    }

    bool CheckIfBoss()
    {
        if (roomCountBoss > ProceduralConstants.MIN_ROOMS_BEFORE_BOSS)
        {
            float randomizer = Random.Range(0f, 1f);
            if (randomizer < chanceBoss)
            {
                //Debug.Log("Spawn Shop");
                chanceBoss = ProceduralConstants.BEGINING_CHANCE_OF_BOSS;
                roomCountBoss = 1;
                return true;
            }
            else
            {
                //Debug.Log(randomizer + ": No Boss from chance: " + chanceBoss);
                chanceBoss += ProceduralConstants.CHANCE_OF_BOSS_INCREMENT;
                return false;

            }
        }

        return false;
    }

    void BeginSpawn()
    {
        enemySpawner.SpawnEnemies();
    }

    void ToggleDoors(bool toggle)
    {
        if (doorHolder != null)
        {
            doorHolder.gameObject.SetActive(toggle);
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        foodHolder = new GameObject("FoodParentObj").transform;
        playerRef = Object.FindObjectOfType<PlayerController>().gameObject;
        //SetupScene();
        MakeHub();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.enemiesKilledInRoom > 5)
        {
            ToggleDoors(true);
        }
    }

    public void ClearAll()
    {
        Destroy(foodHolder.gameObject);
        enemySpawner.ResetEnemies();
    }
}
