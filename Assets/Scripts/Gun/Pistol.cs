using UnityEngine;

public class Pistol : Weapon
{
    [Header("子弹设置")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpreadAngle = 5f; // 子弹散射角度（±）
    [Header("子弹壳设置")]
    [SerializeField] private GameObject bulletShellPrefab;
    [SerializeField] private Transform shellEjectPoint;     // 子弹壳飞出的起点
    [SerializeField] private float shellEjectForce = 2f;    // 弹出力度
    [SerializeField] private float shellTorque = 10f;       // 旋转力度
    [Header("枪口火花设置")]
    [SerializeField] private GameObject muzzleFlashPrefab;
    [Header("震动设置")]
    [SerializeField] private float shakeDuration = 0.1f; // 震动持续时间
    [SerializeField] private float shakeMagnitude = 0.1f; // 震

    public override void Shoot(Vector2 aimDir)
    {
        float spread = Random.Range(-bulletSpreadAngle, bulletSpreadAngle);
        Vector2 spreadDir = Quaternion.Euler(0, 0, spread) * aimDir.normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, spreadDir);
        Instantiate(bulletPrefab, firePoint.position, rotation);

        EjectShell(); // 弹出子弹壳

        // ➤ 生成 muzzle flash
        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, firePoint.position, rotation, transform);
        Destroy(muzzleFlash, 0.1f); // 短暂存在

        CameraShake.Instance.Shake(shakeDuration, shakeMagnitude); // 震动效果

        // 使用 WeaponAudioManager 播放开火音效
        audioManager?.PlayShoot();
    }

    private void EjectShell()
    {
        GameObject shell = Instantiate(bulletShellPrefab, shellEjectPoint.position, shellEjectPoint.rotation);
        Rigidbody2D rb = shell.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 ejectDir = shellEjectPoint.right;
            rb.AddForce(ejectDir * shellEjectForce, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(-shellTorque, shellTorque), ForceMode2D.Impulse);
        }
    }
}
