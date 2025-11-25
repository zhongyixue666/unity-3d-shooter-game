using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Suriyun.MobileTPS;

/**
 * UpdateSensitivity 类用于更新游戏中鼠标灵敏度的显示。
 * 该类会查找场景中的 GameCamera 组件和 Text 组件，并在每帧更新 Text 组件的文本内容，以显示当前的鼠标灵敏度。
 */
public class UpdateSensitivity : MonoBehaviour
{
    /**
     * 对 GameCamera 组件的引用，用于获取鼠标灵敏度。
     */
    public GameCamera g_cam;

    /**
     * 对 Text 组件的引用，用于显示鼠标灵敏度。
     */
    public Text text;

    /**
     * 在脚本启动时调用，用于初始化 g_cam 和 text 变量。
     */
    void Start()
    {
        // 在场景中查找 GameCamera 组件并赋值给 g_cam 变量
        g_cam = GameObject.FindObjectOfType<GameCamera>();
        // 获取当前游戏对象上的 Text 组件并赋值给 text 变量
        text = GetComponent<Text>();
    }

    /**
     * 每帧调用，用于更新 Text 组件的文本内容，以显示当前的鼠标灵敏度。
     */
    void Update()
    {
        // 更新 Text 组件的文本内容，显示当前的鼠标灵敏度
        text.text = "Sensitivity " + (int)(g_cam.mouse_sensitivity*10) * 0.1f;
    }
}