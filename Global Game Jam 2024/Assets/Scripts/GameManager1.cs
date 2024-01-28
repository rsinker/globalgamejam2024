using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*public enum GameState
{
    MainMenu,
    InGame,
    OutGame
}*/

public class GameManager1 : MonoBehaviour
{
    public int playerMaxHealth;
    public int playerCurrentHealth;

    public static GameManager1 Instance;
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
}

