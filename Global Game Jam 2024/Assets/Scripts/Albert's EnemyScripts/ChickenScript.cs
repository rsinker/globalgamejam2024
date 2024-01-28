using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenScript : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] GameObject player;
    [SerializeField] GameObject chicken;
    [SerializeField] float speed = 0.01f;
    [SerializeField] SpriteRenderer sp;
    [SerializeField] private float delay = 2f;
    private bool isWaiting = false;
    [SerializeField] private float Walkdelay = 5f;
    [SerializeField] private bool isShooting = false;
    [SerializeField] private bool isDead = false;
    [SerializeField] float health = 1f;
    [SerializeField] int dmg;
 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(waitToMove());
        isDead = false;

    }

    // Update is called once per frame
    void Update()
    {
        chicken.transform.position = Vector2.MoveTowards(chicken.transform.position, player.transform.position, speed);

        anim.SetBool("isShooting", isShooting);
        anim.SetBool("isDead", isDead);

        Vector2 direction = (player.transform.position - chicken.transform.position).normalized;
        sp.flipX = direction.x < 0;

        if(health <= 0)
        {
            speed = 0f;
            StopAllCoroutines();
            isShooting = false;
            isDead = true;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject,0.5f);
            

        }
    }

    IEnumerator waitToMove()
    {
        while(isWaiting == false)
        {
            yield return new WaitForSeconds(Walkdelay);
            isWaiting = true;
            speed = 0f;
            Debug.Log("Im shooting!");
            ShootEgg();
            Debug.Log("Im Starting to Wait");
            yield return new WaitForSeconds(delay);
            Debug.Log("I waited");
            speed = 0.01f;
            isWaiting = false;
            isShooting = false;
            Debug.Log("Im done shooting");

        }
    }

    private void ShootEgg()
    {
        //shooting code
        isShooting = true;
    }

    
   
   
}
