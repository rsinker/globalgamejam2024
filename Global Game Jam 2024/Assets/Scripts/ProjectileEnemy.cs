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

    [Header("Projectile Components")]
    [SerializeField] private Projectile p_Projectile;
    [SerializeField] private Transform p_SpawnPoint;

    [Header("References")]
    private PlayerController _playerController;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _playerController = PlayerController.Instance;
    }
    protected override void Update()
    {
        base.Update();
        //Test code
        if (Input.GetButtonDown("Attack"))
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        int projectileCount = (int)(m_AttackLength / m_AttackFrequency);
        for (int i = 0; i < projectileCount; i++)
        {
            Projectile p = Instantiate(p_Projectile, p_SpawnPoint.position, Quaternion.identity, p_SpawnPoint);
            p.t_Player = _playerController.transform;
            yield return new WaitForSeconds(m_AttackFrequency);
        }
    }
}



