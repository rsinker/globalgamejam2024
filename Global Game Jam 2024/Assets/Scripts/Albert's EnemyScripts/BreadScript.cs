using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] GameObject player;
    [SerializeField] GameObject bread;
    [SerializeField] float speed = 0.012f;
    [SerializeField] SpriteRenderer sp;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bread.transform.position = Vector2.MoveTowards(bread.transform.position, player.transform.position, speed);

        Vector2 direction = (player.transform.position - bread.transform.position).normalized;
        sp.flipX = direction.x < 0;
    }
}
