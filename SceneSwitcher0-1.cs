using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneJump : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 跳转场景的方法
    public void jump()
    {
        // 加载 GameScene1 场景
        SceneManager.LoadScene("GameScene1");
    }
}
