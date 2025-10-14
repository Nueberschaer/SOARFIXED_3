using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public GameObject[] enemyTypes; //array that holds the enemy prefabs for spawning  I CANT FIGURE OUT HOW TO DO ARRAYS

    public GameObject fused;
    public GameObject advancer;

    private GameObject randomEnemy;
    Vector3 randomSpawnPosition;

    private int spawnCount = 100;

    void Start()
    {

        SpawnEnemies();
    }


    private void SpawnEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            PositionPicker(); // picks the position

            Instantiate(randomEnemy, randomSpawnPosition, Quaternion.identity); // spawns the enemy
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

    private void PositionPicker()
    {
        randomSpawnPosition = new Vector3(Random.Range(-50, 51), Random.Range(-50, 51), Random.Range(-50, 51)); //controls the position in which enemies spawn
    }
}
