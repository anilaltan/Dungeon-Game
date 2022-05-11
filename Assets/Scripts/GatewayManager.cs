using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatewayManager : MonoBehaviour
{
    public GameObject[] spawners;
    private int enemyCount = 0;
    private int enemyLimit = 10;
    public int level = 0;
    void Start()
    {
        PrepareSpawners();
    }

    void PrepareSpawners()
    {
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        if (spawners.Length > 0)
        {
            int rnd = Random.Range(0, spawners.Length);
            spawners[rnd].GetComponent<SpawnManager>().SetGateway(true);
            foreach (GameObject spawner in spawners)
            {
                spawner.GetComponent<SpawnManager>().SetHealth((level * 10) + Random.Range(50, 100));
            }
        }

    }

    public void SetEnemyCount(int amount)
    {
        enemyCount += amount;
    }

    public int GetEnemyCount()
    {
        return enemyCount;
    }

    public int GetEnemyLimit()
    {
        return enemyLimit;
    }
}
