using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Follow Settings / 相机跟随设置")]
    [Tooltip("目标对象（通常是玩家）")]
    [SerializeField] private Transform target;
    [Tooltip("相机跟随目标的平滑速度")]
    [SerializeField] private float smoothSpeed = 4f;
    [Tooltip("相机跟随目标的偏移量")]
    [SerializeField] private Vector2 offset = new Vector2(0f, 1f);
    [Tooltip("Aiming方向偏移距离")]
    [SerializeField] private float lookAheadDistance = 4f;
    [Tooltip("Aiming方向平滑插值")]
    [SerializeField] private float lookAheadSmoothing = 4f;

    private Vector3 currentVelocity;

    // ✅ 改成 Vector2 以支持任意方向
    private Vector2 currentLookOffset = Vector2.zero;
    private Vector3 mouseWorld;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 baseTargetPos = target.position + (Vector3)offset;

        Vector2 toMouse = (Vector2)(mouseWorld - target.position);

        Vector2 lookOffsetTarget = toMouse.normalized * lookAheadDistance;

        // ✅ 直接在 currentLookOffset 上做 Lerp
        currentLookOffset = Vector2.Lerp(
            currentLookOffset,
            lookOffsetTarget,
            Time.deltaTime * lookAheadSmoothing
        );

        Vector3 finalTarget = baseTargetPos + (Vector3)currentLookOffset;

        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, finalTarget, ref currentVelocity, 1f / smoothSpeed);
        transform.position = new Vector3(smoothPos.x, smoothPos.y, transform.position.z);
    }

    public void SetMouseWorldPosition(Vector3 mouseCameraFollowPosition)
    {
        mouseWorld = mouseCameraFollowPosition;
    }
}
