using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerController _playerController;
    public PlayerStats _playerStats;
    public DamageFlash _playerDamageFlash;
    public static PlayerManager Instance { get; private set; }
    public AudioSource gameOst;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        gameOst.Play();
    }
}
