using UnityEngine;
using System.Collections;

namespace Suriyun.MobileTPS
{
    // 管理游戏摄像机的控制逻辑
    public class GameCamera : MonoBehaviour
    {
        public GameObject player; // 玩家对象
        public Transform cam_holder; // 摄像机的挂载位置
        public Transform target; // 摄像机指向的目标点
        public Vector3 offset_pos; // 摄像机的偏移位置
        public float smoothness = 1.66f; // 摄像机平滑度
        private Transform trans; // 当前摄像机的 Transform

        public Transform aimer; // 瞄准器
        public float screen_rotation_speed = 0.33f; // 旋转速度
        public float screen_rotation_smoothness = 16.66f; // 旋转平滑度

        public bool zoomed = false; // 是否处于缩放模式
        public float zoomed_speed_multiplier = 0.33f; // 缩放状态下的速度倍率
        public float zoom_speed = 6f; // 缩放速度
        public float fov_zoom = 30; // 缩放状态下的视角
        public float fov_normal = 60; // 普通状态下的视角
        private Camera cam; // 摄像机组件

        private Agent agent; // 缓存的玩家 Agent 组件
        public float mouse_sensitivity = 0.6f; // 鼠标灵敏度
        private bool mouse_aiming = false; // 是否处于鼠标瞄准模式

        void Start()
        {
            trans = transform;
            cam = GetComponent<Camera>();
            player = GameObject.FindObjectOfType<Agent>().gameObject;
            agent = player.GetComponent<Agent>(); // 缓存玩家的 Agent 组件
        }

        void Update()
        {
            HandleZoom(); // 处理缩放
            UpdateCameraPosition(); // 更新摄像机位置
            UpdateCameraRotation(); // 更新摄像机旋转
        }

        // 处理摄像机缩放
        private void HandleZoom()
        {
            cam.fieldOfView = Mathf.Lerp(
                cam.fieldOfView,
                zoomed ? fov_zoom : fov_normal,
                zoom_speed * Time.deltaTime
            );
        }

        // 更新摄像机位置
        private void UpdateCameraPosition()
        {
            aimer.position = trans.position;

            Vector3 pos = player.transform.position
                          + aimer.forward * offset_pos.z
                          + aimer.up * offset_pos.y
                          + aimer.right * offset_pos.x;

            pos.y = Mathf.Clamp(pos.y, player.transform.position.y, pos.y + 1);
            trans.position = Vector3.Slerp(
                trans.position,
                pos,
                smoothness * Time.deltaTime
            );

            RaycastHit hit;
            if (Physics.Raycast(trans.position, trans.forward, out hit))
            {
                target.transform.position = hit.point;
            }
            else
            {
                target.transform.position = trans.position + trans.forward * 20;
            }
        }

        // 更新摄像机旋转
        private void UpdateCameraRotation()
        {
            aimer.localRotation = Quaternion.Euler(
                aimer.localRotation.eulerAngles.x,
                aimer.localRotation.eulerAngles.y,
                0
            );

            trans.rotation = Quaternion.Slerp(
                trans.rotation,
                aimer.rotation,
                screen_rotation_smoothness * Time.deltaTime
            );
        }

        public void StartMouseAim()
        {
#if UNITY_WEBGL || UNITY_STANDALONE
            mouse_aiming = true;
            StartCoroutine(UpdateMouse());
            Cursor.lockState = CursorLockMode.Locked;
#endif
        }

        public void StopMouseAim()
        {
#if UNITY_WEBGL || UNITY_STANDALONE
            mouse_aiming = false;
#endif
        }

        public void ShowCursor()
        {
#if UNITY_WEBGL || UNITY_STANDALONE
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
#endif
        }

        public void HideCursor()
        {
#if UNITY_WEBGL || UNITY_STANDALONE
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
#endif
        }

#if UNITY_WEBGL || UNITY_STANDALONE
        IEnumerator UpdateMouse()
        {
            Vector3 lastframe_pos = Input.mousePosition;

            while (mouse_aiming)
            {
                Vector3 delta_pos = lastframe_pos - Input.mousePosition;
                lastframe_pos = Input.mousePosition;

                HandleInput(
                    Input.GetAxis("Mouse X") * mouse_sensitivity,
                    Input.GetAxis("Mouse Y") * mouse_sensitivity * -1f
                );

                HandleMouseClick();

                yield return null;
            }
        }

        private void HandleMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                agent.behaviour.StartFiring();
            }
            if (Input.GetMouseButtonUp(0))
            {
                agent.behaviour.StopFiring();
            }
        }
#endif

        public void HandleInput(float delta_x, float delta_y)
        {
            Vector3 rotate_horizontal = Vector3.up * delta_x * screen_rotation_speed;
            Vector3 rotate_vertical = aimer.right * delta_y * screen_rotation_speed;

            if (zoomed)
            {
                rotate_horizontal *= zoomed_speed_multiplier;
                rotate_vertical *= zoomed_speed_multiplier;
            }

            Quaternion tmp = aimer.rotation;
            aimer.Rotate(rotate_vertical, Space.World);

            if (aimer.up.y < 0) // 防止摄像机翻转
            {
                aimer.rotation = tmp;
            }

            aimer.Rotate(rotate_horizontal, Space.World);
        }
    }
}
