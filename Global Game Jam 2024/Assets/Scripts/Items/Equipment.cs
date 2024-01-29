using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item  //Blender, Deep Fryer, Pan
{
    [SerializeField] private CookingType _cookingType = CookingType.STOVED;
    [SerializeField] private float _cookingTime = 5f;
    [SerializeField] private float _shrinkScale = 0.8f;

    private CookingManager _cookingManager;
    private PlayerManager _playerManager;

    private Ingredient _currentIngredient;
    private Food _currentIngredientObject;
    private Vector3 _origScale;

    private bool isCooking = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _cookingManager = CookingManager.Instance;
        _playerManager = PlayerManager.Instance;
        _origScale = transform.localScale;
    }

    IEnumerator Cook(Food food)
    {
        Debug.Log("Start Cooking!");
        _currentIngredientObject = food;
        _currentIngredient = _currentIngredientObject._ingredient;
        isCooking = true;
        yield return new WaitForSeconds(_cookingTime);
        Debug.Log("Done Cookin!");
        _currentIngredientObject.isCooked = true;
        isCooking = false;
    }

    override protected void Interact()
    {
        if (!isInteractable) return;
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
            
            if (!_currentIngredientObject.isCooked)
            {
                return; 
            }

            //Else, assign what's on the stove to CookingManager and remove it from the stove
            
            _cookingManager._carriedItem = _currentIngredientObject;
            UnlockIngredient(_cookingManager._carriedItem.transform);
            _currentIngredient = null;
            _currentIngredientObject = null;
            return;
        }

        else //The guy is carrying food
        {
            if (_currentIngredient != null) 
            {
                //Carrying something, somemething on stove
                return;
            }
            //Carrying something, nothing on stove

            if (_cookingManager._carriedItem.isCooked) //Is the carried food already cooked?
            {
                return;
            }

            Debug.Log(_cookingManager._carriedItem._ingredient._cookingType);
            Debug.Log(this._cookingType);

            if (_cookingManager._carriedItem._ingredient._cookingType != this._cookingType) //Is the carried food not supposed to go here?
            {
                //Put item on the stove
                return;
            }

            LockIngredient(_cookingManager._carriedItem.transform);
            _currentIngredientObject = _cookingManager._carriedItem;
            _currentIngredient = _currentIngredientObject._ingredient;
            StartCoroutine(Cook(_cookingManager._carriedItem));
            _cookingManager._carriedItem = null;
            return;
        }
    }

    void LockIngredient(Transform t)
    {
        t.SetParent(transform, false);
        t.localPosition = Vector2.zero;
        t.localScale *= _shrinkScale;
    }

    void UnlockIngredient(Transform t)
    {
        t.SetParent(null);
        t.localScale = _origScale;
    }

    
    
}
