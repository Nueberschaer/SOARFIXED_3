using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject fused;
    public GameObject advancer;

    private GameObject randomEnemy;
    Vector3 levelOneSpawnPosition;
    Vector3 levelTwoSpawnPosition;

    private int spawnCount = 100;
    
    private int chasmHeightMin = -240;
    private int chasmHeightMax = -290;
    private int chasmSideMin = 686;
    private int chasmSideMax = 717;

    void Start()
    {

        LevelOneEnemies();
    }


    private void LevelOneEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelOneSpawnPosition = new Vector3(Random.Range(chasmSideMin, chasmSideMax), Random.Range(chasmHeightMin, chasmHeightMax), Random.Range(-200, 3000)); //controls the position in which enemies spawn

            Instantiate(randomEnemy, levelOneSpawnPosition, Quaternion.identity); // spawns the enemy
        }
    }

    public void LevelTwoEnemies()
    {
        for (int count = 0; count < spawnCount; count++) // spawns as many enemys as the count length
        {
            EnemyPicker(); //picks the enemy type
            levelTwoSpawnPosition = new Vector3(Random.Range(chasmSideMin, chasmSideMax), Random.Range(chasmHeightMin, chasmHeightMax), Random.Range(3100, 6300)); //controls the position in which enemies spawn

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
