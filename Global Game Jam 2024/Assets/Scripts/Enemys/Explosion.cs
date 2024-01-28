using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private bool m_isInRange = false;
    private bool m_isExploded = false;
    private bool m_isExploding = false;
    
    [SerializeField] private ExplodingEnemy _Enemy;
    private float m_timeLeft;

    public void Start()
    {
        m_timeLeft = _Enemy.m_explosionDelay;
    }

    public void Update()
    {

        if(!m_isExploding)
        {
            return;
        }
        m_timeLeft -= Time.deltaTime;
        if(m_timeLeft < 0 && !m_isExploded)
        {
            _Enemy.Explode();
            if(m_isInRange)
            {
                _Enemy.DealDamage();
            }
            m_isExploded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            m_isInRange = true;
            m_isExploding = true;
            _Enemy.StopCombatLoop();
            _Enemy.StartAnimation();
        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            m_isInRange = false;
        }
    }


}
