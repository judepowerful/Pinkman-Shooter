using UnityEngine;

public class Carbine : Weapon
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private float bulletSpreadAngle = 1f; // 子弹散射角度（±）
    [SerializeField] private float shakeDuration = 0.1f; // 震动持续时间
    [SerializeField] private float shakeMagnitude = 0.1f; // 震

    public override void Shoot(Vector2 aimDir)
    {
        float spread = Random.Range(-bulletSpreadAngle, bulletSpreadAngle);
        Vector2 spreadDir = Quaternion.Euler(0, 0, spread) * aimDir.normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, spreadDir);
        Instantiate(bulletPrefab, firePoint.position, rotation);

        // ➤ 生成 muzzle flash
        Instantiate(muzzleFlashPrefab, firePoint.position, rotation, transform);

        CameraShake.Instance.Shake(shakeDuration, shakeMagnitude); // 震动效果
    }
}
