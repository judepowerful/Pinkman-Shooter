using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcherUI : MonoBehaviour
{
    [SerializeField] private List<RectTransform> weaponIcons;
    [SerializeField] private float offsetX = 20f;
    [SerializeField] private float moveSpeed = 10f;

    private int currentWeaponIndex = 0;
    private List<Vector2> originalPositions = new();
    private Dictionary<RectTransform, Coroutine> runningCoroutines = new();

    private void Start()
    {
        foreach (var icon in weaponIcons)
        {
            originalPositions.Add(icon.anchoredPosition);
        }

        // 初始化第一把武器位置
        UpdateWeaponUI();
    }

    public void SwitchWeapon(int newIndex)
    {
        if (newIndex < 0 || newIndex >= weaponIcons.Count) return;

        currentWeaponIndex = newIndex;
        UpdateWeaponUI();
    }

    private void UpdateWeaponUI()
    {
        for (int i = 0; i < weaponIcons.Count; i++)
        {
            Vector2 target = originalPositions[i];
            if (i == currentWeaponIndex)
                target += new Vector2(offsetX, 0f);

            RectTransform icon = weaponIcons[i];

            // 如果已有协程在运行，先停止
            if (runningCoroutines.ContainsKey(icon) && runningCoroutines[icon] != null)
                StopCoroutine(runningCoroutines[icon]);

            // 启动新的移动协程
            Coroutine co = StartCoroutine(SmoothMove(icon, target));
            runningCoroutines[icon] = co;
        }
    }

    private IEnumerator SmoothMove(RectTransform icon, Vector2 targetPos)
    {
        Vector2 startPos = icon.anchoredPosition;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            icon.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            yield return null;
        }

        icon.anchoredPosition = targetPos;
    }
}
