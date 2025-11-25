using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 单例模式，确保只有一个GameManager实例
    public static GameManager Instance;
    public FailureUI failureUI;
    public QuitButtonUI quitButtonUI;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        // 订阅玩家爆炸事件
        GameEvents.OnPlayerExploded += OnPlayerExplodedHandler;   

    }
    /**
    private void Start()
    {
        // 初始化失败界面隐藏
        if (failureUI!= null)
        {
            failureUI.HideFailureUI();
        }
    }

    **/


    // 玩家爆炸事件处理方法
    public void OnPlayerExplodedHandler(PlayerController player)
    {
        // 显示游戏失败界面
        Debug.Log("游戏失败");
        if (failureUI != null)
        {
            failureUI.ShowFailureUI();
        }

        // 暂停游戏
        Time.timeScale = 0;
    }

    
}