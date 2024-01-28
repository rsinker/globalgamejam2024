using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    maxHealth,
    regenHealth,
    damage,
    movementSpeed,
    rats
}
public class Resource : Item
{
    [SerializeField] private ResourceType m_resourceType;
    [SerializeField] private float m_value;
    private PlayerStats m_playerStats;
    [SerializeField] private string m_pickupSound;

    protected override void Start()
    {
        base.Start();
        PlayerManager.Instance._playerStats = m_playerStats;
    }

    override protected void Interact()
    {
        if(m_resourceType == ResourceType.maxHealth)
        {
            m_playerStats.m_MaxHealth += (int) m_value;
        }
        else if(m_resourceType == ResourceType.regenHealth)
        {
            m_playerStats.m_CurrentHealth += (int)m_value;
        }
        else if(m_resourceType == ResourceType.damage)
        {
            m_playerStats.m_DamageModifier += (int)m_value;
        }
        else if(m_resourceType== ResourceType.rats)
        {
            m_playerStats.m_Rats += (int)m_value;
        }
        AudioManager.Instance.PlaySoundOnce(m_pickupSound);
        Destroy(gameObject);
    }
}
