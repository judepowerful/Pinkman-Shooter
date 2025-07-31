using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private float bulletSpreadAngle = 5f; // 子弹散射角度（±）
    [SerializeField] private float shakeDuration = 0.1f; // 震动持续时间
    [SerializeField] private float shakeMagnitude = 0.1f; // 震

    public override void Shoot(Vector2 aimDir)
    {
        float spread = Random.Range(-bulletSpreadAngle, bulletSpreadAngle);
        Vector2 spreadDir = Quaternion.Euler(0, 0, spread) * aimDir.normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, spreadDir);
        Instantiate(bulletPrefab, firePoint.position, rotation);

        // ➤ 生成 muzzle flash
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, firePoint.position, rotation, transform);
        Destroy(muzzleFlash, 0.1f); // 短暂存在

        CameraShake.Instance.Shake(shakeDuration, shakeMagnitude); // 震动效果
    }
}
