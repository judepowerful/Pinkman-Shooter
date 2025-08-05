using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings / 爆炸设置")]
    [SerializeField] private float damageRadius = 3f;
    [SerializeField] private int damageAmount = 5;
    [SerializeField] private float explosionForce = 15f;

    [Header("镜头震动设置")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.3f;

    [Header("烟雾粒子特效")]
    [SerializeField] private GameObject smokeEffectPrefab;
    [SerializeField] private float smokeDuration = 10f;

    public BulletTeam team;

    private void Start()
    {
        GameObject smokeEffect = Instantiate(smokeEffectPrefab, transform.position, Quaternion.identity);
        Destroy(smokeEffect, smokeDuration);
        
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // 相机震动
            CameraShake.Instance.Shake(shakeDuration, shakeMagnitude);

        // 获取范围内所有碰撞体
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        foreach (var hit in hits)
        {
            Vector2 dir = (hit.transform.position - transform.position).normalized;
            
            dir += Vector2.up * 0.5f;
            dir.Normalize();

            if (team == BulletTeam.Player && hit.CompareTag("Enemy"))
            {
                // 伤害
                Enemy enemy = hit.GetComponent<Enemy>();
                enemy.TakeDamage(damageAmount);
            }
            else if (team == BulletTeam.Enemy && hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerController>().TakeDamage(new HitInfo { damage = damageAmount }, transform.position);
            }

            // 推开
            if (hit.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                float forceMultiplier = 1f - Mathf.Clamp01(distance / damageRadius); // 0~1
                rb.AddForce(explosionForce * forceMultiplier * dir, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
