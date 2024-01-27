using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    [Header("Basic Stats")]
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float damage = 1f;
    private float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth < 0)
        {
            //TODO: Animate death, set destroy to delete gameobject after animation finished.
            Destroy(gameObject, 1f);
        } 
        else //Takes damage but doesn't die
        {
            //TODO: Flash red. Play sound effect?
        }
    }

    protected void DealDamage()
    {
        
    }
}

