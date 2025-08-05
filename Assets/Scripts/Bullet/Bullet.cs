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

    [Header("击中音效")]
    public AudioClip hitSound; // 撞击音效

    private bool isHit = false; // 是否已经命中

    private void Start()
    {
        // 设置子弹生命周期
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector2.right);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHit) return; // 如果已经命中，则不再处理
        if ((team == BulletTeam.Player && collision.CompareTag("Player")) ||
            (team == BulletTeam.Enemy && collision.CompareTag("Enemy")) ||
            collision.CompareTag("Bullet") || collision.CompareTag("Corpse"))
        {
            return;
        }

        if (team == BulletTeam.Player && collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(hitInfo.damage);
        }
        else if (team == BulletTeam.Enemy && collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDamage(hitInfo, transform.position);
        }

        isHit = true; // 标记为已命中

        // 播放命中音效
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        // 播放命中特效
        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);

        // 禁用碰撞 & 停止运动
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Destroy(gameObject);
    }
}
