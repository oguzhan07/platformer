using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemyState enemyState;

    public float health;
    private float moveSpeed;

    private SpriteRenderer renderer;
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject leftEmpty;
    public GameObject rightEmpty;
    public GameObject player;

    public int moveDir = 1;
    private bool run;
    private bool attack;
    private float followDistance;
    private float attackDistance;

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
        print("saldırıyorum");

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

    private void Archer()
    {
        
    }

    private void Follow()
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

        if (Mathf.Abs(player.transform.position.x - transform.position.x) < followDistance)
        {
            enemyState = EnemyState.Follow;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Point"))
        {
            moveDir *= -1;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        print(health);
        if (health > 0)
            return;
        
        Destroy(gameObject);
    }
}

// Enum'lar inspector'da bir değişkenin seçilebilir değerlerinin listelendiği yapı. 
public enum EnemyState
{
    Patrol,
    Follow,
    Attack,
}