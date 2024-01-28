using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("Melee Enemy Stats")]
    [SerializeField] float m_attackDuration = 1f;

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
            FollowPlayer();
            yield return null;

        }
    }

    private IEnumerator Attack()
    {
        m_isAttacking = true;
        m_Animator.SetBool("isAttacking", m_isAttacking);
        yield return new WaitForSeconds(m_attackDuration);
        m_isAttacking = false;
        m_Animator.SetBool("isAttacking", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {

        }
    }
}