using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    [Header("Exploding Enemy Stats")]
    [SerializeField] private Explosion _Explosion;
    [SerializeField] public float m_explosionDelay = 1f;
    [SerializeField] public float m_explosionAnimationDelay = 0.5f;
    private bool isExploding = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(CombatLoop());
    }

    protected override IEnumerator CombatLoop()
    {

        while(!isExploding)
        {
            FollowPlayer();
            yield return null;
        }  
    }

    public void StopCombatLoop()
    {
        isExploding = true;
    }
    public void Explode()
    {
        
        Destroy(gameObject, m_explosionAnimationDelay);
    }

    public void StartAnimation()
    {
        m_Animator.SetBool("isExploded", true);
    }
}
