using System.Collections;
using System.Collections.Generic;
using Suriyun.MobileTPS;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class Game3Manage : MonoBehaviour
{

    // 存储所有的波次
    public MonsterWave[] allWaves;  
    // 记录已完成的波次数
    private int completedWaves = 0;  



    // 显示的成功UI
    public GameObject successUI;

    void Start()
    {
        
        // 确保所有波次都能触发事件
        foreach (MonsterWave wave in allWaves)
        {
            if (wave != null)
            {
                // 订阅每个波次的完成事件
                wave.EventWaveCompleted.AddListener(OnWaveCompleted);
            }
        }
    }

    // 响应波次完成的方法
    void OnWaveCompleted()
    {
        // 增加已完成的波次数
        completedWaves++;

        // 如果所有波次都完成了，显示成功界面
        if (completedWaves == allWaves.Length)
        {
            ShowSuccessUI();
            
           
            Debug.Log("游戏成功");
            //暂停游戏
            Time.timeScale = 0;
            // 检测是否按下了ESC键
            //if (Input.GetKeyDown(KeyCode.K))
            //{
            // 通过代码触发按钮的点击事件，模拟点击按钮的响应逻辑
            // 以下两种方式都可以触发按钮点击的响应逻辑
            // 方式一：直接调用按钮点击事件关联的方法（如果QuitGame是公开方法）
            //StartCoroutine(ExitGameAfterDelay(5f));
            //Application.Quit();

            //EditorApplication.isPlaying = false;
            // 方式二：使用Unity的事件触发机制，更符合按钮点击事件的触发逻辑
            //ExecuteEvents.Execute(quitButton.gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            //}
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            // 退出游戏，这里根据实际情况选择合适的退出方式
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        }
    }
    private IEnumerator ExitGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        // 退出游戏，这里根据实际情况选择合适的退出方式
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        // 如果是要加载到某个特定场景（比如主菜单场景等），可以使用SceneManager
        // SceneManager.LoadScene("MainMenuScene");
    }

    private Font customFont;
    // 对应返回界面的按钮
    public Button returnButton;
    // 对应退出游戏的按钮
    public Button quitButton;
    // 整个失败界面所在的Canvas，方便控制整体显示隐藏
    public Canvas failureCanvas;

    // 加载字体的方法
    private void LoadFont()
    {
        // 从资源文件夹中加载名为"timesnewarial"的字体
        customFont = Resources.Load<Font>("timesnewarial");
    }
    private void PrintFailureTextOnCanvas()
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

    // 显示成功UI
    void ShowSuccessUI()
    {
        if (successUI != null)
        {
            // 激活成功UI
            successUI.SetActive(true);  
        }
    }

}