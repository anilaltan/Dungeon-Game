using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Image healthBar;
    public Text scoreText;
    public Text titleText;
    public Text gameNameText;
    public Text winText;
    public Button playButton;
    public Button restartButton;
    public Joystick joystick;
    public Button attackBtn;
    private int score;
    private static UIManager instance;
    private GameManager _gameManager;

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Button btn = playButton.GetComponent<Button>();
        Button resbtn = restartButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        resbtn.onClick.AddListener(ResOnClick);
        ShowStartGameScreen();
        HideTitleScreen();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 18)
        {
            ShowWinScreen();
            GameObject.Find("Player").SetActive(false);
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateLives(float currentLives)
    {
        float maxLive = 100f;
        healthBar.fillAmount = currentLives / maxLive;
    }

    public void UpdateScore()
    {
        score += 10;
        scoreText.text = "Coin: " + score;
    }

    public void ShowTitleScreen()
    {
        joystick.gameObject.SetActive(false);
        attackBtn.gameObject.SetActive(false);
        titleText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void ShowWinScreen()
    {
        joystick.gameObject.SetActive(false);
        attackBtn.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(true);
        winText.gameObject.SetActive(true);
    }

    public void HideTitleScreen()
    {
        titleText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }
    
    public void ShowStartGameScreen()
    {
        gameNameText.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
    }

    public void HideStartGameScreen()
    {
        attackBtn.gameObject.SetActive(true);
        joystick.gameObject.SetActive(true);
        gameNameText.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
    }

    void TaskOnClick()
    {
        HideStartGameScreen();
        scoreText.text = "Coin: 0";
        healthBar.fillAmount = 100f;
        _gameManager.StartGame();
    }
    void ResOnClick()
    {
        HideTitleScreen();
        this.gameObject.SetActive(false);
        score = 0;
        _gameManager.RestartGame();
    }

    public void GameOver()
    {
        ShowTitleScreen();
    }

    public void RestartGame()
    {
        ShowStartGameScreen();
        _gameManager.RestartGame();
    }

    public void DecreaseCoin()
    {
        score -= 100;
        scoreText.text = "Coin: " + score;
    }

    public int GetCoin()
    {
        return score;
    }
}
