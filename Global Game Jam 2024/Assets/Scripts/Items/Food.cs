using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    [SerializeField] public Ingredient _ingredient;
    [SerializeField] private string m_PickUpSound;
    [SerializeField] private string m_DropSound;
    private AudioManager m_AudioManager;
    private bool isPickedUp;
    private bool nearEquipment;
    private bool nearPlate;
    [SerializeField] public bool isCooked;
    private Vector2 originalScale;
    [SerializeField] private float shrinkScale;
    private CookingManager _cookingManager;
    protected override void Start()
    {
        base.Start();
        _cookingManager = CookingManager.Instance;
        originalScale = transform.localScale;
        isPickedUp = false;
    }
    override protected void Interact()
    {
        if (!isInteractable) return;
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
        Vector2 dist = (player.transform.position - this.transform.position);
        if (dist.magnitude > 1) return;
        _cookingManager._carriedItem = this;
        isPickedUp = true;
        player.IsCarrying = true;
        transform.SetParent(player.transform, false);
        transform.localPosition = Vector2.zero;
        transform.localScale *= shrinkScale;
    }

    private void Drop()
    {
        
        if (nearEquipment) return;
        if (!isPickedUp) { return; }
        _cookingManager._carriedItem = null;
        if (player != null) player.IsCarrying = false;
        isPickedUp = false;
        if (nearPlate)
        {
            _cookingManager.PlateIngredient(this);
            return;
        }
        transform.SetParent(null);
        transform.localScale = originalScale;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if(collision.CompareTag("Equipment"))
        {
            Debug.Log("Near Equipment");
            nearEquipment = true;
        }
        if(collision.CompareTag("Plate"))
        {
            Debug.Log("Near Plate");
            nearPlate = true;
        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Equipment"))
        {
            
            nearEquipment = false;
        }
        if (collision.CompareTag("Plate"))
        {
            nearPlate = false;
        }
    }
}
