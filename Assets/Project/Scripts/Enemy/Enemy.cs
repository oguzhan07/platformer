using System.Collections;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemyState enemyState;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private Player playerScript;
    private GoldManager goldManager;


    private Rigidbody2D rb;
    private Animator animator;
    public GameObject leftEmpty;
    public GameObject rightEmpty;
    public GameObject player = null;
    
    private GameObject arrow;
    public GameObject arrowPrefab;
    public EnemyDamageText enemyDamageText;
    
    
    public int moveDir = 1;
    private bool run;
    private bool attack;
    public float arrowSpeed = 0.5f;
    private float xCoor;
    private float yCoor;

    public float damage;
    private float health;
    private float moveSpeed;
    private float followDistance;
    private float attackDistance;
    private float splashDistance = -3.5f;
    private bool isFalled;
    private SpriteRenderer renderer;
    public bool isAttacking;
    private int currentAnimation;


    private static readonly string AnimationNameRun = "Run";
    private static readonly string AnimationNameAttack = "Attack";
    private static readonly string AnimationNameSplash = "Splash";

    private static readonly int RUN_HASH = Animator.StringToHash(AnimationNameRun);
    private static readonly int ATTACK_HASH = Animator.StringToHash(AnimationNameAttack);
    private static readonly int SPLASH_HASH = Animator.StringToHash(AnimationNameSplash);

    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followDistance);
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //playerScript = player.GetComponent<Player>();
    }

    private void Start()
    {
        name = enemyType.enemyName;
        damage = enemyType.enemyDamage;
        health = enemyType.enemyHealth;
        moveSpeed = enemyType.enemyMoveSpeed;
        followDistance = enemyType.enemyFollowDistance;
        attackDistance = enemyType.enemyAttackDistance;
        renderer.sprite = enemyType.enemySprite;
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Attack:
                Attack();
                break;

            case EnemyState.Follow:
                Follow();
                break;
            
            case EnemyState.Splash:
                Splash();
                break;
        }
    }

    private void Splash()
    {
        if (!isFalled)
        {
            isFalled = true;
            ChangeAnimation(SPLASH_HASH, 0.05f);
            rb.linearVelocity = new Vector2(0, 0);
            rb.gravityScale = 0f;
            Destroy(gameObject, 1.2f);
        }
    }


    private void ChangeAnimation(int hash, float duration = 0.2f)
    {
        if (currentAnimation != hash)
        {
            currentAnimation = hash;
            animator.CrossFade(currentAnimation, duration);
        }    
    }
    
    private void Attack()
    {
        
        if (transform.position.y <= splashDistance)
        {
            isAttacking = false;
            enemyState = EnemyState.Splash;
            return;
        }
        
        if (isAttacking)
        {
            FacePlayer();
            return;
        }

        if (!player)
        {
            enemyState = EnemyState.Patrol;
            return;
        }

        FacePlayer();

        if (Mathf.Abs(player.transform.position.x - transform.position.x) >= attackDistance)
        {
            enemyState = EnemyState.Patrol;
            return;
        }

        ChangeAnimation(ATTACK_HASH, 0.05f);
        isAttacking = true;
    }
    
    private void FacePlayer()
    {
        if (!player) return;
        renderer.flipX = player.transform.position.x < transform.position.x;
    }


    public void OnAttackHit()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackDistance, playerLayerMask);
        if (playerCollider && playerCollider.TryGetComponent(out Player player))
        {
            playerScript.DamageToPlayer(damage);
        }
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    private void Follow()
    {
        
        if (transform.position.y <= splashDistance)
        {
            enemyState = EnemyState.Splash;
        }
        
        if (!player)
        {
            enemyState = EnemyState.Patrol;
        }
        
        if (player)
        {
            if (player.transform.position.x - transform.position.x < 0)
            {
                renderer.flipX = true;
            }

            if (player.transform.position.x - transform.position.x > 0)
            {
                renderer.flipX = false;
            }
            
            ChangeAnimation(RUN_HASH, 0.2f);
            rb.linearVelocity =
                new Vector2((player.transform.position.x - transform.position.x) * moveSpeed, rb.linearVelocityY)
                    .normalized;

            
            
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < attackDistance)
            {
                enemyState = EnemyState.Attack;
            }

            if (Mathf.Abs(player.transform.position.x - transform.position.x) > followDistance)
            {
                enemyState = EnemyState.Patrol;
            }
        }
    }

    private void Patrol()
    {
        rb.linearVelocity = new Vector2(moveDir * moveSpeed, rb.linearVelocityY);
        /*animator.SetBool("Run", true);
        animator.SetBool("Attack", false);*/
        ChangeAnimation(RUN_HASH, 0.2f);
        
        if (transform.position.y <= splashDistance)
        {
            enemyState = EnemyState.Splash;
        }
        
        if (moveDir < 0)
        {
            print("sola gidiyor");
            renderer.flipX = true;
        }

        if (moveDir > 0)
        {
            print("sağa gidiyor");
            renderer.flipX = false;
        }

        /*if (transform.position.x < leftEmpty.transform.position.x)
        {
            rb.linearVelocity = new Vector2((rightEmpty.transform.position.x - transform.position.x) * moveSpeed,
                rb.linearVelocityY).normalized;
            renderer.flipX = false;
        }
        if (transform.position.x > rightEmpty.transform.position.x)
        {
            rb.linearVelocity = new Vector2((leftEmpty.transform.position.x - transform.position.x) * moveSpeed,
                rb.linearVelocityY).normalized;
            renderer.flipX = true;
        }*/
        if (player)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) < followDistance)
            {
                enemyState = EnemyState.Follow;
            }
        }
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Point"))
        {
            moveDir *= -1;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        enemyDamageText.EnemyGetDamage(amount);
        print("enemy health: " + health);
        StartCoroutine(EnemyColorChange());
        if (health > 0)
            return;
        
        Destroy(gameObject);
    }

    IEnumerator EnemyColorChange()
    {
        Color originalColor = renderer.color;
        renderer.color = new Color(1f, 0, 0, 0.8f);
        yield return new WaitForSeconds(0.05f);
        renderer.color = originalColor;
    }
    

    /*private void DamagePolish()
    {
        StartCoroutine(DamagePolishDelay());
    }

    IEnumerator DamagePolishDelay()
    {
        
        yield return new WaitForSeconds(0.2f);
    }*/
}

// Enum'lar inspector'da bir değişkenin seçilebilir değerlerinin listelendiği yapı. 
public enum EnemyState
{
    Patrol,
    Follow,
    Attack,
    Splash,
}