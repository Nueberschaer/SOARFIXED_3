using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject fused;
    public GameObject advancer;
    public GameObject follower;

    private GameObject randomEnemy;
    Vector3 levelOneSpawnPosition;
    Vector3 levelTwoSpawnPosition;
    Vector3 levelThreeSpawnPosition;
    Vector3 levelFourSpawnPosition;
    Vector3 levelFiveSpawnPosition;

    private int spawnCount = 150;
    

    //boundaries of enemy spawning
    private int _chasmHeightMin = -240; 
    private int _chasmHeightMax = -290;
    private int _chasmSideMin = 686;
    private int _chasmSideMax = 717;
    private int _levelOneStart = -200;
    private int _levelOneEnd = 3000;
    private int _levelDistance = 3200;
    private int _bufferZone = 300;

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
            levelOneSpawnPosition = new Vector3(Random.Range(_chasmSideMin, _chasmSideMax), Random.Range(_chasmHeightMin, _chasmHeightMax), Random.Range(_levelOneStart, _levelOneEnd)); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelOneSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    public void LevelTwoEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelTwoSpawnPosition = new Vector3(Random.Range(_chasmSideMin, _chasmSideMax), Random.Range(_chasmHeightMin, _chasmHeightMax), Random.Range(_levelOneStart + _levelDistance + _bufferZone, _levelOneEnd + _levelDistance)); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelTwoSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    public void LevelThreeEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelThreeSpawnPosition = new Vector3(Random.Range(_chasmSideMin, _chasmSideMax), Random.Range(_chasmHeightMin, _chasmHeightMax), Random.Range(_levelOneStart + (_levelDistance* 2 ) + _bufferZone, _levelOneEnd + (_levelDistance * 2))); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelThreeSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    public void LevelFourEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelFourSpawnPosition = new Vector3(Random.Range(_chasmSideMin, _chasmSideMax), Random.Range(_chasmHeightMin, _chasmHeightMax), Random.Range(_levelOneStart + (_levelDistance * 3) + _bufferZone, _levelOneEnd + (_levelDistance * 3))); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelFourSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    public void LevelFiveEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelFiveSpawnPosition = new Vector3(Random.Range(_chasmSideMin, _chasmSideMax), Random.Range(_chasmHeightMin, _chasmHeightMax), Random.Range(_levelOneStart + (_levelDistance * 4) + _bufferZone, _levelOneEnd + (_levelDistance * 4))); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelFiveSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }


    private void EnemyPicker()
    {
        int luckyNumber = Random.Range(0, 100); //selects between fused and advancer 75% chance of choosing fused.

        if (luckyNumber <= 50) randomEnemy = fused;

        else if (luckyNumber > 50 && luckyNumber < 70) randomEnemy = follower;

        else  randomEnemy = advancer;

    }
}
