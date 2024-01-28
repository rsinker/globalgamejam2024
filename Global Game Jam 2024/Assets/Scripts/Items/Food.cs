using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    private bool isPickedUp;
    private Vector2 originalScale;
    [SerializeField] private float shrinkScale;
    protected override void Start(){
        base.Start();
        originalScale = transform.localScale;
        isPickedUp = false;
    } 
    override protected void Interact()
    {
        if (!isPickedUp) {
            PickUp();
        } else {
            Drop();
        }
    }

    private void PickUp()
    {
        //Make sure player can only pick up one Item at a time
        if (player.IsCarrying) return;
        //Pick up food
        isPickedUp = true;
        player.IsCarrying = true;
        transform.SetParent(player.transform, false);
        transform.localPosition = Vector2.zero;
        transform.localScale *= shrinkScale;
    }

    private void Drop()
    {
        if(player != null) player.IsCarrying = false;
        isPickedUp = false;
        transform.SetParent(null);
        transform.localScale = originalScale;
    }
}
