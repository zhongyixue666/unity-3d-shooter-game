using UnityEngine;

public class EnemyNormal : MonoBehaviour
{
    // 玩家对象的引用，用于判断相对位置
    public Transform player;
    // 避让速度，控制远离玩家的速度大小
    public float avoidSpeed = 3f;
    // 安全距离，小于该距离时开始远离玩家
    public float safeDistance = 5f;
    // Y轴位置临界值，低于该值触发爆炸
    public float hellY = -8f;
    // 爆炸效果预制体的引用
    public GameObject explosionEffectPrefab;
    // 自身的刚体组件，用于控制物理运动
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
    }

    void Update()
    {
        if (player == null || rb == null) return;

        // 计算与玩家之间的距离
        float distance = Vector3.Distance(transform.position, player.position);

        // 距离小于安全距离，开始远离玩家移动
        if (distance < safeDistance)
        {
            Vector3 direction = (transform.position - player.position).normalized;
            Vector3 move = direction * avoidSpeed;

            // 对整个 move 向量进行缩放调整，确保依然是 Vector3 类型的正确向量操作
            Vector3 adjustedMove = move * 2f; 

            rb.velocity = new Vector3(adjustedMove.x, rb.velocity.y, adjustedMove.z);
        }
        else
        {
            // 距离大于等于安全距离，停止水平方向移动，仅保留Y轴方向速度（如受重力影响的下落速度）
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
        }

        // 判断是否掉到特定区域触发爆炸
        if (transform.position.y < hellY)
        {
            TriggerExplosion();
        }
    }

    // 用于平滑移动的辅助变量，X轴方向速度平滑
    private Vector3 velocityXSmooth = Vector3.zero;
    // 用于平滑移动的辅助变量，Z轴方向速度平滑
    private Vector3 velocityZSmooth = Vector3.zero;
    public bool isExploded = false;

    public void TriggerExplosion()
    {
        isExploded = true;
        GameEvents.TriggerEnemyExploded(this);
        // 生成爆炸效果
        if (explosionEffectPrefab!= null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 将自身设置为非激活状态，也可选择销毁（Destroy(gameObject)）
        gameObject.SetActive(false);
    }

}