using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Appearance")]
    [SerializeField] private Sprite m_Sprite;
    [SerializeField] private Sound m_SoundFX;

    [Header("Weapon Stats")]
    [SerializeField] private BoxCollider2D m_Collider;
    [SerializeField] private float m_baseDamage = 1f;
    [SerializeField] private float m_attackSpeed = .3f;

    [Header("Sound Effects")]
    [SerializeField] private string s_Attack;
    

    void Update()
    {
        if (Input.GetButtonDown("Attack"))
        {
            //TODO: Implement Swing Animation
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
