using UnityEngine;

public class Carbine : Weapon
{
    [Header("子弹设置")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("散射角设置")]
    [SerializeField] private float initialSpreadAngle = 5f;
    [SerializeField] private float maxSpreadAngle = 15f;
    [SerializeField] private float spreadIncreasePerShot = 0.1f;
    [SerializeField] private float spreadRecoverRate = 5f;

    public float currentSpreadAngle;
    public float CurrentSpreadRatio => Mathf.InverseLerp(initialSpreadAngle, maxSpreadAngle, currentSpreadAngle);

    [Header("子弹壳设置")]
    [SerializeField] private GameObject bulletShellPrefab;
    [SerializeField] private Transform shellEjectPoint;
    [SerializeField] private float shellEjectForce = 5f;
    [SerializeField] private float shellTorque = 3f;

    [Header("枪口火花设置")]
    [SerializeField] private GameObject muzzleFlashPrefab;

    [Header("震动设置")]
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeMagnitude = 0.1f;

    protected override void Start()
    {
        base.Start();
        currentSpreadAngle = initialSpreadAngle;
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0)) // 玩家没有按住左键时恢复
        {
            currentSpreadAngle -= spreadRecoverRate * Time.deltaTime;
            currentSpreadAngle = Mathf.Max(currentSpreadAngle, initialSpreadAngle);
        }
    }

    public override bool TryFire(Vector2 aimDir)
    {
        if (CanFire())
        {
            Shoot(aimDir);
            nextFireTime = Time.time + fireRate;

            currentSpreadAngle += spreadIncreasePerShot;
            currentSpreadAngle = Mathf.Min(currentSpreadAngle, maxSpreadAngle);
            return true;
        }

        return false;
    }

    public override void Shoot(Vector2 aimDir)
    {
        float spread = Random.Range(-currentSpreadAngle, currentSpreadAngle);
        Vector2 spreadDir = Quaternion.Euler(0, 0, spread) * aimDir.normalized;
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, spreadDir);
        Instantiate(bulletPrefab, firePoint.position, rotation);

        EjectShell();
        Instantiate(muzzleFlashPrefab, firePoint.position, rotation, transform);
        CameraShake.Instance.Shake(shakeDuration, shakeMagnitude);
        audioManager.PlayShoot();
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
