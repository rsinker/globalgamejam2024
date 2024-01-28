using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] public int m_MaxHealth = 6;
    [SerializeField] public int m_CurrentHealth = 6;
    [SerializeField] public int m_Rats = 0;
    [SerializeField] public float m_DamageModifier = 1f;
    public bool _isDead => m_CurrentHealth <= 0;
    //[SerializeField] bool isDead = false; //mystuff

    [Header("References")]
    private GameManager m_GameManager;
    private AudioManager m_AudioManager;
    private PlayerController m_PlayerController;
    private DamageFlash m_PlayerDamageFlash;
    private PlayerManager m_PlayerManager;
    [SerializeField] Animator m_Animator; 
    private UIManager_Main m_UIManager;

    [Header("Sound Effects")]
    [SerializeField] private string s_playerHurt;
    [SerializeField] private string s_playerDeath;

    public void Start()
    {
        m_GameManager = GameManager.Instance;
        m_AudioManager = AudioManager.Instance;
        m_PlayerManager = PlayerManager.Instance;
        m_UIManager = Object.FindObjectOfType<UIManager_Main>();

        m_PlayerController = m_PlayerManager._playerController;
        m_PlayerDamageFlash = m_PlayerManager._playerDamageFlash;

        m_UIManager.UpdateHearts(m_CurrentHealth, m_MaxHealth);
    }
    public void RecieveDamage(float damage)
    {
        m_Animator.SetBool("isDead", _isDead); 

        if (m_PlayerController.isDashing) return;
        m_CurrentHealth -= (int)damage;
        if (m_CurrentHealth < 0)
        {
            m_PlayerController.enabled = false;
            m_AudioManager.PlaySoundOnce(s_playerDeath);
        }
        else
        {
            m_Animator.SetTrigger("gotHit"); 
            m_AudioManager.PlaySoundOnce(s_playerHurt);
            m_PlayerDamageFlash.CallDamageFlash();
           
        }
        Debug.Log(m_CurrentHealth + " vs " + m_MaxHealth);
        m_UIManager.UpdateHearts(m_CurrentHealth, m_MaxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Projectile"))
        {

        }
    }

    public void Update() //my stuff
    { 
        if (Input.GetKeyDown(KeyCode.T)) //my stuff
        { //my stuff
            RecieveDamage(6); //my stuff
        } //my stuff
    } 

}

