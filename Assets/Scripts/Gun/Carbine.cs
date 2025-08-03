using UnityEngine;

public class Carbine : Weapon
{
    [Header("子弹设置")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpreadAngle = 5f; // 子弹散射角度（±）
    [Header("子弹壳设置")]
    [SerializeField] private GameObject bulletShellPrefab;
    [SerializeField] private Transform shellEjectPoint;     // 子弹壳飞出的起点
    [SerializeField] private float shellEjectForce = 5f;    // 弹出力度
    [SerializeField] private float shellTorque = 3f;       // 旋转力度
    [Header("枪口火花设置")]
    [SerializeField] private GameObject muzzleFlashPrefab;
    [Header("震动设置")]
    [SerializeField] private float shakeDuration = 0.1f; // 震动持续时间
    [SerializeField] private float shakeMagnitude = 0.1f; // 震

    public override void Shoot(Vector2 aimDir)
    {
        // 生成子弹
        float spread = Random.Range(-bulletSpreadAngle, bulletSpreadAngle);
        Vector2 spreadDir = Quaternion.Euler(0, 0, spread) * aimDir.normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, spreadDir);
        Instantiate(bulletPrefab, firePoint.position, rotation);

        // 弹出子弹壳
        EjectShell();

        // 生成 muzzle flash
        Instantiate(muzzleFlashPrefab, firePoint.position, rotation, transform);

        // 震动
        CameraShake.Instance.Shake(shakeDuration, shakeMagnitude);

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
