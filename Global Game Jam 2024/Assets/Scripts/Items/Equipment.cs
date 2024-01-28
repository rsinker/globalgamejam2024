using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item  //Blender, Deep Fryer, Pan
{
    [SerializeField] private CookingType _cookingType = CookingType.STOVED;
    [SerializeField] private float _cookingTime = 5f;

    private CookingManager _cookingManager;

    private Ingredient _currentIngredient;
    private Food _currentIngredientObject;

    private bool isCooking = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _cookingManager = CookingManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Cook()
    {
        isCooking = true;
        yield return new WaitForSeconds(_cookingTime);
        _currentIngredient.isCooked = true;
        isCooking = false;
    }

    override protected void Interact()
    {
        if (_cookingManager._carriedItem == null) return;

        if(isCooking)
        {
            return;
        }

        if(_currentIngredient != null) //Not cooking,  currentIngredient
        {

        }

        //Not cooking, no ingredient
        //Empty
        StartCoroutine(Cook());
        //Cooking
        //Finished Cooking
    }
}
