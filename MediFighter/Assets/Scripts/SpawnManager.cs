using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public Text waveText;
    public GameObject[] enemyPrefabs;
    public List <GameObject> spawns;
    public List <GameObject> enemyAmount;
    public int enemiesLeft;
    public int enemiesToSpawn;
    public int enemySpawnInterval = 5;
    private int waveNum = 0;
    private int nextWaveDelay = 5;
    private int spawnRate = 3;
    private float chancePercentage = 0f;
    private bool canSpawn;
    private bool nextWave;
    private bool loadingWave;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NextWave());

        foreach (Transform spawn in transform)
        {
            if (spawn.tag == "SpawnPoint")
            {
                spawns.Add(spawn.gameObject);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (enemiesLeft != enemyAmount.Count)
        {
            enemiesLeft = enemyAmount.Count;
        }
        

        if (canSpawn && enemyAmount.Count >= 0 && enemiesToSpawn > 0 && loadingWave == false)
        {
            canSpawn = false;
            StartCoroutine(SpawnEnemy());
        }

        if (enemiesLeft + enemiesToSpawn <= 0 && !nextWave)
        {
            nextWave = true;
            loadingWave = true;
            StartCoroutine(NextWave());
        }
    }

    IEnumerator NextWave()
    {
        waveNum += 1;
        if (waveNum > 2 && chancePercentage == 0f)
        {
            chancePercentage = 0.3f;
        }
        enemiesToSpawn = enemySpawnInterval * waveNum;
        enemiesLeft = enemiesToSpawn;
        yield return new WaitForSeconds(3);
        waveText.text = "Wave: " + waveNum.ToString();
        waveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(nextWaveDelay);
        waveText.gameObject.SetActive(false);
        nextWave = false;
        canSpawn = true;
        loadingWave = false;
    }

    IEnumerator SpawnEnemy()
    {
        var enemyType = enemyPrefabs[0];
        float determiner = Random.Range(0f, 1f);

        if (determiner <= chancePercentage)
        {
            enemyType = enemyPrefabs[1];
        }

        var randomSpawn = Random.Range(0, spawns.Count);
        Vector3 spawnPosEnemy = spawns[randomSpawn].transform.position;

        enemyAmount.Add(Instantiate(enemyType, spawnPosEnemy, enemyType.transform.rotation));
        yield return new WaitForSeconds(spawnRate);
        canSpawn = true;
        enemiesToSpawn -= 1;
    }
}