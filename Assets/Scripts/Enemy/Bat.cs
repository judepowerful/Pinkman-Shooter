using UnityEngine;

public class Bat : Enemy
{
    private bool facingRight = true;
    private Animator animator;
    private float attackCooldown = 1f;   // 攻击间隔（秒）
    private float attackTimer = 0f;      // 冷却计时器
    public HitInfo hitInfo;
    [SerializeField] private float attackRange = 1f; // 攻击范围

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void MoveTowardsPlayer()
    {
        if (player == null || isDead) return;
        if (Vector2.Distance(transform.position, player.position) <= StopDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * MoveSpeed;
    }

    protected override void UpdateFacingAndAnimation()
    {
        Vector2 toPlayer = player.position - transform.position;

        if (Mathf.Abs(toPlayer.x) > 0.1f)
        {
            facingRight = toPlayer.x > 0f;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (facingRight ? 1 : -1);
            transform.localScale = scale;
        }
    }

    protected override void AttackPlayer()
    {
        // TODO: Bat的攻击方式
        // 靠近之后直接攻击
        if (player == null || isDead) return;

        // 冷却计时器更新
        attackTimer -= Time.deltaTime;
        
        // 距离足够近，且冷却完毕 → 攻击
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= attackRange && attackTimer <= 0f)
        {
            player.GetComponent<PlayerController>().TakeDamage(hitInfo, transform.position);
            // 重置冷却
            attackTimer = attackCooldown;
        }
    }


    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        animator.SetTrigger("hit");
    }
    
    protected override void Die()
    {
        base.Die();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1f;
    }
}
