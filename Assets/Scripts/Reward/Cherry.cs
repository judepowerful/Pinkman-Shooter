using UnityEngine;

public class Cherry : MonoBehaviour
{
    [SerializeField] private int scoreValue = 1;
    [SerializeField] private GameObject pickupEffect; // 可选：捡起时的特效
    [SerializeField] private AudioClip pickupSound; // 可选：捡起时的音效

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 加分
            ScoreManager.Instance.AddScore(scoreValue);
            // 播放捡起特效
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }
            // 播放捡起音效
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }
            // 可加音效 / 特效...

            Destroy(gameObject); // 捡起后消失
        }
    }
}
