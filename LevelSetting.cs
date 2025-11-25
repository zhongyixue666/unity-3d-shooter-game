using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Suriyun.MobileTPS
{
    // 关卡设置类，管理关卡相关的设置和事件
    public class LevelSetting : MonoBehaviour
    {
        // 玩家移动位置列表，存储玩家在关卡中的移动点
        public List<Transform> player_move_pos;

        // 关卡开始事件，用于在关卡开始时触发
        public UnityEvent EventOnStart;

        // 在对象初始化时调用
        protected virtual void Start()
        {
            // 触发关卡开始事件
            EventOnStart.Invoke();
        }
    }
}