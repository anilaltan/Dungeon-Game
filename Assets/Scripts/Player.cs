using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private bool playerMoving;
    private Vector2 lastMove;
    public Joystick joystick;
    public Button attackBtn;
    [SerializeField]
    private float speed = 4.0f;
    private float maxLive = 100f;
    public float currentLive;
    [SerializeField]
    private float strength = 10f;
    private Rigidbody2D _rigidbody;
    private Animator _anim;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private bool isInCoolDown = false;
    private bool damage = false;
    private bool attacking;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    private static Player _instance;
    private bool isShieldOpen = false;
    public AudioClip swordSwing;
    private AudioSource audioSource;
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        Button btn = attackBtn.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        currentLive = maxLive;
        audioSource = GetComponent<AudioSource>();
        if (_uiManager != null)
        {
            _uiManager.UpdateLives(currentLive);
        }
    }

    void Update()
    {
        Movement();
        //if (Time.time >= nextAttackTime)
        //{
        //    Attack();
        //    nextAttackTime = Time.time + 1f / attackRate;
            
        //}
        //else
        //{
        //    _anim.SetBool("Attack", false);
        //    attacking = false;
        //}
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    
    void TaskOnClick()
    {
        if (Time.time >= nextAttackTime)
        {

            if (!isInCoolDown)
            {
                Attack();
                Invoke("ResetCoolDown", 0.5f);
                isInCoolDown = true;
                nextAttackTime = Time.time + 1.5f / attackRate;
            }
        }   
    }

    private void ResetCoolDown()
    {
        isInCoolDown = false;
        _anim.SetBool("Attack", false);
        attacking = false;
    }

    private void Movement()
    {
        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");
        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;
        playerMoving = false;

        if (!attacking)
        {
            if (horizontal > 0.5f || horizontal < -0.5f)
            {
                //transform.Translate(new Vector3(horizontal * speed * Time.deltaTime, 0f, 0f));
                _rigidbody.velocity = new Vector2(horizontal * speed, _rigidbody.velocity.y);
                playerMoving = true;
                lastMove = new Vector2(horizontal, 0f);
            }
            if (vertical > 0.5f || vertical < -0.5f)
            {
                //transform.Translate(new Vector3(0f, vertical * speed * Time.deltaTime, 0f));
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, vertical * speed);
                playerMoving = true;
                lastMove = new Vector2(0f, vertical);
            }
            if (horizontal < 0.5f && horizontal > -0.5f)
            {
                _rigidbody.velocity = new Vector2(0f, _rigidbody.velocity.y);
            }
            if (vertical < 0.5f && vertical > -0.5f)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
            }
        }
        
        _anim.SetFloat("MoveX", horizontal);
        _anim.SetFloat("MoveY", vertical);
        _anim.SetBool("PlayerMoving", playerMoving);
        _anim.SetFloat("LastMoveX", lastMove.x);
        _anim.SetFloat("LastMoveY", lastMove.y);
    }

    private void Attack()
    {
        attacking = true;
        _rigidbody.velocity = Vector2.zero;
        _anim.SetBool("Attack", true);
        audioSource.Play();
        //detect enemies in range of attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        //damage them
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                enemy.GetComponent<Enemy>().TakeDamage(strength);
            }
            if (enemy.gameObject.CompareTag("Spawner"))
            {
                enemy.GetComponent<SpawnManager>().TakeDamage(strength);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Damage();
        }
        else if (collision.gameObject.CompareTag("Spawner"))
        {
            collision.gameObject.GetComponent<SpawnManager>().GetGateway();
        }
    }

    public void Damage()
    {
        if (!isShieldOpen)
        {
            currentLive -= 10f;
            damage = true;
            _anim.SetBool("Damage", damage);
            Invoke("HideBlood", 0.5f);
            _uiManager.UpdateLives(currentLive);
        }

        if (currentLive < 1f)
        {
            _gameManager.gameOver = true;
            _uiManager.GameOver();
            _anim.SetTrigger("IsDead");
            StartCoroutine(WaitForAnim());
            if (_anim.GetCurrentAnimatorStateInfo(0).IsName("Death Pose"))
            {
                //Destroy(this.gameObject);
                this.gameObject.SetActive(false);
                currentLive = maxLive;
            }
        }
    }

    public IEnumerator WaitForAnim()
    {
        yield return new WaitForSeconds(5.0f);
        _anim.ResetTrigger("IsDead");
    }

    private void HideBlood()
    {
        damage = false;
        _anim.SetBool("Damage", damage);
    }

    public void FullHealthdPowerupOn()
    {
        currentLive = maxLive;
        _uiManager.UpdateLives(currentLive);
    }

    public void StrengthPowerupOn()
    {
        strength += 5f;
        Debug.Log(strength);
    }

    public void ShieldPowerupOn()
    {
        isShieldOpen = true;
        StartCoroutine(ShieldPowerDownRoutine());
    }
    public IEnumerator ShieldPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isShieldOpen = false;
    }
}


