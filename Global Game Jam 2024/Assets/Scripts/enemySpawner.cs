using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;

    [SerializeField] private GameObject[] completeEnemyArray;
    private GameObject[] currentEnemyArray;


    private Transform enemyParent;

    private void Awake()
    {
        currentEnemyArray = completeEnemyArray;
        enemyParent = new GameObject("EnemyParentObj").transform;
    }
    
    public void SpawnEnemies()
    {
        /*enemyParent = new GameObject("EnemyParentObj").transform;
        int difficulty = (int)(GameManager.roomCount * GameManager.ROOMS_TO_DIFFICULTY);
        Debug.Log("Difficulty: " + difficulty);
        //Index correct difficulty of enemy
        if (difficulty == 0)
        {
            int index = Random.Range(0, cookingManager.m_Stage1Enemys.Length);
            cookingManager.m_Stage1Enemys[index];
        }
        else if (difficulty == 1)
        {
            int index = Random.Range(0, cookingManager.m_Stage2Enemys.Length);
            cookingManager.m_Stage2Enemys[index];
        }
        else if (difficulty == 2)
        {
            int index = Random.Range(0, cookingManager.m_Stage3Enemys.Length);
            cookingManager.m_Stage3Enemys[index];
        }*/

        int randEnemyIndex = Random.Range(0, currentEnemyArray.Length);
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
}
