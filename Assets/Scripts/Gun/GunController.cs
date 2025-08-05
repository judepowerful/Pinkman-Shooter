using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("武器初始设定")]
    public List<Weapon> weaponPrefabs; // 可装备的武器列表
    public Transform holdingPosition;
    private PlayerController playerController;
    public Weapon currentWeapon;
    private bool facingRight = true;
    private int currentWeaponIndex = 0;
    [SerializeField] private float switchCooldown = 0.3f; // 切枪间隔（秒）
    private float lastSwitchTime = -999f;

    private Vector3 originalWeaponLocalPos;
    private Vector3 recoilVisualOffset;
    [SerializeField] private float recoilReturnSpeed = 20f;
    [SerializeField] private WeaponSwitcherUI weaponSwitcherUI;

    private void Start()
    {
        // 初始化武器
        EquipWeapon(weaponPrefabs[currentWeaponIndex]);
        playerController = GetComponent<PlayerController>();

        originalWeaponLocalPos = holdingPosition.localPosition;
    }

    private void Update()
    {
        if (currentWeapon == null) return;

        facingRight = playerController.facingRight;

        HandleScrollInput();
        HandleAim();
        HandleWeaponPosition();
        HandleWeaponFiring();

        // 平滑回弹视觉 recoil
        recoilVisualOffset = Vector3.Lerp(recoilVisualOffset, Vector3.zero, recoilReturnSpeed * Time.deltaTime);
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
        Vector3 localPos = originalWeaponLocalPos + recoilVisualOffset;
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
            bool fired = currentWeapon.TryFire(aimDir);
            if (fired)
            {
                ApplyRecoilToPlayer(aimDir);

                // 应用视觉后坐力
                float kickDistance = currentWeapon.recoilVisualDistance;
                recoilVisualOffset = Vector3.left * kickDistance;
            }
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

        // 装备武器后通知 UI
        SpreadIndicator spreadUI = FindObjectOfType<SpreadIndicator>();
        if (spreadUI != null && currentWeapon is Carbine c)
        {
            spreadUI.SetCarbine(c);
        }
        else if (spreadUI != null)
        {
            spreadUI.SetCarbine(null); // 如果换成不是 Carbine 的武器，就隐藏 UI
        }
    }

    /// <summary>
    /// 切换武器调整UI
    /// </summary>
    /// <param name="currentIndex">当前武器index</param>
    public void UpdateWeaponUI(int currentIndex)
    {
        if (weaponSwitcherUI != null && currentWeapon != null)
        {
            weaponSwitcherUI.SwitchWeapon(currentIndex);
        }
    }

    /// <summary>
    /// 应用后坐力到玩家
    /// 根据武器朝向施加力
    /// </summary>
    private void ApplyRecoilToPlayer(Vector2 aimDir)
    {
        float direction = -Mathf.Sign(aimDir.x);
        float recoilStrength = currentWeapon.recoilForce;

        playerController.AddRecoilVelocity(direction * recoilStrength);
    }

    /// <summary>
    /// 处理武器切换
    /// 使用鼠标滚轮切换武器
    /// </summary>
    private void HandleScrollInput()
    {
        if (Time.time - lastSwitchTime < switchCooldown) return; // 冷却中，忽略滚动

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            currentWeaponIndex++;
            if (currentWeaponIndex >= weaponPrefabs.Count)
                currentWeaponIndex = 0;

            EquipWeapon(weaponPrefabs[currentWeaponIndex]);
            UpdateWeaponUI(currentWeaponIndex);
            lastSwitchTime = Time.time;
        }
        else if (scroll < 0f)
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
                currentWeaponIndex = weaponPrefabs.Count - 1;

            EquipWeapon(weaponPrefabs[currentWeaponIndex]);
            UpdateWeaponUI(currentWeaponIndex);
            lastSwitchTime = Time.time;
        }
    }
}
