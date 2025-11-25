using UnityEngine;
using UnityEngine.UI;
using ToolManager;
using UnityEngine.SceneManagement;

public class CountdownItemCollector : Singleton<CountdownItemCollector>
{
    public int startTime = 60;            // 倒计时的起始时间（秒）
    public int targetItemCount = 4;       // 目标物品数量
    public Text countdownText;            // 显示倒计时的UI文本
    public Text resultText;               // 显示结果（成功或失败）的UI文本
    public GameObject dialogueUI;         // 对话框UI（用来显示“失败”或“成功”）

    private float timeRemaining;          // 剩余时间
    private int collectedItemCount = 0;   // 已收集的物品数量
    private bool isCountingDown = false;  // 是否正在倒计时
    private bool gameOver = false;        // 游戏是否结束

    void Start()
    {
        timeRemaining = startTime; // 初始化剩余时间
        resultText.text = "";       // 清空结果文本
      
        StartCountdown();           // 开始倒计时
    }

    void Update()
    {
        if (isCountingDown && !gameOver)
        {
            // 更新剩余时间
            timeRemaining -= Time.deltaTime;
            UpdateCountdownText(); // 更新倒计时UI

            if (timeRemaining <= 0)
            {
                // 倒计时结束
                timeRemaining = 0;
                isCountingDown = false;
                EndGame(); // 调用游戏结束方法
            }
        }


        if (Input.GetMouseButtonDown(0) && gameOver)
        {
            if (collectedItemCount >= targetItemCount)
            {
                SceneManager.LoadScene(2);
            }
            else {

                SceneManager.LoadScene(1);
            }
        }
    }

    void StartCountdown()
    {
        isCountingDown = true; // 开始倒计时
    }

    void UpdateCountdownText()
    {
        // 格式化倒计时显示为 "分钟:秒"
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        countdownText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

   

    public void GetItem() {

        // 收集物品
        collectedItemCount++;
        // 更新收集物品的数量
        if (collectedItemCount >= targetItemCount)
        {
            // 达到目标物品数量时，结束游戏并显示成功
            EndGame();
        }
    }

    // 游戏结束的方法，检查玩家是否成功
    void EndGame()
    {
        gameOver = true; // 游戏结束
        dialogueUI.SetActive(true); // 显示对话框
        if (collectedItemCount >= targetItemCount)
        {
            // 成功收集到足够的物品
            resultText.text = "成功！你收集了足够的物品！";
        }
        else
        {
            // 失败，时间用尽或未收集到足够物品
            resultText.text = "失败！时间结束，未收集到足够的物品！";
        }

        // 隐藏倒计时UI，显示结果
        countdownText.gameObject.SetActive(false);
    }
}
