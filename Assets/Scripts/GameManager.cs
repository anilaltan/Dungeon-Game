using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //private GameObject[] spawners;
    //private int level = 0; 
    //private int sceneNumber = 0; 
    private int currentScene = 0; 
    //private int enemyCount = 0;
    //private int enemyLimit = 10;
    public bool gameOver = false;

    public GameObject player;
    public GameObject hudCanvas;
    private static GameManager instance;

    void Start()
    {   
        //PrepareSpawners();
    }

    void Awake()
    {
        //DontDestroyOnLoad(player.gameObject);
        //DontDestroyOnLoad(hudCanvas.gameObject);
        DontDestroyOnLoad(this.gameObject);
        
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //void PrepareSpawners()
    //{
    //    spawners = GameObject.FindGameObjectsWithTag("Spawner");
    //    if(spawners.Length > 0)
    //    {
    //        int rnd = Random.Range(0, spawners.Length);
    //        spawners[rnd].GetComponent<SpawnManager>().SetGateway(true);
    //        foreach (GameObject spawner in spawners)
    //        {
    //            spawner.GetComponent<SpawnManager>().SetHealth(level + Random.Range(50, 100));
    //        }
    //    }
        
    //}

    //public void SetEnemyCount(int amount)
    //{
    //    enemyCount += amount;
    //}

    //public int GetEnemyCount()
    //{
    //    return enemyCount;
    //}

    //public int GetEnemyLimit()
    //{
    //    return enemyLimit;
    //}

    public void LoadLevel()
    {
        //enemyCount = 0;
        if(SceneManager.GetActiveScene().buildIndex != 18)
        {
            currentScene = 1;
        }
        //else
        //{
        //    currentScene = -1;
        //}
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + currentScene);
    }

    public void StartGame()
    {

        player.gameObject.SetActive(true);
        //player.GetComponent<Player>().currentLive = 100;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        hudCanvas.SetActive(true);
        hudCanvas.GetComponent<UIManager>().ShowStartGameScreen();
        player.GetComponent<Player>().currentLive = 100;
    }
}
