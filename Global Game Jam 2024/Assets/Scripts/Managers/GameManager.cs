using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameState
{
    MainMenu,
    InCombatArea,
    InPassiveArea,
    InBossArea,
    OutGame
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState state = GameState.MainMenu;
    //public static string currentRecipe;
    public static bool leftDoorLocked = false;
    public static bool rightDoorLocked = false;
    public static bool topDoorLocked = false;
    public static bool bottomDoorLocked = false;


    public static bool dialogueRunning = false;

    public static int roomCount = 0;
    public const float ROOMS_TO_DIFFICULTY = 0.2f;

    public static int enemiesKilledInRoom = 0;


    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        Debug.Log("killed in room: " + enemiesKilledInRoom);
        if (enemiesKilledInRoom >= 5)
        {
            UnlockDoors();
        }
    }

    public static void ResetRoomLock()
    {
        enemiesKilledInRoom = 0;
        leftDoorLocked = true;
        rightDoorLocked = true;
        topDoorLocked = true;
        bottomDoorLocked = true;
    }

    public static void UnlockDoors()
    {
        leftDoorLocked = false;
        rightDoorLocked = false;
        topDoorLocked = false;
        bottomDoorLocked = false;
    }
}

