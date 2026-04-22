using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 4f;
    public int spawnAmount = 1;
    public int spawnRadius = 8;
    public int maxEnemyCount = 5;

    void Start()
    {
        StartCoroutine(SpawnEnemies(spawnInterval));
    }
    IEnumerator SpawnEnemies(float spawnFrequency)
    {
        while (true)
        {
            var enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (maxEnemyCount > enemyCount)
            {
                SpawnEnemies();
            }
            yield return new WaitForSeconds(spawnFrequency);
        }
    }
    void SpawnEnemies()
    {
        var positionOffset = Random.insideUnitSphere*spawnRadius;
        Instantiate(enemyPrefab, transform.position+positionOffset, transform.rotation);
    }
}