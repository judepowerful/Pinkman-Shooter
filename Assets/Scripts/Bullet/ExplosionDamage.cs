using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public float damageRadius = 2f;
    public int damageAmount = 1;
    public float explosionForce = 10f;
    public BulletTeam team;

    private void Start()
    {
        CameraShake.Instance.Shake(0.5f, 0.3f); // 震动效果
        // 获取范围内所有碰撞体
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        foreach (var hit in hits)
        {
            Vector2 dir = (hit.transform.position - transform.position).normalized;

            if (team == BulletTeam.Player && hit.CompareTag("Enemy"))
            {
                // 伤害
                hit.GetComponent<Enemy>()?.TakeDamage(damageAmount);

                // 推力
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
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

        // 自动销毁特效体
        Destroy(gameObject, 0.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
