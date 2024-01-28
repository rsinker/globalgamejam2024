using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] float maxHealth = 10f;
    [SerializeField] private float damage = 1f;

    [SerializeField] private bool isInRange = false;
    private float currentHealth;
    private float playerWeaponDamage;

    [SerializeField] private Animator my_Anim;

    [Header("References")]
    private PlayerManager m_PlayerManager;
    private PlayerController m_PlayerController;
    private PlayerStats m_PlayerStats;
    [SerializeField] private DamageFlash m_DamageFlash;
    protected AudioManager m_AudioManager;

    [Header("Sound Effects")]
    [SerializeField] private string m_hurt;
    [SerializeField] private string m_death;
    [SerializeField] private string m_moving;
    [SerializeField] protected string m_attack;


    protected virtual void Start()
    {
        m_PlayerManager = PlayerManager.Instance;
        m_PlayerController = m_PlayerManager._playerController;
        m_PlayerStats = m_PlayerManager._playerStats;
        m_AudioManager = AudioManager.Instance;
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        //my_Anim.SetFloat("horizontalVelocity", rb.velocity.x);
        if (Input.GetButtonDown("Attack") && isInRange)
        {
            TakeDamage(playerWeaponDamage);
        }
    }

    private void TakeDamage(float recievedDamage)
    {
        currentHealth -= recievedDamage;
        if(currentHealth < 0)
        {
            //TODO: Animate death, set destroy to delete gameobject after animation finished.
            //my_Anim.SetBool("isExploded", true);
            
            Destroy(gameObject);
            m_AudioManager.PlaySoundOnce(m_death);
        } 
        else //Takes damage but doesn't die
        {
            m_DamageFlash.CallDamageFlash();
            m_AudioManager.PlaySoundOnce(m_hurt);
        }
    }

    public void DealDamage()
    {
        m_PlayerStats.RecieveDamage(damage);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Weapon"))
        {
            isInRange = true;
            playerWeaponDamage = collision.transform.GetComponent<Weapon>().GetDamage();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Weapon"))
        {
            isInRange = false;
        }
    }
}

