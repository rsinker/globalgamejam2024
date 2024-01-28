using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int m_MaxHealth = 6;
    [SerializeField] private int m_CurrentHealth = 6;
    [SerializeField] private int m_Rats = 0;

    [Header("References")]
    private GameManager m_GameManager;
    private AudioManager m_AudioManager;
    private PlayerController m_PlayerController;
    private DamageFlash m_PlayerDamageFlash;
    private PlayerManager m_PlayerManager;
    
    [Header("Sound Effects")]
    [SerializeField] private string s_playerHurt;
    [SerializeField] private string s_playerDeath;

    public void Start()
    {
        m_GameManager = GameManager.Instance;
        m_AudioManager = AudioManager.Instance;
        m_PlayerManager = PlayerManager.Instance;

        m_PlayerController = m_PlayerManager._playerController;
        m_PlayerDamageFlash = m_PlayerManager._playerDamageFlash;
    }
    public void RecieveDamage(float damage)
    {
        if (m_PlayerController.isDashing) return;
        m_CurrentHealth -= (int)damage;
        if (m_CurrentHealth < 0)
        {
            m_AudioManager.PlaySoundOnce(s_playerDeath);
        }
        else
        {
            m_AudioManager.PlaySoundOnce(s_playerHurt);
            m_PlayerDamageFlash.CallDamageFlash();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Projectile"))
        {

        }
    }

}

