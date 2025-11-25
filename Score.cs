using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int health = 2;
    public int score = 0;
    private Text uiText;
    private ScoreTextEffects uiTextEffects;
    public Canvas canvas;

    void Start()
    {
        // 创建UITextEffects实例并初始化文本效果
        uiTextEffects = gameObject.AddComponent<ScoreTextEffects>();
        uiTextEffects.canvas = canvas;
        uiTextEffects.Start();
        uiText = CreateUIText();
        // 更新UI显示
        UpdateUI();
        // 订阅敌人爆炸事件
        GameEvents.OnEnemyExploded += OnEnemyExplodedHandler;
    }

    // 创建用于显示生命值和分数的文本组件的方法
    private Text CreateUIText()
    {
        GameObject textObject = new GameObject("UIInfoText");
        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.SetParent(canvas.transform);
        rectTransform.anchorMin = new Vector2(0.1f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.1f, 1f);
        rectTransform.offsetMin = new Vector2(-100, -20);
        rectTransform.offsetMax = new Vector2(100, -20);
        Text text = textObject.AddComponent<Text>();
        text.text = "";
        text.font = Resources.Load<Font>("timesnewarial");
        text.fontSize = 30;
        text.color = Color.yellow;
        return text;
    }

    // 当玩家与其他物体发生碰撞时调用
    void OnCollisionEnter(Collision collision)
    {
        // 如果碰撞物体的标签是"Obstacle"
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // 玩家生命值减1
            health -= 1;
            // 更新UI显示
            UpdateUI();
            // 检查游戏是否结束
            CheckGameOver();
        }
        // 如果碰撞物体的标签是"Boss"
        else if (collision.gameObject.CompareTag("Boss"))
        {
            // 玩家生命值减2
            health -= 2;
            // 更新UI显示
            UpdateUI();
            // 检查游戏是否结束
            CheckGameOver();
        }
    }

    // 增加分数的方法
    void Plusscore()
    {
        if (GetComponent<EnemyNormal>().isExploded == true)
        {
            // 分数加1
            score += 1;
            // 更新UI显示
            UpdateUI();
            // 检查是否胜利
            CheckWin();
        }
    }

    // 更新UI显示的方法
    void UpdateUI()
    {
        if (uiText != null)
        {
            // 设置uiText的文本内容为"生命值: " + 当前生命值 + " 分数: " + 当前分数
            uiText.text = "生命值: " + health + " 分数: " + score;
        }
    }

    // 检查游戏是否结束的方法
    void CheckGameOver()
    {
        // 如果玩家生命值小于等于0
        if (health <= 0)
        {
            // 在控制台输出"游戏失败！"
            Debug.Log("游戏失败！");
            // 显示失败界面
            GameManager.Instance.failureUI.ShowFailureUI();
            Time.timeScale = 0;
        }
    }

    // 检查是否胜利的方法
    void CheckWin()
    {
        if (score >= 10)
        {
            Debug.Log("游戏胜利！");
        }
    }

    // 当敌人爆炸时调用的方法
    private void OnEnemyExplodedHandler(EnemyNormal enemy)
    {
        // 分数加1
        score += 1;
        // 更新UI显示
        UpdateUI();
        // 检查是否胜利
        CheckWin();
        Debug.Log("OnEnemyExplodedHandler");
    }
}