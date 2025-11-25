using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Suriyun.MobileTPS
{
    // 游戏主类，管理游戏状态和事件
    public class Game : MonoBehaviour
    {
        // 单例模式，确保只有一个Game实例
        public static Game instance;

        // 当前关卡设置
        [HideInInspector]
        public LevelSetting level_setting;

        // 游戏是否暂停
        public bool is_pause;

        // 处理热键的协程
        [HideInInspector]
        public Coroutine c_handle_hotkey;

        // 暂停游戏的热键
        public KeyCode hotkey_pause;

        // 游戏开始事件
        public UnityEvent EventGameStart;

        // 游戏暂停事件
        public UnityEvent EventGamePause;

        // 游戏恢复事件
        public UnityEvent EventGameResume;

        // 游戏重新开始事件
        public UnityEvent EventGameRestart;

        // 游戏结束事件
        public UnityEvent EventGameOver;

        // 玩家角色
        private Player player;

        // 游戏音频管理
        private AudioSource audioSource;

        // 游戏状态
        private bool isGameOver = false;
        private void Start()
        {
            
        }
        // 在对象初始化时调用
        protected virtual void Awake()
        {
            // 设置单例实例
            instance = this;
            // 设置目标帧率为60帧
            Application.targetFrameRate = 60;

            // 如果关卡设置为空，查找场景中的LevelSetting对象
            if (level_setting == null)
            {
                level_setting = GameObject.FindObjectOfType<LevelSetting>();
            }

            // 查找玩家和音频源
            player = FindObjectOfType<Player>();
            audioSource = GetComponent<AudioSource>();

            // 确保游戏初始状态为非暂停
            is_pause = false;
           
        }

        // 开始处理热键
        public void StartHandleHotkey()
        {
            // 启动协程处理热键
            c_handle_hotkey = StartCoroutine(HandleHotkey());
        }

        // 停止处理热键
        public void StopHandleHotkey()
        {
            // 停止协程
            StopCoroutine(c_handle_hotkey);
        }

        // 处理热键的协程
        IEnumerator HandleHotkey()
        {
            while (true)
            {
                // 检测是否按下暂停热键
                if (Input.GetKeyDown(hotkey_pause))
                {
                    // 如果游戏已暂停，恢复游戏
                    if (is_pause)
                    {
                        this.GameResume();
                    }
                    else
                    {
                        // 否则暂停游戏
                        this.GamePause();
                    }
                }
                // 等待一帧
                yield return null;
            }
        }

        // 游戏开始
        public virtual void GameStart()
        {
            // 触发游戏开始事件
            EventGameStart.Invoke();
        }

        // 游戏暂停
        public virtual void GamePause()
        {
            if (isGameOver) return; // 如果游戏已结束，无法暂停

            // 设置游戏为暂停状态
            is_pause = true;

            // 暂停时间
            Time.timeScale = 0;

            // 暂停音频
            if (audioSource != null)
            {
                audioSource.Pause();
            }

            // 触发游戏暂停事件
            EventGamePause.Invoke();
        }

        // 游戏恢复
        public virtual void GameResume()
        {
            if (isGameOver) return; // 如果游戏已结束，无法恢复

            // 设置游戏为非暂停状态
            is_pause = false;

            // 恢复时间
            Time.timeScale = 1;

            // 恢复音频
            if (audioSource != null)
            {
                audioSource.Play();
            }

            // 触发游戏恢复事件
            EventGameResume.Invoke();
        }

        // 游戏重新开始
        public virtual void GameRestart()
        {
            // 恢复时间
            Time.timeScale = 1;

            // 显示鼠标光标
            Cursor.visible = true;

            // 解锁鼠标光标
            Cursor.lockState = CursorLockMode.None;

            // 重新加载当前场景
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // 游戏结束
        public virtual void GameOver()
        {
            if (isGameOver) return; // 防止重复调用

            // 标记游戏已结束
            isGameOver = true;

            // 触发游戏结束事件
            EventGameOver.Invoke();

            // 输出调试信息
            Debug.Log("Game Over");

        }
       
        // 加载关卡
        public virtual void LoadLevel(string level)
        {
            // 启动协程延迟加载关卡
            StartCoroutine(LoadLevelDelay(level, 3f));
        }

        // 延迟加载关卡的协程
        protected IEnumerator LoadLevelDelay(string level, float sec)
        {
            // 等待指定时间
            yield return new WaitForSeconds(sec);

            // 加载指定关卡
            SceneManager.LoadScene(level);
        }
        

        // 检查玩家状态
        private bool IsPlayerAlive()
        {
            return player != null && player.health > 0;
        }
    }
}