using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spawnPoint;
    public WaveData[] waves;
    // Start is called before the first frame update
    void Start()
    {
        StartWave(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave(int waveNumber)
    {
        WaveData wave = waves[waveNumber];
        StartCoroutine(SpawnWave(wave));
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(wave.spawnRate);
        }
        yield return new WaitForSeconds(wave.timeBetweenWaves);
    }
}
[System.Serializable]
public class WaveData
{
    public int waveNumber;
    public int enemyCount;
    public float spawnRate;
    public float timeBetweenWaves;
}