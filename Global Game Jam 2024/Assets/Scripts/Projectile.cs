using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    
    //References
    public Transform t_Player { private get; set; }

    [Header("Stats")]
    [SerializeField] private float m_ProjectileSpeed = 1f;
    [SerializeField] [Range(0.0f, 1.0f)] private float m_BulletFollow = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
        
        Launch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch()
    {
        Vector2 playerVel = t_Player.GetComponent<Rigidbody2D>().velocity;
        Vector2 distance = t_Player.position - transform.position; 
        float estTimeToHit = (playerVel.magnitude == 0) ? 0 : (distance.magnitude / playerVel.magnitude);
        Vector2 estDeltaDistance = playerVel * estTimeToHit;
        estDeltaDistance *= m_BulletFollow;
        Vector2 estDistance = distance + estDeltaDistance;
        
        m_Rigidbody.velocity = estDistance.normalized * m_ProjectileSpeed;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {

        }
        else if (collision.transform.CompareTag("Player"))
        {

        }
    }
}



