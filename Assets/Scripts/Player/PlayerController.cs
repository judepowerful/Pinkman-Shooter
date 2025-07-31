using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 8f;
    public float airControlFactor = 0.3f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

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

        rb.freezeRotation = true;
    }

    private void Update()
    {
        HandleFacingDirection();
        HandleMovement();
        HandleJump();
        UpdateAnimator();
        UpdateCameraFacing();
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
    private void UpdateCameraFacing()
    {
        Camera.main.GetComponentInParent<CameraFollow>().SetFacingDirection(facingRight);
    }

    /// <summary>
    /// 检测角色是否在地面上
    /// 使用重叠圆检测地面
    /// </summary>
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
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
