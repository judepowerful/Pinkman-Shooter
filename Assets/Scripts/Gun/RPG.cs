using System.Collections;
using UnityEngine;
public class RPG : Weapon
{
    [Header("RPG 设置")]
    [SerializeField] private Transform rocketVisual; // 武器上装配的火箭模型
    [SerializeField] private GameObject rocketPrefab; // 火箭

    [Header("枪口特效")]
    [SerializeField] private GameObject smokeCloudPrefab; // 场地残留烟雾

    [Header("震动设置")]
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.15f;

    public override void Shoot(Vector2 aimDir)
    {
        // 生成火箭
        Quaternion rotation = Quaternion.FromToRotation(Vector2.right, aimDir.normalized);
        Instantiate(rocketPrefab, firePoint.position, rotation);

        // 隐藏枪上的火箭模型（相当于射出去了）
        if (rocketVisual != null)
            rocketVisual.gameObject.SetActive(false);

        if (smokeCloudPrefab != null)
            Instantiate(smokeCloudPrefab, firePoint.position, rotation, transform);

        // 相机震动
        CameraShake.Instance.Shake(shakeDuration, shakeMagnitude);

        // 音效
        audioManager.PlayShoot();

        // 启动协程在 fireRate 秒后重新显示火箭头
        StartCoroutine(ReloadRocketVisual());
    }

    private IEnumerator ReloadRocketVisual()
    {
        yield return new WaitForSeconds(fireRate);

        if (rocketVisual != null)
            rocketVisual.gameObject.SetActive(true); // 模拟重新装弹
    }
}
