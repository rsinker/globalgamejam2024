using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private recipeManager recipeGenerator;

    [SerializeField] private GameObject[] completeEnemyArray;
    private List<GameObject> currentEnemyArray;


    private Transform enemyParent;

    private void Awake()
    {
        currentEnemyArray = completeEnemyArray.ToList();
        enemyParent = new GameObject("EnemyParentObj").transform;
    }
    
    public void SpawnEnemies()
    {
        

        int randEnemyIndex = Random.Range(0, currentEnemyArray.Count);
        GameObject toSpawn = currentEnemyArray[randEnemyIndex];

        

        Vector2Int enemySpawnPoint = new Vector2Int(Random.Range(boardManager.currentRoom.left, boardManager.currentRoom.right), Random.Range(boardManager.currentRoom.bottom, boardManager.currentRoom.top));

        if (boardManager.currentRoom.GetTile(enemySpawnPoint.x, enemySpawnPoint.y) != "floor")
        {
            enemySpawnPoint = boardManager.currentRoom.GetValidPosition(enemySpawnPoint);
        }

        GameObject enemyObj = Instantiate(toSpawn, new Vector3(enemySpawnPoint.x + 0.5f, enemySpawnPoint.y + 0.5f, 0), Quaternion.identity);
        enemyObj.transform.SetParent(enemyParent);
    }

    public void ResetEnemies()
    {
        //Delete
        Destroy(enemyParent.gameObject);
        enemyParent = new GameObject("EnemyParentObj").transform;
    }

    public void InstanceEnemies()
    {
        currentEnemyArray = (recipeGenerator.currentRecipe.m_Stage1Enemys).ToList();

        foreach (GameObject en in recipeGenerator.currentRecipe.m_Stage2Enemys)
        {
            currentEnemyArray.Add(en);
        }

        foreach (GameObject en in recipeGenerator.currentRecipe.m_Stage3Enemys)
        {
            currentEnemyArray.Add(en);
        }
    }
}
