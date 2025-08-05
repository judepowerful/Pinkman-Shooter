using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int currentScore = 0;

    [SerializeField] private TextMeshProUGUI scoreText; // 可选：绑定 UI 显示

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        currentScore += amount;

        if (scoreText != null)
        {
            scoreText.text = currentScore.ToString();
        }
    }
}
