using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [Header("刷怪点设置")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("怪物设置")]
    [SerializeField] private GameObject[] monsterPrefabs;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int monstersPerWave = 3; // 每轮生成几个怪物
    [SerializeField] private float spawnDelayOnStart = 10f; // 每个怪物生成的间隔时间

    private float timer = 0f;

    private void Start()
    {
        // 初始化计时器
        timer = spawnDelayOnStart;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnMonsters();
            timer = spawnInterval;
        }
    }

    private void SpawnMonsters()
    {
        if (spawnPoints.Length == 0 || monsterPrefabs.Length == 0) return;

        // 随机打乱出生点顺序
        List<Transform> shuffledPoints = new List<Transform>(spawnPoints);
        ShuffleList(shuffledPoints);

        int spawnCount = Mathf.Min(monstersPerWave, shuffledPoints.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            Transform spawnPoint = shuffledPoints[i];

            // 随机选择怪物种类
            int prefabIndex = Random.Range(0, monsterPrefabs.Length);
            GameObject prefab = monsterPrefabs[prefabIndex];

            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
    }

    // Fisher–Yates 洗牌算法
    private void ShuffleList(List<Transform> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
