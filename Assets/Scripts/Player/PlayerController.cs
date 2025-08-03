using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings / 移动设置")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float airControlFactor = 0.3f;

    [Header("Health Settings / 生命值设置")]
    [SerializeField] private int maxHealth = 20;
    public int currentHealth;

    [Header("Ground Detection / 地面检测")]
    public Transform groundCheck;

    [Header("UI Settings / UI设置")]
    public SegmentedHealthBar healthBarUI; // 拖入 UI 控件

    private float groundCheckRadius = 0.1f;
    private LayerMask groundLayer;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public Vector3 mouseWorld;
    
    // 输入
    private float moveInput = 0f;
    private bool jumpRequested = false;

    // 后坐力
    private float recoilVelocity = 0f;
    [SerializeField] private float recoilDecay = 10f; // 衰减速度（单位/秒）


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
        healthBarUI.Init(maxHealth);
        healthBarUI.UpdateHealth(currentHealth);

        rb.freezeRotation = true;
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        HandleFacingDirection();
        UpdateAnimator();
        UpdateCameraMouseFollow();

        moveInput = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
            jumpRequested = true;
    }

    private void FixedUpdate()
    {
        // 这里可以添加物理相关的处理
        HandleMovement();
        HandleJump();
    }

    /// <summary>
    /// 处理角色朝向
    /// 根据鼠标位置调整角色朝向
    /// </summary>
    private void HandleFacingDirection()
    {
        mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        facingRight = mouseWorld.x >= transform.position.x;
        sr.flipX = !facingRight;
    }

    /// <summary>
    /// 处理角色移动
    /// 根据输入调整水平速度
    /// </summary>
    private void HandleMovement()
    {
        float targetSpeed = IsGrounded() ? moveSpeed : moveSpeed * airControlFactor;
        float moveVelocityX = moveInput * targetSpeed;

        // ✅ 加上 recoilVelocity
        float finalVelocityX = moveVelocityX + recoilVelocity;
        rb.velocity = new Vector2(finalVelocityX, rb.velocity.y);

        // ✅ recoilVelocity 每帧慢慢衰减为 0
        recoilVelocity = Mathf.MoveTowards(recoilVelocity, 0f, recoilDecay * Time.fixedDeltaTime);
    }

    /// <summary>
    /// 添加后坐力速度
    /// 通过外部调用来添加后坐力
    /// 例如：在武器射击时调用此方法
    /// </summary>
    /// <param name="recoil"></param>
    public void AddRecoilVelocity(float recoil)
    {
        recoilVelocity = recoil;
    }

    /// <summary>
    /// 处理角色跳跃
    /// 根据输入和地面检测添加跳跃力
    /// </summary>
    private void HandleJump()
    {
        if (jumpRequested && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        // ✅ 清除跳跃请求（即使跳不起来也要清除）
        jumpRequested = false;
    }

    /// <summary>
    /// 更新动画状态
    /// 根据水平输入和是否在地面上设置动画参数
    /// </summary>
    private void UpdateAnimator()
    {
        anim.SetBool("isRunning", moveInput != 0 && IsGrounded());
        anim.SetBool("isGrounded", IsGrounded());
        anim.SetFloat("velocityY", rb.velocity.y);
    }

    /// <summary>
    /// 更新相机朝向
    /// 根据角色朝向设置相机跟随方向
    /// </summary>
    private void UpdateCameraMouseFollow()
    {
        Camera.main.GetComponentInParent<CameraFollow>().SetMouseWorldPosition(mouseWorld);
    }

    /// <summary>
    /// 检测角色是否在地面上
    /// 使用重叠圆检测地面
    /// </summary>
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    /// <summary>
    /// 处理玩家受到伤害
    /// </summary>
    /// <param name="hit">击中信息</param>
    /// <param name="attackerPosition">攻击者位置</param>
    public void TakeDamage(HitInfo hit, Vector2 attackerPosition)
    {
        // 减少生命值
        currentHealth -= hit.damage;

        // 更新 UI
        healthBarUI.UpdateHealth(currentHealth);

        // 播动画
        anim.SetTrigger("hit");

        // 击退方向根据攻击者位置 & hit.knockbackDirection
        Vector2 dir = ((Vector2)transform.position - attackerPosition).normalized;
        Vector2 knockback = new Vector2(Mathf.Sign(dir.x) * hit.knockbackDirection.x, hit.knockbackDirection.y);
        rb.velocity = Vector2.zero;
        rb.AddForce(knockback * hit.knockbackForce, ForceMode2D.Impulse);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 处理玩家死亡
    /// 播放死亡动画并执行其他死亡逻辑
    /// </summary>
    private void Die()
    {
        anim.SetTrigger("die");
        // 这里可以添加更多死亡逻辑，比如重置场景等
        Debug.Log("Player has died.");
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
