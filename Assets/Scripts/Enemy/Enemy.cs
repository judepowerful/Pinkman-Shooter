using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("移动设置")]
    [Tooltip("Enemy chasing speed.")]
    [SerializeField] [Min(0f)] private float moveSpeed = 2f;

    [Header("生命值设置")]
    [Tooltip("Max health of the enemy.")]
    [SerializeField] [Min(1)] private int maxHealth = 3;

    [Header("停止距离")]
    [Tooltip("Enemy stops approaching player when within this distance.")]
    [SerializeField] [Range(0f, 10f)] private float stopDistance = 1f;

    [Header("樱桃掉落")]
    [SerializeField] private GameObject cherryPrefab; // 拖入你的 prefab

    public float MoveSpeed => moveSpeed;
    public int MaxHealth => maxHealth;
    public float StopDistance => stopDistance;

    protected Transform player;
    protected Rigidbody2D rb;
    protected int currentHealth;
    protected SpriteRenderer sr;
    public bool isDead = false;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;

        // 锁定刚体旋转，防止敌人翻转
        rb.freezeRotation = true;
    }

    protected virtual void Update()
    {
        if (player != null && !isDead)
        {
            MoveTowardsPlayer();
            UpdateFacingAndAnimation();
            AttackPlayer();
        }
    }
    
    protected virtual void MoveTowardsPlayer()
    {
        // 到底stopDistance距离时停止移动
        if (Vector2.Distance(transform.position, player.position) <= stopDistance)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        float dx = player.position.x - transform.position.x;
        Vector2 newVel = new Vector2(Mathf.Sign(dx) * moveSpeed, rb.velocity.y);
        rb.velocity = newVel;
    }

    protected virtual void UpdateFacingAndAnimation()
    {
        // Optional: default does nothing
    }

    public virtual void TakeDamage(int amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        UpdateDamageTint();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void UpdateDamageTint()
    {
        if (sr == null) return;
        float healthRatio = (float)currentHealth / maxHealth;
        sr.color = Color.Lerp(Color.gray, Color.white, healthRatio);
    }

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        // 停止所有行为
        rb.velocity = Vector2.zero;
        // 掉落樱桃
        if (cherryPrefab != null)
        {
            Instantiate(cherryPrefab, transform.position, Quaternion.identity);
        }
        // 随机方向跳起（模拟死亡反弹）
        Vector2 deathForce = new Vector2(Random.Range(-1f, 1f), 1f).normalized * 5f;
        rb.AddForce(deathForce, ForceMode2D.Impulse);

        // 让敌人“平躺”在地上：Z轴旋转 ±90 度
        float direction = Random.value < 0.5f ? 1f : -1f;
        transform.rotation = Quaternion.Euler(0f, 0f, direction * 90f);

        // 镜像Y轴以获得贴地感（翻身朝下）
        Vector3 scale = transform.localScale;
        scale.y = -Mathf.Abs(scale.y);
        transform.localScale = scale;

        Animator animator = GetComponent<Animator>();
        StartCoroutine(WaitForDeathAnimationThenFreeze(animator));
        
        // 设置标签为尸体
        gameObject.tag = "Corpse";

        // ✅ 可选：关闭碰撞触发器（只保留落地碰撞）
        Collider2D col = GetComponent<Collider2D>();
        if (col) col.isTrigger = false;

        this.enabled = false;
        // ✅ 不销毁 GameObject，让尸体留在场景
        // Destroy(gameObject); // ❌ 删除这行
    }

    private IEnumerator WaitForDeathAnimationThenFreeze(Animator anim)
    {
        if (anim == null) yield break;

        // 等当前动画状态播放完
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        float waitTime = state.length;

        yield return new WaitForSeconds(waitTime);

        anim.enabled = false;
    }

    protected abstract void AttackPlayer();
}
