using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee Enemy Stats")]
    [SerializeField] private float m_attackDuration = 1f;
    [SerializeField] private float m_scaleIncrease = 0.1f;

    private bool m_isAttacking = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(CombatLoop());
    }
    protected override IEnumerator CombatLoop()
    {
        while (true)
        {
            if (!m_isAttacking)
            {
                FollowPlayer();
            }
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        DealDamage();
        m_isAttacking = true;
        transform.localScale *= 1+m_scaleIncrease;
        m_Animator.SetBool("isAttacking", m_isAttacking);
        yield return new WaitForSeconds(m_attackDuration);
        m_isAttacking = false;
        transform.localScale /= 1 + m_scaleIncrease;
        m_Animator.SetBool("isAttacking", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (!m_isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
    }
}