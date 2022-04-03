using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    [SerializeField] private Transform enemiesParent;
    private float currentSpawnTime = 7f;
    private int currentRepeatInt = 1;

    [SerializeField] private int repeatSpawnsEvery = 10;
    void Awake()
    {

        instance = this;
    }
   
    public void SetSpawnTimeAndFrequency(float newSpawnTime, int newRepeatInt)
    {
        currentSpawnTime = newSpawnTime;
        currentRepeatInt = newRepeatInt;
    }

    IEnumerator SpawnEnemy()
    {
        int randomInt = Random.Range(0, 2);
        float timeToSpawn = currentSpawnTime;
        int repeatInt = (LevelManager.instance.currentTokensCollected / repeatSpawnsEvery) +1;//1 = no repeats, 2=1 repeat, [repeats = X-1]
        if (randomInt == 0)
        {
            while (repeatInt > 0)
            {
                yield return new WaitForSeconds(2.0f);

                GameObject newSmallEnemy0 = ObjectPooler.Instance.SpawnFromPool("Small Enemy", LevelManager.instance.levels[LevelManager.instance.currentLevelInt].enemySpawnPoint[0].position, LevelManager.instance.levels[LevelManager.instance.currentLevelInt].enemySpawnPoint[0].rotation);

                yield return new WaitForSeconds(2.0f);

                GameObject newBigEnemy0 = ObjectPooler.Instance.SpawnFromPool("Big Enemy", LevelManager.instance.levels[LevelManager.instance.currentLevelInt].enemySpawnPoint[0].position, LevelManager.instance.levels[LevelManager.instance.currentLevelInt].enemySpawnPoint[0].rotation);

                if (repeatInt != 0) { repeatInt--; }

            }

        }
        else if (randomInt == 1)
        {

            while (repeatInt > 0)
            {
                yield return new WaitForSeconds(2.0f);

                GameObject newSmallEnemy0 = ObjectPooler.Instance.SpawnFromPool("Small Enemy", LevelManager.instance.levels[LevelManager.instance.currentLevelInt].enemySpawnPoint[0].position, LevelManager.instance.levels[LevelManager.instance.currentLevelInt].enemySpawnPoint[0].rotation);

                yield return new WaitForSeconds(2.0f);

                GameObject newSmallEnemy1 = ObjectPooler.Instance.SpawnFromPool("Small Enemy", LevelManager.instance.levels[LevelManager.instance.currentLevelInt].enemySpawnPoint[0].position, LevelManager.instance.levels[LevelManager.instance.currentLevelInt].enemySpawnPoint[0].rotation);

                yield return new WaitForSeconds(3.0f);

                GameObject newSmallEnemy2 = ObjectPooler.Instance.SpawnFromPool("Small Enemy", LevelManager.instance.levels[LevelManager.instance.currentLevelInt].enemySpawnPoint[0].position, LevelManager.instance.levels[LevelManager.instance.currentLevelInt].enemySpawnPoint[0].rotation);

                if (repeatInt != 0) { repeatInt--; }

            }
        }
        yield return new WaitForSeconds(timeToSpawn);
        StartCoroutine(SpawnEnemy());
    }
  
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemy());
    }

  
    public void EndSpawning()
    {
        StopAllCoroutines();
    }

    



   
}
