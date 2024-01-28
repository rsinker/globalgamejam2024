using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour 
{
    [Header("Appearance")]
    [SerializeField] private float m_shrinkFactor = .2f;
    private Vector3 origScale;

    [Header("Weapon Stats")]
    [SerializeField] private BoxCollider2D m_Collider;
    [SerializeField] private float m_baseDamage = 1f;
    [SerializeField] private float m_attackSpeed = .3f;

    [Header("Sound Effects")]
    [SerializeField] private string s_Attack;

    private void Start()
    {
        origScale = transform.localScale;
    }

    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            transform.localScale *= (1 - m_shrinkFactor);
        }
        if (Input.GetButtonUp("Attack"))
        {
            transform.localScale = origScale;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    }

    private void DealDamage(Enemy enemy)
    {
        
    }

    public float GetDamage()
    {
        return m_baseDamage;
    }
}
