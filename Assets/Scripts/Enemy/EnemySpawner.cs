using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Action OnEnemyDeath;
    public GameObject spawnPoint;
    public WaveData[] waves;

    public int waveNumber = 0;
    public int enemiesRemaining = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartWave(int waveNumber)
    {
        //if wave number is greater than the number of waves, return
        if (waveNumber >= waves.Length)
        {
            //create a new wave randomly
            WaveData wave = new WaveData();
            wave.waveNumber = waveNumber;
            wave.enemyCount = UnityEngine.Random.Range(1, 5) * waveNumber;
            wave.spawnRate = UnityEngine.Random.Range(1, 5);
            wave.timeBetweenWaves = UnityEngine.Random.Range(1, 5);
            wave.enemyPrefabs = waves[3].enemyPrefabs;
            waves[waveNumber] = wave;
            StartCoroutine(SpawnWave(wave));
        }
        else
        {
            WaveData Levelwave = waves[waveNumber];
            StartCoroutine(SpawnWave(Levelwave));
        }
    }

    IEnumerator SpawnWave(WaveData wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            Instantiate(wave.enemyPrefabs[UnityEngine.Random.Range(0, wave.enemyPrefabs.Length)], spawnPoint.transform.position, Quaternion.identity);
            enemiesRemaining++;
            OnEnemyDeath.Invoke();
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
    public GameObject[] enemyPrefabs;
}