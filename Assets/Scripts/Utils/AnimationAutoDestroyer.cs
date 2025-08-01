using UnityEngine;

public class AnimationAutoDestroy : MonoBehaviour
{
    void Start()
    {
        var animator = GetComponent<Animator>();
        if (animator != null)
        {
            // 获取动画长度
            float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
            // 设置延迟销毁时间
            Destroy(gameObject, animationLength);
        }
        else
        {
            Debug.LogWarning("Animator component not found on " + gameObject.name);
        }
    }
}