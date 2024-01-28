using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailCornScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] GameObject player;
    [SerializeField] GameObject snail;
    [SerializeField] float speed = 0.012f;
    [SerializeField] SpriteRenderer sp;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        snail.transform.position = Vector2.MoveTowards(snail.transform.position, player.transform.position, speed);

        Vector2 direction = (player.transform.position - snail.transform.position).normalized;
        sp.flipX = direction.x < 0;
    }
}
