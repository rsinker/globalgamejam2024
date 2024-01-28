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

    IEnumerator Cook(Food food)
    {
        Debug.Log("Start Cooking!");
        _currentIngredientObject = food;
        _currentIngredient = _currentIngredientObject._ingredient;
        isCooking = true;
        yield return new WaitForSeconds(_cookingTime);
        Debug.Log("Done Cookin!");
        _currentIngredient.isCooked = true;
        isCooking = false;
    }

    override protected void Interact()
    {
        if (isCooking)
        {
            return;
        }

        if (_cookingManager._carriedItem == null) //The guy is not carrying food
        {
            if (_currentIngredient == null) //Is there not anything on the stove?
            {
                return;
            }
            
            if (!_currentIngredient.isCooked)
            {
                return; 
            }

            //Else, assign what's on the stove to CookingManager and remove it from the stove
            _cookingManager._carriedItem = _currentIngredientObject;
            _currentIngredient = null;
            _currentIngredientObject = null;
        }

        else if(_cookingManager._carriedItem != null) //The guy is carrying food
        {
            if (_currentIngredient != null) 
            {
                //Carrying something, somemething on stove
                return;
            }
            //Carrying something, nothing on stove

            if (_cookingManager._carriedItem._ingredient.isCooked) //Is the carried food already cooked?
            {
                return;
            }

            if (_cookingManager._carriedItem._ingredient._cookingType == this._cookingType) //Is the carried food not supposed to go here?
            {
                //Put item on the stove
                return;
            }
            
            _currentIngredientObject = _cookingManager._carriedItem;
            _currentIngredient = _currentIngredientObject._ingredient;
            _cookingManager._carriedItem = null;
            StartCoroutine(Cook(_cookingManager._carriedItem));
        }
        Debug.LogError("Cooking logic branch not caught!");
        return;

    }
}
