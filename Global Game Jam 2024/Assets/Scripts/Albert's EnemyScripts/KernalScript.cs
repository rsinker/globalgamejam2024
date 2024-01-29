using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KernalScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] GameObject player;
    [SerializeField] GameObject kernal;
    [SerializeField] float speed = 0.012f;
    [SerializeField] SpriteRenderer sp;
    [SerializeField] GameObject explosionRange; //make this a trigger collidedr
    [SerializeField] int dmg;
    [SerializeField] bool isExploding = false;
    [SerializeField] bool isDead = false;





    // Start is called before the first frame update
    void Start()
    {
        isExploding = false;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        kernal.transform.position = Vector2.MoveTowards(kernal.transform.position, player.transform.position, speed);

        anim.SetBool("isExploding", isExploding);
        anim.SetBool("isDead", isDead);
        

       // anim.SetFloat("HorizontalVelocity", transform.position.x);
      

        Vector2 direction = (player.transform.position - kernal.transform.position).normalized;
        sp.flipX = direction.x < 0;

        anim.SetFloat("Magnitude", direction.magnitude);

        

       // kernal.transform.position = direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isExploding = true;
            Debug.Log("Im Exploding!");
            //do dmg or somthing? player.RecieveDamge(2);
            //Destroy(kernal,3);
        }
    }
}
