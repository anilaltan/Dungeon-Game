using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float range;
    [SerializeField]
    private Transform healthBar;
    private Transform target;
    private float healthWidth;
    private float minDistance = 5.0f;
    private bool targetCollision = false;
    private float speed = 2.0f;
    private float thrust = 1.5f;
    [SerializeField]
    private float maxHealth = 100;
    private float currentHealth;
    private bool isDead = false;

    private UIManager _uiManager;

    [SerializeField]
    private GameObject[] powerups;
    private GameManager _gameManager;
    private GatewayManager _gatewayManager;
    public Sprite deathSprite;
    public Sprite[] sprites;
    void Start()
    {
        currentHealth = maxHealth;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gatewayManager = GameObject.Find("GatewayManager").GetComponent<GatewayManager>();
        healthWidth = healthBar.localScale.x;
        target = GameObject.Find("Player").transform;
        int rnd = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[rnd];
    }

    void Update()
    {
        range = Vector2.Distance(transform.position, target.position);
        if (range < minDistance && !isDead)
        {
            if (!targetCollision)
            {
                transform.LookAt(target.position);
                transform.Rotate(new Vector3(0, -90, 0), Space.Self);
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
            }
        }
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !targetCollision)
        {
            
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 center = collision.collider.bounds.center;

            targetCollision = true;

            bool right = contactPoint.x > center.x;
            bool left = contactPoint.x < center.x;  
            bool top = contactPoint.y > center.y;
            bool bottom = contactPoint.y < center.y;

            if (right)
            {
                GetComponent<Rigidbody2D>().AddForce(transform.right * thrust, ForceMode2D.Impulse);
            }
            if (left)
            {
                GetComponent<Rigidbody2D>().AddForce(-transform.right * thrust, ForceMode2D.Impulse);
            }
            if (top)
            {
                GetComponent<Rigidbody2D>().AddForce(transform.up * thrust, ForceMode2D.Impulse);
            }
            if (bottom)
            {
                GetComponent<Rigidbody2D>().AddForce(-transform.up * thrust, ForceMode2D.Impulse);
            }
            Invoke("FalseCollision", 0.5f);
        }
    }

    private void FalseCollision()
    {
        targetCollision = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("enemy health: " + currentHealth);
        if (currentHealth <= 0)
        {
            isDead = true;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<SpriteRenderer>().sprite = deathSprite;
            GetComponent<SpriteRenderer>().sortingOrder = -1;
            GetComponent<Collider2D>().enabled = false;
            Destroy(transform.GetChild(0).gameObject);
            Destroy(transform.GetChild(1).gameObject);
            _uiManager.UpdateScore();
            Invoke("EnemyDeath", 1.5f);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Invoke("HideBlood", 0.25f);
            Vector3 temp = new Vector3(healthWidth * (currentHealth / maxHealth), 0.1f, 1);
            healthBar.localScale = temp;
        }
    }

    private void HideBlood()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void EnemyDeath()
    {
        dropRandomItem();
        _gatewayManager.SetEnemyCount(-1);
        Destroy(this.gameObject);
    }

    private void dropRandomItem()
    {
        int randomNum = Random.Range(1, 6);
        if(randomNum == 1)
        {
            Instantiate(powerups[Random.Range(0, powerups.Length)], transform.position, Quaternion.identity);
        }
    }
}
