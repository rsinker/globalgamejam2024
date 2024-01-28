using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProjectileEnemy : Enemy
{

    [Header("Projectile Enemy Stats")]
    [SerializeField] private float m_AttackCooldown = 3f;
    [SerializeField] private float m_AttackLength = 1f;
    [SerializeField] private float m_AttackFrequency = .1f;
    private bool m_IsAttacking = false;

    [Header("Projectile Components")]
    [SerializeField] private Projectile _Projectile;
    [SerializeField] private Transform _SpawnPoint;

    [Header("Projectile Stats")]
    [SerializeField] public float m_ProjectileSpeed = 1f;
    [SerializeField][Range(0.0f, 1.0f)] public float m_BulletFollow = 1f;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        StartCoroutine(EnemyLoop());
    }
    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private IEnumerator Shoot()
    {
        int projectileCount = (int)(m_AttackLength / m_AttackFrequency);
        for (int i = 0; i < projectileCount; i++)
        {
            Projectile p = Instantiate(_Projectile, _SpawnPoint.position, Quaternion.identity, _SpawnPoint);
            p.t_Player = m_PlayerController.transform;
            p.m_Enemy = this;
            yield return new WaitForSeconds(m_AttackFrequency);
        }
    }

    IEnumerator EnemyLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_AttackCooldown);
            m_Animator.SetBool("isShooting", true);
            m_IsAttacking = true;

            yield return StartCoroutine(Shoot());
            m_Animator.SetBool("isShooting", false);
            m_IsAttacking = false;


        }
        
    }

    void UpdateMovement()
    {
        if (m_IsAttacking)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, m_PlayerController.transform.position, speed);
        Vector2 direction = (m_PlayerController.transform.position - transform.position).normalized;
        m_SpriteRenderer.flipX = direction.x < 0;
    }


}



