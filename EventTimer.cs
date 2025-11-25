using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// 事件定时器类，用于在指定时间后触发事件
public class EventTimer : MonoBehaviour
{
    // 延迟时间
    public float timer;

    // 在脚本启用时调用
    void Start()
    {
        // 启动延迟协程
        StartCoroutine(Delay());
    }

    // 延迟协程
    IEnumerator Delay()
    {
        // 等待指定时间
        yield return new WaitForSeconds(timer);
        // 触发事件
        EventTimed.Invoke();
    }

    // 定时事件
    public UnityEvent EventTimed;
}
