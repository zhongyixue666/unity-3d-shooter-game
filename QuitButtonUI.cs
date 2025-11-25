using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class QuitButtonUI : MonoBehaviour
{
    public Button quitButton;
    public Button pauseButton;
    public bool isPaused = false;

    private void Awake()
    {
        quitButton = transform.Find("QuitGameButton").GetComponent<Button>();
        quitButton.onClick.AddListener(QuitGame);
        pauseButton = transform.Find("PauseGameButton").GetComponent<Button>();
        Debug.Log("pauseButton");
        if (pauseButton == null)
        {
            Debug.LogError("未能找到PauseGameButton对应的Button组件，请检查对象层级和组件挂载情况！");
        }
        pauseButton.onClick.AddListener(ISPause);
        Debug.Log("PauseButton");
    }

    // 退出游戏的方法
    private void QuitGame()
    {
        // 退出游戏
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 切换暂停/继续状态的方法
    private void ISPause()
    {
        Debug.Log("Pause");
        // 切换暂停状态
        isPaused = !isPaused;
        if (isPaused)
        {
            // 设置时间缩放为0，暂停游戏中的所有基于时间的更新等操作
            // 暂停游戏
            Time.timeScale = 0;
            Debug.Log("meicg    ");
        }
        else
        {
            // 恢复时间缩放为1，游戏继续正常运行
            Time.timeScale = 1;
            Debug.Log("???");
        }
    }
}