using UnityEngine;
using System;

public static class GameEvents
{
    // 声明事件对应的委托类型，这里参数为Enemy类型，表示触发事件时传递的参数
    public delegate void EnemyExplodedDelegate(EnemyNormal enemy);
    // 定义事件本身，使用刚才声明的委托类型
    public static event EnemyExplodedDelegate OnEnemyExploded;

    // 新增一个方法用于在合适的地方触发事件，该方法内部去调用事件对应的委托实例
    public static void TriggerEnemyExploded(EnemyNormal enemy)
    {
        OnEnemyExploded?.Invoke(enemy);
    }
     // 声明事件对应的委托类型，这里参数为Enemy类型，表示触发事件时传递的参数
    public delegate void PlayerExplodedDelegate(PlayerController player);
    // 定义事件本身，使用刚才声明的委托类型
    public static event PlayerExplodedDelegate OnPlayerExploded;

    // 新增一个方法用于在合适的地方触发事件，该方法内部去调用事件对应的委托实例
    public static void TriggerPlayerExploded(PlayerController player)
    {
        OnPlayerExploded?.Invoke(player);
    }
}