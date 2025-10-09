using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject fused; // spawning basic enemy later add more enemy types
    private int spawnCount = 100; 


    void Start()
    {

        SpawnEnemies();
    }


    private void SpawnEnemies()
    {
        for (int count = 0; count < spawnCount; count++)
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-50, 51), Random.Range(-50, 51), Random.Range(-50, 51)); //controls the position in which enemies spawn
            // random enemypicker add once more enemies
            Instantiate(fused, randomSpawnPosition, Quaternion.identity);
        }
    }
}
