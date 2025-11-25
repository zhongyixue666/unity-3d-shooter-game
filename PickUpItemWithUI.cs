using UnityEngine;
using UnityEngine.UI; // 导入UI命名空间

public class PickUpItemWithUI : MonoBehaviour
{
    public float pickupRange = 3f;    // 设置拾取的最大范围
    public LayerMask pickupLayer;     // 设置可拾取物体所在的层
    public Text pickupHintText;       // 用于显示提示信息的 UI Text
    public GameObject pickupUI;       // 提示框 UI（可以是一个包含 Text 的 Panel）

    private GameObject currentItem = null;

    void Start()
    {
        // 初始时隐藏拾取提示框
        pickupUI.SetActive(false);
    }

    void Update()
    {
        // 检测鼠标位置并进行射线检测
        DetectItemUnderMouse();

        // 按下 F 键进行拾取操作
        if (Input.GetKeyDown(KeyCode.F) && currentItem != null)
        {
            PickUp(currentItem);
        }
    }

    void DetectItemUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // 从鼠标位置发射射线
        RaycastHit hit;

        // 发射射线，检查射线是否与可拾取的物体碰撞
        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            // 如果射线碰到的新物体与之前的物体不同，更新提示框
            if (hitObject != currentItem)
            {
                currentItem = hitObject;
                ShowPickupHint(true);
            }
        }
        else
        {
            // 如果射线没有碰到任何物体，隐藏提示框
            if (currentItem != null)
            {
                currentItem = null;
                ShowPickupHint(false);
            }
        }
    }

    void ShowPickupHint(bool show)
    {
        // 显示或隐藏 UI 提示框
        pickupUI.SetActive(show);

        if (show && currentItem != null)
        {
            // 如果显示提示框，更新提示文本
            pickupHintText.text = "按 F 键拾取 " + currentItem.name;
        }
    }

    void PickUp(GameObject pickedObject)
    {
        // 示例：销毁拾取的物体
        Debug.Log("拾取物体: " + pickedObject.name);
        Destroy(pickedObject);
        CountdownItemCollector.Instance.GetItem();
        // 隐藏提示框
        ShowPickupHint(false);
    }
}
