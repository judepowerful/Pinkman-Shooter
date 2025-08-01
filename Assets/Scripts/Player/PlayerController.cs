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
    
    [HideInInspector]
    public bool facingRight = true; // ✅ 由 GunController 控制角色翻转
    public Vector3 mouseWorld; // ✅ 用于获取鼠标位置

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
        HandleMovement();
        HandleJump();
        UpdateAnimator();
        UpdateCameraMouseFollow();
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
        float xInput = Input.GetAxisRaw("Horizontal");
        float targetSpeed = IsGrounded() ? moveSpeed : moveSpeed * airControlFactor;
        float velocityX = Mathf.Lerp(rb.velocity.x, xInput * targetSpeed, 0.1f);
        rb.velocity = new Vector2(velocityX, rb.velocity.y);
    }

    /// <summary>
    /// 处理角色跳跃
    /// 根据输入和地面检测添加跳跃力
    /// </summary>
    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// 更新动画状态
    /// 根据水平输入和是否在地面上设置动画参数
    /// </summary>
    private void UpdateAnimator()
    {
        float xInput = Input.GetAxisRaw("Horizontal");

        anim.SetBool("isRunning", xInput != 0 && IsGrounded());
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
