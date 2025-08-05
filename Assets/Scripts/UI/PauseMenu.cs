using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private MonoBehaviour[] scriptsToDisable;

    [SerializeField] private MonsterSpawner[] monsterSpawners;
    [SerializeField] private TextMeshProUGUI monsterSpawnerText;

    private bool isPaused = false;
    public bool mosnterSpawnerEnabled = true;

    private void Update()
    {
        // 你可以改成用按钮点击调用
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        foreach (var script in scriptsToDisable)
        {
            if (script != null) script.enabled = false;
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        foreach (var script in scriptsToDisable)
        {
            if (script != null) script.enabled = true;
        }
    }

    public void ToggleMonsterSpawners()
    {
        mosnterSpawnerEnabled = !mosnterSpawnerEnabled;
        monsterSpawnerText.text = mosnterSpawnerEnabled ? "Monster OFF" : "Monster ON";
        foreach (var spawner in monsterSpawners)
        {
            if (spawner != null) spawner.enabled = mosnterSpawnerEnabled;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // 确保时间恢复正常
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        // 在 Editor 中运行不会退出
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
