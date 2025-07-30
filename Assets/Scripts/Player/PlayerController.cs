using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 8f;
    public float airControlFactor = 0.3f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private bool facingRight = true; // 👈 新增

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");

        float targetSpeed = IsGrounded() ? moveSpeed : moveSpeed * airControlFactor;
        float velocityX = Mathf.Lerp(rb.velocity.x, x * targetSpeed, 0.1f);
        rb.velocity = new Vector2(velocityX, rb.velocity.y);

        // ✅ 只在 x 有输入时改变方向
        if (x > 0)
            facingRight = true;
        else if (x < 0)
            facingRight = false;

        // ✅ 翻转角色图像
        sr.flipX = !facingRight;

        // ✅ 跳跃
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        // ✅ 动画参数
        anim.SetBool("isRunning", x != 0);
        anim.SetBool("isGrounded", IsGrounded());
        anim.SetFloat("velocityY", rb.velocity.y);

        // ✅ 同步相机朝向（只在有方向变动时才更新）
        Camera.main.GetComponentInParent<CameraFollow>().SetFacingDirection(facingRight);
    }

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
