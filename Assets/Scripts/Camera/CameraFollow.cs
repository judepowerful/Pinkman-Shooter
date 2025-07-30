using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // 玩家
    public float smoothSpeed = 5f;     // Lerp 速度
    public Vector2 offset = new Vector2(0f, 1.5f); // 基础偏移（让相机稍微高一点）
    public float lookAheadDistance = 2f;  // 面朝方向偏移距离
    public float lookAheadSmoothing = 2f;

    private Vector3 currentVelocity;
    private float currentLookAhead = 0f;
    private bool facingRight = true;

    void LateUpdate()
    {
        if (target == null) return;

        // 获取目标位置 + 上下偏移
        Vector3 baseTargetPos = target.position + (Vector3)offset;

        // === 处理 Look Ahead ===
        float lookAheadTarget = facingRight ? lookAheadDistance : -lookAheadDistance;
        currentLookAhead = Mathf.Lerp(currentLookAhead, lookAheadTarget, Time.deltaTime * lookAheadSmoothing);

        baseTargetPos.x += currentLookAhead;

        // 平滑跟随
        Vector3 desiredPos = baseTargetPos;
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, desiredPos, ref currentVelocity, 1f / smoothSpeed);
        transform.position = new Vector3(smoothPos.x, smoothPos.y, transform.position.z);
    }

    public void SetFacingDirection(bool isFacingRight)
    {
        facingRight = isFacingRight;
    }
}
