using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject fused;
    public GameObject advancer;

    private GameObject randomEnemy;
    Vector3 levelOneSpawnPosition;
    Vector3 levelTwoSpawnPosition;
    Vector3 levelThreeSpawnPosition;
    Vector3 levelFourSpawnPosition;
    Vector3 levelFiveSpawnPosition;

    private int spawnCount = 300;
    

    //boundaries of enemy spawning
    private int chasmHeightMin = -240; 
    private int chasmHeightMax = -290;
    private int chasmSideMin = 686;
    private int chasmSideMax = 717;
    private int levelOneStart = -200;
    private int levelOneEnd = 3000;
    private int levelDistance = 3200;

    void Awake()
    {

        LevelOneEnemies();
    }

    //Spawn info for each level

    private void LevelOneEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelOneSpawnPosition = new Vector3(Random.Range(chasmSideMin, chasmSideMax), Random.Range(chasmHeightMin, chasmHeightMax), Random.Range(levelOneStart, levelOneEnd)); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelOneSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    public void LevelTwoEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelTwoSpawnPosition = new Vector3(Random.Range(chasmSideMin, chasmSideMax), Random.Range(chasmHeightMin, chasmHeightMax), Random.Range(levelOneStart + levelDistance, levelOneEnd + levelDistance)); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelTwoSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    public void LevelThreeEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelThreeSpawnPosition = new Vector3(Random.Range(chasmSideMin, chasmSideMax), Random.Range(chasmHeightMin, chasmHeightMax), Random.Range(levelOneStart + (levelDistance* 2 ), levelOneEnd + (levelDistance * 2))); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelThreeSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    public void LevelFourEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelFourSpawnPosition = new Vector3(Random.Range(chasmSideMin, chasmSideMax), Random.Range(chasmHeightMin, chasmHeightMax), Random.Range(levelOneStart + (levelDistance * 3), levelOneEnd + (levelDistance * 3))); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelFourSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    public void LevelFiveEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelFiveSpawnPosition = new Vector3(Random.Range(chasmSideMin, chasmSideMax), Random.Range(chasmHeightMin, chasmHeightMax), Random.Range(levelOneStart + (levelDistance * 4), levelOneEnd + (levelDistance * 4))); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelFiveSpawnPosition, Quaternion.identity); // spawns the enemy
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
