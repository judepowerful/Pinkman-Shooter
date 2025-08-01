using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SegmentedHealthBar : MonoBehaviour
{
    [Header("配置")]
    public GameObject heartUnitPrefab;
    public int maxHealth = 10;

    private List<GameObject> units = new();

    private GridLayoutGroup grid;
    private RectTransform rectTransform;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init(int health)
    {
        maxHealth = health;

        // 清空旧血块
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        units.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject unit = Instantiate(heartUnitPrefab, transform);
            unit.SetActive(true);
            units.Add(unit);
        }

        ResizeGridToFit();
    }

    private void ResizeGridToFit()
    {
        if (grid == null || rectTransform == null) return;

        // 获取格子尺寸和间距
        Vector2 cellSize = grid.cellSize;
        Vector2 spacing = grid.spacing;

        float totalWidth = cellSize.x * maxHealth + spacing.x * (maxHealth - 1) + (grid.padding.left + grid.padding.right) * 3f;

        rectTransform.sizeDelta = new Vector2(totalWidth, rectTransform.sizeDelta.y);
    }

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].SetActive(i < currentHealth);
        }
    }
}
