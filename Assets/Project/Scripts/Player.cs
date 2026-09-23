using System;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private LayerMask enemyLayerMask;
    public HealthBar healthBar;
    public float endValue = 0.5f;
    private GoldManager goldManager;

    public float health;
    [SerializeField] private int maxHealth;

    
    private bool onGround;
    private float horizontal;
    private float vertical;
    private bool splash = false;
    public float guard;
    private float attack;
    private int[] listHashCodes = new[] {ATTACK_HASH_1, ATTACK_HASH_2};
    private bool isAttackReady = true;
    public float damage = 10;
    public bool attackAnimation;
    private bool isPressedW;
    private bool isFalled = false;
     

    private Rigidbody2D rb = null;
    private Animator animator = null;
    private SpriteRenderer sprite = null;
    [SerializeField] private CameraShake cameraShake;

    private static readonly string AnimationNameSpeed = "Speed";
    private static readonly string AnimationNameGuard = "Guard";
    private static readonly string AnimationNameSplash = "Splash";
    private static readonly string AnimationNameAttack1 = "Attack1";
    private static readonly string AnimationNameAttack2 = "Attack2";


    private static readonly int SPEED_HASH = Animator.StringToHash(AnimationNameSpeed);
    private static readonly int GUARD_HASH = Animator.StringToHash(AnimationNameGuard);
    private static readonly int SPLASH_HASH = Animator.StringToHash(AnimationNameSplash);
    private static readonly int ATTACK_HASH_1 = Animator.StringToHash(AnimationNameAttack1);
    private static readonly int ATTACK_HASH_2 = Animator.StringToHash(AnimationNameAttack2);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        // Fizik hariç diğer her şey Update'de.
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.W))
        {
            isPressedW = true;
        }
        
        Attack();
        Animation();
        Fall();
    }

    private void FixedUpdate()
    {
        // Fizik işlemleri FixedUpdate'de.
        Move();
    }

    public void DamageToPlayer(float amount)
    {
        healthBar.Bar(health/100);
        cameraShake.Shake();
        health -= amount;
        if (health >= 0)
        {
            return;
        }
        Destroy(gameObject);
    }
    
    
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
            Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, attackDistance, enemyLayerMask);
            if (enemyCollider)
            {
                if (enemyCollider.TryGetComponent(out Enemy enemy))
                {
                    if (isAttackReady)
                    {
                        StartCoroutine(AttackDelay(enemy));
                    }
                }
            }
        }
    }

    

    IEnumerator AttackDelay(Enemy enemy)
    {
        isAttackReady = false;
        enemy.TakeDamage(damage);
        yield return new WaitForSeconds(0.3f);
        isAttackReady = true;
    }
    

    private int AttackType()
    {
        int currentFightTypes = listHashCodes[UnityEngine.Random.Range(0, 2)];
        return currentFightTypes;
    }

    private void Animation()
    {
        animator.SetFloat(SPEED_HASH, Mathf.Abs(horizontal));
        animator.SetFloat(GUARD_HASH, guard);
        animator.SetBool(SPLASH_HASH, splash);
        animator.SetBool(AttackType(), attackAnimation);

    }


    private void Fall()
    {
        if (gameObject.transform.position.y < -3.5 && !isFalled)
        {
            // Destroy(gameObject) yaptığım zaman animasyon oynamaya fırsat bulamadan karakter siliniyor.
            // Problemi bu şekilde çözdüm ama bu durumda da karakter yok olmuyor.
            // Karakteri yok ederek animasyonun çalışmasını nasıl sağlarım ?
            // Animasyonlar aynı anda nasıl çalıştırlır ?
            
            isFalled = true;
            Physics2D.gravity = new Vector2(0, -0.5f);
            healthBar.Bar(0);
            splash = true;
            rb.linearVelocity = new Vector2(0, 0);
            Destroy(gameObject, 1.2f);
        }
    }
    
    private void Move()
    {
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocityY);
        sprite.flipX = horizontal < 0;

        if (isPressedW && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpSpeed);
            onGround = false;
            isPressedW = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            print("oyuncu yerde");
            onGround = true;
        }
    }
}
    