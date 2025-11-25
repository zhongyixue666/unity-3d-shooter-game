using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // 要跟随的目标物体的Transform组件
    public Transform target;
    // 相机相对于目标物体的偏移量
    public Vector3 offset;

    // 相机旋转速度
    public float rotationSpeed = 10f;
    // 相机绕Y轴的旋转角度（偏航角）
    private float yaw = 0f;
    // 相机绕X轴的旋转角度（俯仰角）
    private float pitch = 0f;
    // 是否正在拖动鼠标
    private bool isDragging = false;

    void Update()
    {
        if (target != null)
        {
            // 鼠标右键按下时开始拖动
            if (Input.GetMouseButtonDown(1))
            {
                isDragging = true;
            }
            // 鼠标右键松开时停止拖动
            else if (Input.GetMouseButtonUp(1))
            {
                isDragging = false;
            }

            // 当拖动鼠标时，更新相机的旋转角度
            if (isDragging)
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");

                // 根据鼠标移动量更新偏航角和俯仰角
                yaw += mouseX * rotationSpeed;
                pitch -= mouseY * rotationSpeed;

                // 限制俯仰角度，避免相机翻转
                pitch = Mathf.Clamp(pitch, -80f, 80f);
            }

            // 计算相机的旋转，并更新相机的位置
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            offset = rotation * new Vector3(0f, 0f, -offset.magnitude);  // 根据旋转调整偏移量
            transform.position = target.position + offset;
            transform.LookAt(target);
        }
    }
}