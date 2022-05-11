using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] spawnPoints;
    private float timer;
    private int spawnIndex = 0;
    public float health = 100;
    public Sprite deathSprite;
    public Sprite gateway;
    private bool isGateway = false;

    public Sprite[] sprites;

    private GameManager _gameManager;
    private GatewayManager _gatewayManager;
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gatewayManager = GameObject.Find("GatewayManager").GetComponent<GatewayManager>();
        int rnd = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[rnd];
        Instantiate(enemyPrefab, spawnPoints[0].transform.position, Quaternion.identity);
        Instantiate(enemyPrefab, spawnPoints[1].transform.position, Quaternion.identity);
        timer = Time.time + 7f;
        _gatewayManager.SetEnemyCount(2);
    }

    void Update()
    {
        if(timer < Time.time && _gatewayManager.GetEnemyCount() < _gatewayManager.GetEnemyLimit())
        {
            if(GetComponent<SpriteRenderer>().sprite != gateway)
            {
                Instantiate(enemyPrefab, spawnPoints[spawnIndex % 2].transform.position, Quaternion.identity);
                timer = Time.time + 7f;
                spawnIndex++;
                _gatewayManager.SetEnemyCount(1);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if(GetComponent<SpriteRenderer>().sprite != gateway)
        {
            health -= amount;
            Debug.Log("spawner health: " + health);
            GetComponent<SpriteRenderer>().color = Color.red;

            if (health < 0)
            {
                GetComponent<SpriteRenderer>().sprite = deathSprite;
                if (isGateway)
                {
                    Debug.Log("gate is open");
                    Invoke("OpenGateWay", 0.5f);
                }
                else
                {
                    Invoke("DestroySpawner", 0.6f);
                }
            }
            Invoke("DefaultColor", 0.3f);
        }
    }

    private void OpenGateWay()
    {
        GetComponent<SpriteRenderer>().sprite = gateway;
    }

    private void DestroySpawner()
    {
        Destroy(gameObject);
    }

    private void DefaultColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void SetHealth(int newHealth)
    {
        health = newHealth;
    }

    public void SetGateway(bool maybe)
    {
        isGateway = maybe;
    }

    public void GetGateway()
    {
        if(GetComponent<SpriteRenderer>().sprite == gateway)
        {
            _gameManager.LoadLevel();
        }
    }
}
