using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // 如果你使用的是Text组件
// using TMPro;  // 如果你使用的是TextMeshPro

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;           // 用于显示对话的Text组件
    // public TextMeshProUGUI dialogueText; // 如果你使用的是TextMeshPro
    public float typingSpeed = 0.05f;   // 每个字母出现的时间间隔
    public GameObject dialogueUI;       // 对话框的UI面板（包含Text）

    private string[] dialogueLines;     // 存储对话的数组
    private int currentLine = 0;        // 当前对话的索引
    private bool isTyping = false;      // 是否正在打字
    private bool skipText = false;      // 是否跳过当前文本

    public CountdownItemCollector itemCollector;
    void Start()
    {
        // 初始化对话内容
        dialogueLines = new string[]
        {
            "你终于来了",
            "学校现在正受到外部的攻击！",
            "请尽快找到合适的道具，并前往支援！",
            "赶快行动吧",
            "任务 ：寻找4个收集品",
        };

        // 显示对话面板并开始显示第一句对话
        dialogueUI.SetActive(true);
        ShowDialogue();
    }

    void Update()
    {
        // 监听鼠标点击事件，如果正在打字并点击，则跳过当前文本
        if (Input.GetMouseButtonDown(0) && !isTyping)
        {
            if (currentLine < dialogueLines.Length)
            {
                ShowDialogue();
            }
            else {
                itemCollector.enabled = true;
                dialogueUI.SetActive(false);
            }
        }
        else if (Input.GetMouseButtonDown(0) && isTyping)
        {
            // 跳过当前打字动画，直接显示完整的文本
            skipText = true;
        }
    }

    void ShowDialogue()
    {
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = "";  // 清空现有文本
            StartCoroutine(TypeSentence(dialogueLines[currentLine]));  // 开始逐字显示
            currentLine++;  // 显示下一句
        }
        else
        {
            // 如果所有对话都已经显示完，隐藏对话面板
            dialogueUI.SetActive(false);
            itemCollector.enabled = true;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        // 逐字显示文本
        foreach (char letter in sentence.ToCharArray())
        {
            if (skipText)
            {
                // 如果跳过当前文本，则直接显示完整文本
                dialogueText.text = sentence;
                skipText = false;
                break;
            }
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
