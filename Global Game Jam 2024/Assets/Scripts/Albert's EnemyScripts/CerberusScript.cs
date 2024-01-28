using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] GameObject player;
    [SerializeField] GameObject cerberus;
    [SerializeField] float speed = 0.012f;
    [SerializeField] SpriteRenderer sp;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cerberus.transform.position = Vector2.MoveTowards(cerberus.transform.position, player.transform.position, speed);

        Vector2 direction = (cerberus.transform.position - player.transform.position).normalized;
        sp.flipX = direction.x < 0;
    }
}
