using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject explosionEffectPrefab; // 爆炸效果的预设体
    public float hellY = -8f; // 指定触发爆炸的Y轴位置，低于该值触发爆炸
    private bool hasExploded = false; // 标记Boss是否已经爆炸

    void Update()
    {
        // 检查Boss是否掉到地板下，触发爆炸
        if (!hasExploded && transform.position.y < hellY)
        {
            TriggerExplosion();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检查Boss是否与Player碰撞，触发爆炸
        if (collision.gameObject.CompareTag("Player") && !hasExploded)
        {
            TriggerExplosion();
        }
    }

    private void TriggerExplosion()
    {
        hasExploded = true; // 标记Boss已爆炸

        Debug.Log("Boss触发爆炸！");

        // 生成爆炸效果
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("ExplosionEffectPrefab没有在Inspector中指定！");
        }

        // 销毁Boss对象
        gameObject.SetActive(false); // 隐藏Boss，或者你可以使用 Destroy(gameObject) 来销毁对象
    }
}
