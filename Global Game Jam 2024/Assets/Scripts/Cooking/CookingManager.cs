using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CookingType
{
    BLENDED,
    FRYED,
    STOVED
}

public class CookingManager : MonoBehaviour
{
    [SerializeField] private Recipe[] _easyRecipes;
    [SerializeField] private Recipe[] _mediumRecipes;
    [SerializeField] private Recipe[] _hardRecipes;
    [SerializeField] private Recipe _testRecipe;

    [SerializeField] private Transform _plate;

    private Recipe _currentRecipe;
    private List<Ingredient> _ingredientList;

    public static CookingManager Instance;

    public bool isRecipeComplete = false;


    public Food _carriedItem = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void PlateIngredient(Food food)
    {
        
        if(!_ingredientList.Contains(food._ingredient))
        {
            return;
        }
        _ingredientList.Remove(food._ingredient);
        
        LockInPlace(food.transform);
        food.enabled = false;
        if (_ingredientList.Count <= 0)
        {
            for (int i = 0; i < _plate.childCount; i++)
            {
                Destroy(_plate.GetChild(i).gameObject);
            }
            //Instantiate(_currentRecipe.recipePrefabs);
            _plate.GetComponent<Plate>().enabled = true;
        }
        isRecipeComplete = true;
    }

    void LockInPlace(Transform t)
    {
        t.SetParent(_plate, false);
        t.localPosition = Vector2.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateEasyRecipes()
    {
        _currentRecipe = _easyRecipes[Random.Range(0, _easyRecipes.Length)];
        _ingredientList = new List<Ingredient>();
        foreach(Ingredient ingredient in _currentRecipe.m_ingredientList)
        {
            _ingredientList.Add(ingredient);
        }
    }

    public void GenerateMediumRecipes()
    {
        _currentRecipe = _mediumRecipes[Random.Range(0, _mediumRecipes.Length)];
        _ingredientList = new List<Ingredient>();
        foreach (Ingredient ingredient in _currentRecipe.m_ingredientList)
        {
            _ingredientList.Add(ingredient);
        }
    }

    public void GenerateHardRecipes()
    {
        _currentRecipe = _hardRecipes[Random.Range(0, _hardRecipes.Length)];
        _ingredientList = new List<Ingredient>();
        foreach (Ingredient ingredient in _currentRecipe.m_ingredientList)
        {
            _ingredientList.Add(ingredient);
        }
    }

    public void GenerateTestRecipe()
    {
        _currentRecipe = _testRecipe;
        _ingredientList = new List<Ingredient>();
        foreach (Ingredient ingredient in _currentRecipe.m_ingredientList)
        {
            _ingredientList.Add(ingredient);
        }
    }
}
