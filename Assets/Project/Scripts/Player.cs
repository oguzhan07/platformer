using UnityEngine;


public class Player : MonoBehaviour
{
    public float health;
    public float damage = 10;
    public HealthBar healthBar;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private int maxHealth;

    private bool onGround;
    private float horizontal;
    private int[] listHashCodes = new[] {ATTACK_HASH_1, ATTACK_HASH_2};
    private bool isPressedW;
    private bool isFalled = false;
    private int currentAnimation;
    private bool isAttacking;
    private int attackAnimationHash;


    private Rigidbody2D rb = null;
    private Animator animator = null;
    private SpriteRenderer sprite = null;

    private static readonly string AnimationNameRun = "Run";
    private static readonly string AnimationNameGuard = "Guard";
    private static readonly string AnimationNameSplash = "Splash";
    private static readonly string AnimationNameAttack1 = "Attack1";
    private static readonly string AnimationNameAttack2 = "Attack2";
    private static readonly string AnimationNameIdle = "Idle";


    private static readonly int RUN_HASH = Animator.StringToHash(AnimationNameRun);
    private static readonly int GUARD_HASH = Animator.StringToHash(AnimationNameGuard);
    private static readonly int SPLASH_HASH = Animator.StringToHash(AnimationNameSplash);
    private static readonly int ATTACK_HASH_1 = Animator.StringToHash(AnimationNameAttack1);
    private static readonly int ATTACK_HASH_2 = Animator.StringToHash(AnimationNameAttack2);
    private static readonly int IDLE_HASH = Animator.StringToHash(AnimationNameIdle);

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
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.W))
        {
            isPressedW = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            StartAttack();
        }

        Fall();
        CheckAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ChangeAnimation(int animationHash, float duration = 0.2f)
    {
        if (currentAnimation != animationHash)
        {
            currentAnimation = animationHash;
            animator.CrossFade(currentAnimation, duration);
        }
    }

    private void CheckAnimation()
    {
        if (isFalled)
        {
            ChangeAnimation(SPLASH_HASH);
        }

        else if (isAttacking)
        {
            ChangeAnimation(attackAnimationHash, 0.05f);
        }

        else if (Input.GetKey(KeyCode.Mouse1))
        {
            ChangeAnimation(GUARD_HASH);
        }

        else if (rb.linearVelocityX != 0)
        {
            ChangeAnimation(RUN_HASH);
        }

        else
        {
            ChangeAnimation(IDLE_HASH);
        }
    }


    private void StartAttack()
    {
        isAttacking = true;
        attackAnimationHash = AttackType();
    }

    public void OnAttackHit()
    {
        Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, attackDistance, enemyLayerMask);
        if (enemyCollider && enemyCollider.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(damage);
        }
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    private int AttackType()
    {
        int currentFightTypes = listHashCodes[Random.Range(0, 2)];
        return currentFightTypes;
    }


    public void DamageToPlayer(float amount)
    {
        cameraShake.Shake();
        health -= amount;
        if (health > 0)
        {
            print(health);
            healthBar.Bar(health/maxHealth);
            return;
        }
        healthBar.Bar(health/maxHealth);
        Destroy(gameObject);
    }


    private void Fall()
    {
        if (gameObject.transform.position.y < -3.5 && !isFalled)
        {
            isFalled = true;
            rb.gravityScale = 0f;
            healthBar.Bar(0);
            rb.linearVelocity = new Vector2(0, 0);
            Destroy(gameObject, 1.2f);
        }
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocityY);
        if (horizontal > 0)
        {
            sprite.flipX = false;
        }

        if (horizontal < 0)
        {
            sprite.flipX = true;
        }

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
            onGround = true;
        }
    }
}