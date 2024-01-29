using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Item : MonoBehaviour
{
    protected bool isInteractable;
    protected PlayerController player;
    protected abstract void Interact();
    protected virtual void Start()
    {
        isInteractable = false;
    }

    void FixedUpdate()
    {

    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && isInteractable) {
            Interact();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == null) {
            player = collision.gameObject.GetComponent<PlayerController>();
        }
        if (collision.gameObject.tag == "Player") {
            isInteractable = true;
        }  
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            isInteractable = false;
        }
    }
}
