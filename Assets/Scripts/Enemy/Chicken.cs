using UnityEngine;

public class Chicken : Enemy
{
    [Header("巡逻设置")]
    [SerializeField] private float walkTimeMin = 2f;
    [SerializeField] private float walkTimeMax = 4f;
    [SerializeField] private float waitTimeMin = 1f;
    [SerializeField] private float waitTimeMax = 3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float wallCheckDistance = 0.5f;

    private int moveDir = 1;
    private float timer = 0f;
    private bool isWalking = true;
    private Animator anim;

    protected override void Start()
    {
        base.Start();
        ResetTimer(); // 初始化第一次的计时
        anim = GetComponent<Animator>();
    }

    protected override void AttackPlayer()
    {
        // 鸡不攻击
    }

    protected override void MoveTowardsPlayer()
    {
        if (isWalking)
        {
            anim.SetBool("isRunning", true);
            // 检测前方墙体
            bool hitWall = Physics2D.Raycast(transform.position, Vector2.right * moveDir, wallCheckDistance, groundLayer);
            if (hitWall)
            {
                moveDir *= -1;
                Flip();
            }

            rb.velocity = new Vector2(moveDir * MoveSpeed, rb.velocity.y);
        }
        else
        {
            anim.SetBool("isRunning", false);
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isWalking = !isWalking;
            ResetTimer();
        }
    }

    protected override void UpdateFacingAndAnimation()
    {
        Flip();
    }

    private void ResetTimer()
    {
        timer = isWalking
            ? Random.Range(walkTimeMin, walkTimeMax)
            : Random.Range(waitTimeMin, waitTimeMax);
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * moveDir;
        transform.localScale = scale;
    }

    protected override void Die()
    {
        base.Die();
        ChickenDead.Instance.ShowDeadMessage();
    }
}
