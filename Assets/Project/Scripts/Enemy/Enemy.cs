using Unity.Mathematics;
using Unity.VisualScripting;
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
    private float arrowMaxh = 3;
    private float xCoor;
    private float yCoor;

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

        ArrowPath();
    }

    private void ArrowPath()
    {
        // okun güzergahında kaldım. fonksiyon ile güzergahı
        // çizdireceğim. go to giib şeylere bakıp, gidiş yolunu verme
        // işine de bakabilirim. aklıma ilk gelen şey fonksiyonun x
        // değerleri döndürmesi ve okunn bunları her framde
        // input olarak alması oldu.
        // düşmanla aramdaki mesafenin yarısı / hipotenüs = cosx
        // -1 < cosx < 1
        // arccos(x) = derece
        // vector2() ne ister ?: koordinatta bir nokta, x ve y değerleri
        
        
        
        // 1. yay çizilecek
        // 2. yayın güzergahını her frame'de return edecek
        // 3. return'ün çıktısını ok'un hareket fonksiyonu alacak
        // 
        for (float i = 0; i <= 1; i += 0.1f)
        {
            /*float d = Mathf.Sqrt(math.square(player.transform.position.x - transform.position.x) + math.square(player.transform.position.y - transform.position.y));
            xCoor = (1-i) * (transform.position.x) + i * (player.transform.position.x) - 2 * i * (1 - i) * arrowMaxh * (player.transform.position.y - transform.position.y) / d;
            yCoor = (1-i) * (transform.position.y) + i * (player.transform.position.y) - 2 * i * (1 - i) * arrowMaxh * (player.transform.position.x - transform.position.x) / d;
            arrow.transform.position = Vector2.MoveTowards(transform.position, player.transform.position, arrowMaxh);*/
            
            xCoor = transform.position.x + i * (player.transform.position.x - transform.position.x);
            yCoor = arrow.transform.position.y - 4 * arrowMaxh * i * (1 - i);
            arrow.transform.position = new Vector2(xCoor, yCoor);
            print("x:" +xCoor);
            print("y:" +yCoor);
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