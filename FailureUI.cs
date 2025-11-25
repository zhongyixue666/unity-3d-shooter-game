using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FailureUI : MonoBehaviour
{
    private Font customFont;
    // 对应返回界面的按钮
    public Button returnButton;
    // 对应退出游戏的按钮
    public Button quitButton;
    // 整个失败界面所在的Canvas，方便控制整体显示隐藏
    public Canvas failureCanvas;



    // 加载字体的方法
    public void LoadFont()
    {
        // 从资源文件夹中加载名为"timesnewarial"的字体
        customFont = Resources.Load<Font>("timesnewarial");
    }

    // 在Canvas上打印"游戏失败"文本并设置样式的方法

    public void PrintFailureTextOnCanvas()
    {
        // 调用LoadFont方法加载字体
        LoadFont();
        if (failureCanvas != null)
        {
            // 创建一个新的GameObject并添加Text组件
            GameObject textObject = new GameObject("FailureText", typeof(Text));
            // 获取Text组件
            Text text = textObject.GetComponent<Text>();
            // 设置文本内容
            text.text = "游 戏 失 败";
            // 设置字体
            text.font = customFont;
            // 设置字体大小
            text.fontSize = 60;
            // 设置字体颜色
            text.color = Color.red;
            // 设置文本对齐方式
            text.alignment = TextAnchor.MiddleCenter;
            // 设置文本对象为Canvas的子对象
            textObject.transform.SetParent(failureCanvas.transform);
            // 获取RectTransform组件
            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            // 设置锚点最小值
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            // 设置锚点最大值
            rectTransform.anchorMax = new Vector2(0.5f, 0.3f);
            // 设置轴心点
            rectTransform.pivot = new Vector2(0.5f, 0f);
            // 设置大小增量
            rectTransform.sizeDelta = new Vector2(300, 300);
            // 设置本地位置
            rectTransform.localPosition = Vector3.zero;
        }
        else
        {
            // 如果Canvas为空，打印错误信息
            Debug.Log("字体未加载成功或Canvas未创建，无法打印文本");
        }
    }

    private void Start()
    {
        // 在Awake方法中调用PrintFailureTextOnCanvas方法
        // PrintFailureTextOnCanvas();
        // 获取返回按钮组件
        returnButton = transform.Find("ReturnButton").GetComponent<Button>();
        // 获取退出按钮组件
        quitButton = transform.Find("QuitButton").GetComponent<Button>();
        // 获取Canvas组件
        failureCanvas = GetComponent<Canvas>();

        // 为返回按钮添加点击事件监听
        returnButton.onClick.AddListener(ReturnToMenu);
        // 为退出按钮添加点击事件监听
        quitButton.onClick.AddListener(QuitGame);

        // 初始状态下隐藏界面
        HideFailureUI();
    }

    // 返回界面的方法，这里加载指定的主菜单场景
    private void ReturnToMenu()
    {
        // 加载名为"GameScene1"的场景
        SceneManager.LoadScene("GameScene1");
        Time.timeScale = 1;
    }

    // 退出游戏的方法，在不同平台有不同处理方式，在编辑器中模拟退出
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 用于外部调用，显示游戏失败界面
    public void ShowFailureUI()
    {
        if (failureCanvas != null)
        {
            // 设置Canvas为激活状态
            failureCanvas.gameObject.SetActive(true);
            // 触发事件，通知订阅者
            //OnShowFailureUI?.Invoke();
        }
    }

    /*
    *用于外部调用，隐藏游戏失败界面
    */
    public void HideFailureUI()
    {
        if (failureCanvas != null)
        {
            // 设置Canvas为非激活状态
            failureCanvas.gameObject.SetActive(false);
        }
    }
}