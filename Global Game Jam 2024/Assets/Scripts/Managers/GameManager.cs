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
    public static bool leftDoorLocked = true;
    public static bool rightDoorLocked = true;
    public static bool topDoorLocked = true;
    public static bool bottomDoorLocked = true;


    public static bool dialogueRunning = false;


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

    public static void ResetRoomLock()
    {
        leftDoorLocked = true;
        rightDoorLocked = true;
        topDoorLocked = true;
        bottomDoorLocked = true;
    }
}

