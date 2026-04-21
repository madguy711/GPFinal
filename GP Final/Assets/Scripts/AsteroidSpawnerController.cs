using System.Collections;
using UnityEngine;

public class AsteroidSpawnerController : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    public float spawnInterval = 2f;
    public int spawnAmount = 1;
    void Start()
    {
        StartCoroutine(SpawnAsteroids(spawnInterval));
    }

    void Update()
    {
        
    }
    void SpawnAsteroids()
    {
        int asteroidIndex = Random.Range(0, asteroidPrefabs.Length);
        var positionOffset = Random.insideUnitSphere*5;
        Instantiate(asteroidPrefabs[asteroidIndex], transform.position+positionOffset, transform.rotation);
    }

    IEnumerator SpawnAsteroids(float spawnFrequency)
    {
        while (true)
        {
            SpawnAsteroids();
            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}
