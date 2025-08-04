using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [Header("Explosion Settings / 爆炸设置")]
    [SerializeField] private float damageRadius = 2f;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private GameObject smokeEffectPrefab;
    public BulletTeam team;

    private void Start()
    {
        GameObject smokeEffect = Instantiate(smokeEffectPrefab, transform.position, Quaternion.identity);
        Destroy(smokeEffect, 10f);
        
        CameraShake.Instance.Shake(0.5f, 0.3f); // 震动效果
        // 获取范围内所有碰撞体
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        foreach (var hit in hits)
        {
            Vector2 dir = (hit.transform.position - transform.position).normalized;
            // 向上偏移 1 倍单位向量
            dir += new Vector2(dir.x, 1f);
            dir.Normalize();

            if (team == BulletTeam.Player && hit.CompareTag("Enemy"))
            {
                // 伤害
                hit.GetComponent<Enemy>().TakeDamage(damageAmount);

                // 推力
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                rb.AddForce(dir * explosionForce, ForceMode2D.Impulse);
            }
            else if (team == BulletTeam.Enemy && hit.CompareTag("Player"))
            {
                hit.GetComponent<PlayerController>()?.TakeDamage(new HitInfo { damage = damageAmount }, transform.position);

                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.AddForce(dir * explosionForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
