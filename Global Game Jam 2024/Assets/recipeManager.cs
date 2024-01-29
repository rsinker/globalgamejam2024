using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class recipeManager : MonoBehaviour
{
    public List<Recipe> recipes;

    public Recipe currentRecipe;


    public EnemySpawner enemySpawner;

    public void PickNewRecipe()
    {
        int index = Random.Range(0, recipes.Count);
        currentRecipe = recipes[index];
        Debug.Log("New Recipe: " + currentRecipe.name);

        enemySpawner.InstanceEnemies();
    }

    public void RecipeMade()
    {
        
    }
}

//Shop sprite