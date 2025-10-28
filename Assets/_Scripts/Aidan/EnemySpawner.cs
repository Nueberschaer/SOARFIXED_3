using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public GameObject[] enemyTypes; //array that holds the enemy prefabs for spawning  I CANT FIGURE OUT HOW TO DO ARRAYS

    public GameObject fused;
    public GameObject advancer;

    private GameObject randomEnemy;
    Vector3 levelOneSpawnPosition;
    Vector3 levelTwoSpawnPosition;

    private int spawnCount = 100;
    
    private int chasmHeightMin = -240;
    private int chasmHeightMax = -290;
    private int chasmSideMin = -717;
    private int chasmSideMax = -686;

    void Start()
    {

        LevelOneEnemies();
        //LevelTwoEnemies();
    }


    private void LevelOneEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelOneSpawnPosition = new Vector3(Random.Range(-1250, 1900), Random.Range(chasmHeightMin, chasmHeightMax), Random.Range(chasmSideMin, chasmSideMax)); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelOneSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    private void LevelTwoEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelTwoSpawnPosition = new Vector3(Random.Range(-1900, 5050), Random.Range(chasmHeightMin, chasmHeightMax), Random.Range(chasmSideMin, chasmSideMax)); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelTwoSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    private void EnemyPicker()
    {
        int luckyNumber = Random.Range(0, 100); //selects between fused and advancer 75% chance of choosing fused.

        if (luckyNumber < 75)
        {
            randomEnemy = fused;
        }
        else
        {
            randomEnemy = advancer;
        }
    }
}
