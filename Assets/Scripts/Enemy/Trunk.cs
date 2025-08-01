using UnityEngine;

public class Trunk : Enemy
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float attackCooldown = 2f;
    public float attackRange = 6f;

    private float lastAttackTime = -999f;
    private bool facingRight = true;

    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
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

        animator.SetBool("isRunning", Mathf.Abs(rb.velocity.x) > 0.1f);
    }

    protected override void AttackPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            /// 触发射击动画 ///
            /// Fire Bullet is called from the Animator ///
            /// This method will be called when the shoot animation reaches the point where the bullet should be fired ///
            animator.SetTrigger("shoot");
            lastAttackTime = Time.time;
        }
    }
    public void FireBullet() // Animator 会调用这个
    {
        Vector2 dir = facingRight ? Vector2.right : Vector2.left;
        Quaternion rot = Quaternion.FromToRotation(Vector2.right, dir);
        Instantiate(bulletPrefab, firePoint.position, rot);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        animator.SetTrigger("hit");
    }
}
