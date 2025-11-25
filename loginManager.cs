using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField nameInput; 
    public InputField passwordInput;
    public GameObject registerSuccessMessage;
    public GameObject loginErrorMessage;

    private string registerUrl = "http://localhost:3000/register"; // 注册 API 地址
    private string loginUrl = "http://localhost:3000/login"; // 登录 API 地址

    // 注册功能
    public void Register()
    {
        string username = nameInput.text.Trim(); // 去除多余空格
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("用户名或密码不能为空！");
            StartCoroutine(ShowMessage(loginErrorMessage)); // 显示错误提示
            return;
        }

        // 调用协程进行注册
        StartCoroutine(RegisterUser(username, password));
    }

    private IEnumerator RegisterUser(string username, string password)
    {
        // 创建 JSON 数据
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

        // 创建请求
        UnityWebRequest request = new UnityWebRequest(registerUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log($"发送注册请求到: {registerUrl}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"注册成功: {request.downloadHandler.text}");
            StartCoroutine(ShowMessage(registerSuccessMessage)); // 显示注册成功提示
        }
        else
        {
            Debug.LogError($"注册失败: {request.error}\n响应: {request.downloadHandler.text}");
            StartCoroutine(ShowMessage(loginErrorMessage)); // 显示错误提示
        }
    }

    // 登录功能
    public void Login()
    {
        string username = nameInput.text.Trim(); // 去除多余空格
        string password = passwordInput.text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("用户名或密码不能为空！");
            StartCoroutine(ShowMessage(loginErrorMessage)); // 显示错误提示
            return;
        }

        // 调用协程进行登录
        StartCoroutine(LoginUser(username, password));
    }

    private IEnumerator LoginUser(string username, string password)
    {
        // 创建 JSON 数据
        string jsonData = $"{{\"username\":\"{username}\",\"password\":\"{password}\"}}";

        // 创建请求
        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log($"发送登录请求到: {loginUrl}");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"登录成功: {request.downloadHandler.text}");
            SceneManager.LoadScene("GameScene"); // 跳转到下一个场景
        }
        else
        {
            Debug.LogError($"登录失败: {request.error}\n响应: {request.downloadHandler.text}");
            StartCoroutine(ShowMessage(loginErrorMessage)); // 显示登录失败提示
        }
    }

    // 显示提示信息
    private IEnumerator ShowMessage(GameObject messageObject)
    {
        messageObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        messageObject.SetActive(false);
    }
}
