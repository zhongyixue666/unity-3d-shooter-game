using System.Collections;
using System.Collections.Generic;
using Suriyun.MobileTPS;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

/**
 * SuccessUI 类用于管理游戏成功界面的显示和隐藏，以及在界面上显示成功文本。
 */
public class SuccessUI : MonoBehaviour
{
    /**
     * 对 MonsterWave 组件的引用，用于获取当前怪物波次。
     */
    private MonsterWave monsterWave;

    /**
     * 自定义字体的引用，用于设置成功文本的字体。
     */
    private Font customFont;

    /**
     * 成功界面所在的 Canvas，方便控制整体显示隐藏。
     */
    public Canvas successCanvas;
    //public Button quitButton;

    /**
     * 加载字体的方法，从资源文件夹中加载名为"timesnewarial"的字体。
     */
    public void LoadFont()
    {
        // 从资源文件夹中加载名为"timesnewarial"的字体
        customFont = Resources.Load<Font>("timesnewarial");
    }

    /**
     * 在 Canvas 上打印"游戏成功"文本并设置样式的方法。
     */
    public void PrintSuccessTextOnCanvas()
    {
        // 调用LoadFont方法加载字体
        LoadFont();
        if (successCanvas != null)
        {
            // 创建一个新的GameObject并添加Text组件
            GameObject textObject = new GameObject("SuccessText", typeof(Text));
            // 获取Text组件
            Text text = textObject.GetComponent<Text>();
            // 设置文本内容
            text.text = "游 戏 成 功";
            // 设置字体
            text.font = customFont;
            // 设置字体大小
            text.fontSize = 60;
            // 设置字体颜色
            text.color = Color.red;
            // 设置文本对齐方式
            text.alignment = TextAnchor.MiddleCenter;
            // 设置文本对象为Canvas的子对象
            textObject.transform.SetParent(successCanvas.transform);
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

    /**
     * 在Awake方法中调用PrintSuccessTextOnCanvas方法，并获取SuccessCanvas组件。
     */
    private void Awake()
    {
        // 在Awake方法中调用PrintSuccessTextOnCanvas方法
        //PrintSuccessTextOnCanvas();

        successCanvas = GetComponent<Canvas>();
        // 在场景中查找名为 "QuitGameButton" 的游戏对象，并获取其 Button 组件
        //quitButton = transform.Find("QuitGame").GetComponent<Button>();
        // 为退出按钮添加点击事件监听
        // quitButton.onClick.AddListener(QuitGame);
        // 在场景中查找名为 "PauseGameButton" 的游戏对象，并获取其Button组件
        //pauseButton = transform.Find("PauseGameButton").GetComponent<Button>();
        //Debug.Log("pauseButton");
        //if (pauseButton == null)
        //{
        //    // 如果没有找到PauseGameButton，记录错误日志
        //    Debug.LogError("未能找到PauseGameButton对应的Button组件，请检查对象层级和组件挂载情况！");
        //}

        // 初始状态下隐藏界面
        HideSuccessUI();
    }

    /**
     * 显示游戏成功界面的方法。
     */
    public void ShowSuccessUI()
    {

        if (successCanvas != null)
        {
            // 设置Canvas为激活状态
            successCanvas.gameObject.SetActive(true);
            Debug.Log("success");

        }
    }

    /**
     * 用于外部调用，隐藏游戏成功界面的方法。
     */
    public void HideSuccessUI()
    {
        if (successCanvas != null)
        {
            // 设置Canvas为非激活状态
            successCanvas.gameObject.SetActive(false);
        }
    }
    /**
     * 退出游戏的方法。
     */
    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    // 切换暂停/继续状态的方法
    public bool isPaused = false;
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