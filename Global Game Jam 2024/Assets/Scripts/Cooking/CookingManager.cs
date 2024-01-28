using System.Collections;
using System.Collections.Generic;
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

    private Recipe _currentRecipe;

    public static CookingManager Instance;


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
        _easyRecipes[Random.Range(0, _easyRecipes.Length)] = _currentRecipe;
    }

    public void GenerateMediumRecipes()
    {
        _mediumRecipes[Random.Range(0, _mediumRecipes.Length)] = _currentRecipe;
    }

    public void GenerateHardRecipes()
    {
        _hardRecipes[Random.Range(0, _hardRecipes.Length)] = _currentRecipe;
    }
}
