using UnityEngine;

namespace Suriyun.MobileTPS
{
    // 用于使游戏对象始终朝向目标对象的脚本
    public class LookAt : MonoBehaviour
    {
        // 偏移量，用于调整游戏对象的朝向
        public Vector3 offset;

        // 目标对象
        GameObject target;

        // 在脚本启用时调用
        void Start()
        {
            // 查找名为 "target" 的游戏对象并将其赋值给 target 变量
            target = GameObject.Find("target");

            // 检查是否成功找到目标对象
            if (target == null)
            {
                // 如果未找到目标对象，记录错误日志
                Debug.LogError("Target object with name 'target' not found!");
            }
        }

        // 每帧调用一次
        void Update()
        {
            // 检查 target 是否为 null
            if (target != null)
            {
                // 使游戏对象朝向目标对象
                transform.LookAt(target.transform);
                // 应用偏移量
                transform.Rotate(offset);
            }
        }
    }
}