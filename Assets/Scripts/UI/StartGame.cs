using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    void Update()
    {
        // 按下任意键开始游戏
        if (Input.anyKeyDown)
        {
            // 加载游戏场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
    }
}
