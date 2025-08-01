using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("武器初始设定")]
    public GameObject initialWeaponPrefab; // 初始武器预设
    public Transform holdingPosition;

    private PlayerController playerController;
    public Weapon currentWeapon;
    private bool facingRight = true;


    private void Start()
    {
        // 初始化武器
        EquipWeapon(initialWeaponPrefab.GetComponent<Weapon>());
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (currentWeapon == null) return;

        facingRight = playerController.facingRight;

        HandleAim();
        HandleWeaponPosition();
        HandleWeaponFiring();
    }

    /// <summary>
    /// 处理瞄准
    /// 根据鼠标位置调整武器朝向
    /// </summary>
    private void HandleAim()
    {
        Vector3 mouseWorld = playerController.mouseWorld;
        Vector3 scale = currentWeapon.transform.localScale;
        scale.y = (facingRight ? 1 : -1) * Mathf.Abs(scale.y);
        currentWeapon.transform.localScale = scale;
        Vector2 aimDir = (mouseWorld - currentWeapon.transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        currentWeapon.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// 处理武器位置
    /// 将武器放置在持有位置，并根据角色朝向调整位置
    /// </summary>
    private void HandleWeaponPosition()
    {
        Vector3 localPos = holdingPosition.localPosition;
        currentWeapon.transform.localPosition = new Vector2(
            facingRight ? localPos.x : -localPos.x,
            localPos.y
        );
    }

    /// <summary>
    /// 处理武器开火
    /// 根据武器类型和输入触发射击
    /// </summary>
    private void HandleWeaponFiring()
    {
        bool isFiring = currentWeapon.isAuto
            ? Input.GetMouseButton(0)
            : Input.GetMouseButtonDown(0);

        if (isFiring)
        {
            Vector2 aimDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - currentWeapon.transform.position).normalized;
            currentWeapon.TryFire(aimDir);
            ApplyRecoilToPlayer(aimDir);
        }
    }

    /// <summary>
    /// 装备武器
    /// 实例化并设置当前武器
    /// </summary>
    public void EquipWeapon(Weapon weaponPrefab)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        currentWeapon = Instantiate(weaponPrefab, transform);
    }

    /// <summary>
    /// 应用后坐力到玩家
    /// 根据武器朝向施加力
    /// </summary>
    private void ApplyRecoilToPlayer(Vector2 aimDir)
    {
        if (playerController == null) return;
        Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // 计算后坐力方向
        // 这里假设后坐力是水平的，向相反方向施加
        Vector2 recoilDir = new Vector2(-aimDir.normalized.x, 0f).normalized;
        rb.AddForce(recoilDir * currentWeapon.recoilForce, ForceMode2D.Impulse);
    }
}
