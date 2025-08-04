using UnityEngine;
using UnityEngine.UI;

public class SpreadIndicator : MonoBehaviour
{
    [SerializeField] private Image spreadImage;
    private Carbine carbine;
    [SerializeField] private Gradient spreadColor;

    public void SetCarbine(Carbine weapon)
    {
        carbine = weapon;
        spreadImage.enabled = carbine != null; // 没枪就隐藏 UI
    }

    private void Update()
    {
        if (carbine == null || spreadImage == null) return;

        float ratio = carbine.CurrentSpreadRatio;
        spreadImage.fillAmount = ratio;
        spreadImage.color = spreadColor.Evaluate(ratio);
    }
}
