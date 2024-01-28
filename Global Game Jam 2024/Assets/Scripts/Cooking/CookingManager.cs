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

    void GenerateRecipes()
    {

    }
}
