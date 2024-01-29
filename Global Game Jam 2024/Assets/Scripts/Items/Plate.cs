using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : Item
{
    private bool isPickedUp;
    protected override void Start()
    {
        base.Start();
        isPickedUp = false;
    }

    override protected void Interact()
    {
        if (!isPickedUp && isInteractable)
        {
            PickUp();
        }
        else
        {
            Drop();
        }
    }
    private void PickUp()
    {
        //Make sure player can only pick up one Item at a time
        if (player.IsCarrying) return;

        isPickedUp = true;
        player.IsCarrying = true;
        transform.SetParent(player.transform, false);
        transform.localPosition = Vector2.zero;
    }

    private void Drop()
    {
        if (!isPickedUp) { return; }
        if (player != null) player.IsCarrying = false;
        isPickedUp = false;
        transform.SetParent(null);
    }
}
