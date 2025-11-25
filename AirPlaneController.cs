using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class AirplaneController : MonoBehaviour
{

    public Transform player;


    private bool isPlayerOnAirplane = false;
    private Vector3 movement;
    public float speed = 10f;
    public float waveAmplitude = 0.5f; 

    public float waveFrequency = 1f;  

    private float initialY;         


    private void OnCollisionEnter(Collision collision)
    {
        // 检查碰撞对象是否是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            
            isPlayerOnAirplane = true;
            Debug.Log("Player is on the airplane!");
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        // 检查碰撞对象是否是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            
            isPlayerOnAirplane = false;
        
            Debug.Log("Player has left the airplane!");
        }
    }

    /**
     * 每帧调用一次，用于更新飞机的位置和状态。
     */
    void Update()
    {

        float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        Vector3 currentPosition = transform.position;
        currentPosition.y = initialY + waveOffset;
        // 设置飞机的新位置
        transform.position = currentPosition;

        // 检查玩家是否在飞机上
        if (isPlayerOnAirplane)
        {
            // 获取玩家的水平和垂直输入
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            movement = new Vector3(horizontal, 0, vertical);

            transform.Translate(movement * speed * Time.deltaTime);
            player.position = transform.position + new Vector3(0, 1, 0);

            // 检查玩家是否按下跳跃键
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                
                isPlayerOnAirplane = false;
                Debug.Log("Player has jumped off the airplane!");
            }

            // 检查飞机的Z坐标是否小于等于-150
            if (transform.position.z <= -150)
            {
                Debug.Log("Airplane reached the target position. Loading new scene...");
                SceneManager.LoadScene("大厅");
            }
        }
    }
}