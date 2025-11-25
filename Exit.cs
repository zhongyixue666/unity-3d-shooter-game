using UnityEngine;
using UnityEngine.UI;

public class ExitScript : MonoBehaviour
{
    private void Awake()
    {
        // 通过名称查找退出按钮对象
        GameObject exitButtonObject = GameObject.Find("Exit");
        if (exitButtonObject != null)
        {
            // 获取退出按钮的组件
            Button exitButton = exitButtonObject.GetComponent<Button>();
            if (exitButton != null)
            {
                exitButton.onClick.AddListener(ExitGame);
            }
            else
            {
                Debug.LogError("没有找到名为Exit的游戏对象上的Button组件");
            }
        }
        else
        {
            Debug.LogError("没有找到名为Exit的游戏对象");
        }
    }

    // 退出游戏的方法
    private void ExitGame()
    {
        // 在编辑器中停止播放模式
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在构建的应用程序中退出应用程序
        Application.Quit();
#endif
    }
}