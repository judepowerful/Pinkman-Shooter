using UnityEngine;

public enum BulletTeam
{
    Player,
    Enemy
}

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings / 子弹设置")]
    public float speed = 15f;
    public BulletTeam team = BulletTeam.Player;
    public float lifeTime = 5f; // 子弹生命周期

    [Header("Hit Effect on object / 子弹击中物体对物体产生的效果")]
    public HitInfo hitInfo;

    [Header("Hit Effect / 命中特效")]
    public GameObject hitEffectPrefab; // 每种子弹可指定命中特效

    private void Start()
    {
        // 设置子弹生命周期
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((team == BulletTeam.Player && collision.CompareTag("Player")) ||
            (team == BulletTeam.Enemy && collision.CompareTag("Enemy")) ||
            collision.CompareTag("Bullet") || collision.CompareTag("Corpse"))
        {
            return;
        }

        if (team == BulletTeam.Player && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>()?.TakeDamage(1);
        }
        else if (team == BulletTeam.Enemy && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>()?.TakeDamage(hitInfo, transform.position);
        }

        // 播放命中特效
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }

        // 禁用碰撞 & 停止运动
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // 延迟销毁，等特效播放完
        Destroy(gameObject);
    }
}
