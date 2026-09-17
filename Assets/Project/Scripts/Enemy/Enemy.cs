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
    private GoldManager goldManager;


    private Rigidbody2D rb;
    private Animator animator;
    public GameObject leftEmpty;
    public GameObject rightEmpty;
    public GameObject player;
    
    private GameObject arrow;
    public GameObject arrowPrefab;
    
    
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
    private SpriteRenderer renderer;
    public bool isAttackReady = true;

    
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
        }

    }

    
    
    private void Attack()
    {
        animator.SetBool("Attack", true);

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackDistance, playerLayerMask);
        if (playerCollider)
        {
            if (playerCollider.TryGetComponent(out Player player))
            {
                if (isAttackReady)
                {
                    StartCoroutine(AttackDelay(player));
                }
            }
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

            if (Mathf.Abs(player.transform.position.x - transform.position.x) >= attackDistance)
            {
                enemyState = EnemyState.Patrol;
            }
        }
    }

    IEnumerator AttackDelay(Player player)
    {
        player.DamageToPlayer(damage);
        isAttackReady = false;
        yield return new WaitForSecondsRealtime(1.2f);
        isAttackReady = true;
    }

    private void Follow()
    {
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
        animator.SetBool("Run", true);
        animator.SetBool("Attack", false);

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
        print("enemy health: " + health);
        if (health > 0)
            return;
        
        Destroy(gameObject);
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
}