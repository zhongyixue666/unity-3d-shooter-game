using UnityEngine;
using System.Collections;

namespace Suriyun.MobileTPS
{
    // 主摄像机类，用于控制摄像机的移动和过渡
    public class MainCamera : MonoBehaviour
    {
        // 摄像机的父对象，用于控制摄像机的位置
        public Transform cam_holder;
        // 摄像机的变换组件
        Transform trans;

        // 在脚本启用时调用
        void Start()
        {
            // 获取摄像机的变换组件
            trans = transform;
        }

        // 每帧调用一次
        void Update()
        {
            // 平滑摄像机过渡
            trans.position = Vector3.Lerp(trans.position, cam_holder.position, 60f * Time.deltaTime);
        }
    }
}