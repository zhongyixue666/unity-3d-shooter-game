using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreTextEffects : MonoBehaviour
{
    // 用于在编辑器中拖入场景中的Canvas对象
    public Canvas canvas; 

    // 用于显示欢迎文本的Text组件
    private Text welcomeText;

    // 在脚本启动时调用
    public void Start()
    {
        // 创建欢迎文本
        welcomeText = CreateWelcomeText();
        // 启动协程以淡入欢迎文本
        StartCoroutine(FadeInText(welcomeText));
        // 为欢迎文本添加阴影效果
        AddTextShadowEffect(welcomeText);
    }

    // 创建欢迎文本的方法
    public Text CreateWelcomeText()
    {
        // 创建一个新的GameObject来承载Text组件
        GameObject textObject = new GameObject("WelcomeText");

        // 给GameObject添加RectTransform组件
        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        // 设置父对象为canvas
        rectTransform.SetParent(canvas.transform);
        // 设置锚点为左下角
        rectTransform.anchorMin = new Vector2(0f, 1f);
        // 设置锚点为右上角
        rectTransform.anchorMax = new Vector2(1f, 0f);
        // 设置偏移量
        rectTransform.offsetMin = new Vector2(-100, -20);
        rectTransform.offsetMax = new Vector2(100, 20);

        // 使用AddComponent方法添加Text组件
        Text text = textObject.AddComponent<Text>();
        // 设置文本内容
        text.text = "欢迎来到游戏";
        // 设置字体
        text.font = Resources.Load<Font>("timesnewarial");
        // 设置字体大小
        text.fontSize = 30;
        // 设置字体颜色
        text.color = new Color(1, 0, 0, 0);  // 初始透明度为0，用于淡入效果
        // 返回Text组件
        return text;
    }

    // 淡入文本的协程
    public IEnumerator FadeInText(Text text)
    {
        // 淡入时长
        float fadeDuration = 2f;  
        // 已过时间
        float elapsedTime = 0f;
        // 初始颜色
        Color startColor = text.color;
        // 目标颜色
        Color targetColor = new Color(1, 0, 0, 0);  // 最终完全不透明的颜色

        // 当已过时间小于淡入时长时
        while (elapsedTime < fadeDuration)
        {
            // 增加已过时间
            elapsedTime += Time.deltaTime;
            // 计算当前透明度
            float alpha = Mathf.Lerp(startColor.a, targetColor.a, elapsedTime / fadeDuration);
            // 设置文本颜色
            text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            // 等待一帧
            yield return null;
        }

        // 设置文本颜色为目标颜色
        text.color = targetColor;
    }

    // 添加文本阴影效果的方法
    public void AddTextShadowEffect(Text text)
    {
        // 添加Shadow组件
        Shadow shadow = text.gameObject.AddComponent<Shadow>();
        // 设置阴影颜色为黑色
        shadow.effectColor = Color.black;  
        // 设置阴影偏移距离
        shadow.effectDistance = new Vector2(1, -1);  
        // 根据文本自身透明度来调整阴影透明度
        shadow.useGraphicAlpha = true;  
    }
}