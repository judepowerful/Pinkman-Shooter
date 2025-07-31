using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Bullet"))
        {
            // 不处理玩家或子弹碰撞
            return;
        }
        Debug.Log("Bullet hit: " + collision.gameObject.name);
        // 播放爆炸动画
        anim.SetTrigger("Explode");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        Destroy(gameObject);
    }
}
