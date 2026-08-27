using System;
using UnityEngine;
using Random = System.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float attackDistance;
    [SerializeField] private LayerMask enemyLayerMask;

    
    private bool onGround;
    private float horizontal;
    private float vertical;
    private bool splash = false;
    public float guard;
    private float attack;
    private int[] listHashCodes = new[] {ATTACK_HASH_1, ATTACK_HASH_2};
     

    private Rigidbody2D rb = null;
    private Animator animator = null;
    private SpriteRenderer sprite = null;

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

    private void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Fall();
        Move();
        Animation();
        Attack();
        AttackAnimation();
    }

    private void Attack()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // SORU: GetKey ve GetKeyDown. Normalde GetKeyDown kullanmak mantıklı, yoksa düşman direkt ölüyor
            // fakat onu kullanınca da aniamsyonlar sıkıntıya giriyor. Ne yapmak lazım ?
            Collider2D enemyCollider = Physics2D.OverlapCircle(transform.position, attackDistance, enemyLayerMask);
            if (enemyCollider)
            {
                if (enemyCollider.TryGetComponent(out Enemy enemy))
                {
                    enemy.TakeDamage(1);
                }
            }
        }
    }

    private bool AttackAnimation()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            return true;
        }
        return false;
    }

    private int AttackType()
    {
        // PROBLEM: Random sürekli aynı sonucu: 0 döndürüyor. 
        int currentFightTypes = listHashCodes[UnityEngine.Random.Range(0, 1)];
        return currentFightTypes;
    }

    private void Animation()
    {
        animator.SetFloat(SPEED_HASH, Mathf.Abs(horizontal));
        animator.SetFloat(GUARD_HASH, guard);
        animator.SetBool(AttackType(), AttackAnimation());
        animator.SetBool(SPLASH_HASH, splash);
    }

    private void Fall()
    {
        if (gameObject.transform.position.y < -3.5)
        {
            // Destroy(gameObject) yaptığım zaman animasyon oynamaya fırsat bulamadan karakter siliniyor.
            // Problemi bu şekilde çözdüm ama bu durumda da karakter yok olmuyor.
            // Karakteri yok ederek animasyonun çalışmasını nasıl sağlarım ?
            // Animasyonlar aynı anda nasıl çalıştırlır ?

            splash = true;
            rb.linearVelocity = new Vector2(0, 0);
            //Destroy(gameObject, 1.2f);
            //ikinci kisimdaki saniye gectikten sonra yok etme islemini gerceklesecek
        }
    }
    

    private void Move()
    {
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocityY);
        sprite.flipX = horizontal < 0;

        if (Input.GetKey(KeyCode.W) && onGround)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, vertical * jumpSpeed);
            onGround = false;
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