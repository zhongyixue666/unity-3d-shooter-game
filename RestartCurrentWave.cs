using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Suriyun.MobileTPS;

// 重启当前怪物波次的脚本
public class RestartCurrentWave : MonoBehaviour
{
    // 当前怪物波次的引用
    public MonsterWave current_wave;

    // 设置当前怪物波次
    public void SetCurrentWave(MonsterWave wave_spawner)
    {
        current_wave = wave_spawner;
    }

    // 重启当前怪物波次
    public void Do()
    {
        current_wave.RestartWave();
    }
}
