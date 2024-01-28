using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] float maxHealth = 10f;
    [SerializeField] private float damage = 1f;
    [SerializeField] protected float speed = 0.01f;
    [SerializeField] protected float m_deathAnimationDelay = 0.5f;
    [SerializeField] private float m_shrinkFactor = .25f;
    [SerializeField] protected float m_shrinkLength = .1f;
    [SerializeField] private Ingredient[] m_Ingredients;
    [SerializeField] public int stage = 1; // 1 2 3

    private Vector3 origScale;
    private bool isInRange = false;
    private float currentHealth;
    private float playerWeaponDamage;
    
    protected bool isDead => currentHealth < 0;

    protected Animator m_Animator;

    [Header("References")]
    private PlayerManager m_PlayerManager;
    protected PlayerController m_PlayerController;
    private PlayerStats m_PlayerStats;
    [SerializeField] private DamageFlash m_DamageFlash;
    protected AudioManager m_AudioManager;
    protected SpriteRenderer m_SpriteRenderer;

    [Header("Sound Effects")]
    [SerializeField] private string m_hurt;
    [SerializeField] private string m_death;
    [SerializeField] private string m_moving;
    [SerializeField] protected string m_attack;


    protected abstract IEnumerator CombatLoop();

    protected virtual void Start()
    {
        m_PlayerManager = PlayerManager.Instance;
        m_PlayerController = m_PlayerManager._playerController;
        m_PlayerStats = m_PlayerManager._playerStats;

        m_AudioManager = AudioManager.Instance;
        currentHealth = maxHealth;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        origScale = transform.localScale;
    }

    protected virtual void Update()
    {
        //my_Anim.SetFloat("horizontalVelocity", rb.velocity.x);
        if (Input.GetButtonDown("Attack") && isInRange)
        {
            TakeDamage(playerWeaponDamage);
            StartCoroutine(TakeDamageAnimation());
        }
    }

    void DropIngredients()
    {
        foreach(Ingredient ingredient in m_Ingredients)
        {
            Instantiate(ingredient._foodPrefab, transform.position, Quaternion.identity);
        }
    }

    IEnumerator TakeDamageAnimation()
    {
        transform.localScale *= (1 - m_shrinkFactor);
        yield return new WaitForSeconds(m_shrinkLength);
        transform.localScale = origScale;
    }

    protected void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, m_PlayerController.transform.position, speed);
        Vector2 direction = (m_PlayerController.transform.position - transform.position).normalized;
        m_SpriteRenderer.flipX = direction.x < 0;
    }

    private void TakeDamage(float recievedDamage)
    {
        
        currentHealth -= recievedDamage;
        if(currentHealth < 0)
        {
            Die();
            GameManager.enemiesKilledInRoom++;
            m_Animator.SetBool("isDead", true);
            DropIngredients();
            //TODO: Animate death, set destroy to delete gameobject after animation finished.
            //my_Anim.SetBool("isExploded", true);
            StopAllCoroutines();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, m_deathAnimationDelay);
            m_AudioManager.PlaySoundOnce(m_death);
        } 
        else //Takes damage but doesn't die
        {
            //m_DamageFlash.CallDamageFlash();
            m_AudioManager.PlaySoundOnce(m_hurt);
        }
    }

    public virtual void Die()
    {

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

