using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public EnemyState enemyState;


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

    private float health;
    private float moveSpeed;
    private float followDistance;
    private float attackDistance;
    private SpriteRenderer renderer;

    
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

        arrow = GameObject.Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        
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
        
        arrow.transform.position += new Vector3(arrowSpeed, 0, 0);
    }

    private float ArrowPath()
    {
        // okun güzergahında kaldım. fonksiyon ile güzergahı
        // çizdireceğim. go to giib şeylere bakıp, gidiş yolunu verme
        // işine de bakabilirim. aklıma ilk gelen şey fonksiyonun x
        // değerleri döndürmesi ve okunn bunları her framde
        // input olarak alması oldu.
        
        return 1;
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