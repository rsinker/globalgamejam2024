using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    HealthRegen,
    MaxHealth,
    AttackSpeed,
    MovementSpeed,
    Damage
}
public class Resource : Item
{
    [SerializeField] private ResourceType m_ResourceType = ResourceType.MaxHealth;
    [SerializeField] private float m_Modifer = 1f;
    protected override void Start()
    {
        base.Start();
    }
    override protected void Interact()
    {
        if (isInteractable)
        {
            Destroy(gameObject);
        }
    }
}
