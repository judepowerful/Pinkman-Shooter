using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance; // 全局访问（单例）

    private Vector3 originalPos;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;

    private void Awake()
    {
        Instance = this;
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (shakeDuration > 0)
        {
            Vector2 shakeOffset = Random.insideUnitCircle * shakeMagnitude;
            transform.localPosition = originalPos + new Vector3(shakeOffset.x, shakeOffset.y, 0f);
            shakeDuration -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
