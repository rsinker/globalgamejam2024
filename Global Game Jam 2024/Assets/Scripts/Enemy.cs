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

    protected virtual void Start()
    {
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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth < 0)
        {
            //TODO: Animate death, set destroy to delete gameobject after animation finished.
            //my_Anim.SetBool("isExploded", true);
            
            Destroy(gameObject, 1f);
        } 
        else //Takes damage but doesn't die
        {
            //TODO: Flash red. Play sound effect?
        }
    }

    protected void DealDamage()
    {
        //player.RecieveDamage(damage);
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

