using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public Text dialogueText;
    public float typingSpeed = 0.05f;
    public GameObject dialogueUI;
    private string[] dialogueLines;
    private int currentLine = 0;
    public bool isTyping = false;
    private bool skipText = false;
    public Canvas canvas;
    void Start()
    {
        dialogueText = DialogueText();
        dialogueLines = new string[]
        {
            "    Tips \n按Esc关闭Tips \n  按空格键跳跃\n鼠标右键可以移动视角\nADWS移动上下左右"
        };
        dialogueUI.SetActive(true);
        ShowDialogue();
    }

    // 每帧更新时调用，用于检测用户输入
    void Update()
    {
        // 如果按下Esc键且对话UI处于激活状态，则关闭对话UI
        if (Input.GetKeyDown(KeyCode.Escape) && dialogueUI.activeSelf)
        {
            dialogueUI.SetActive(false);
        }
    }

    // 逐字显示句子的协程
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            // 如果跳过打字，则直接显示完整句子
            if (skipText)
            {
                dialogueText.text = sentence;
                skipText = false;
                break;
            }
            // 逐个添加字符到对话文本
            dialogueText.text += letter;
            // 等待一段时间
            yield return new WaitForSeconds(typingSpeed);
        }
        // 设置打字结束状态
        isTyping = false;
    }

    // 显示对话的方法
    void ShowDialogue()
    {
        // 如果当前行小于对话行数
        if (currentLine < dialogueLines.Length)
        {
            // 逐字显示当前行的句子
            StartCoroutine(TypeSentence(dialogueLines[currentLine]));
            // 增加当前行
            currentLine++;
        }
    }

    // 创建对话文本组件的方法
    public Text DialogueText()
    {

        GameObject textObject = new GameObject("WelcomeText");
        RectTransform rectTransform = textObject.AddComponent<RectTransform>();
        rectTransform.SetParent(canvas.transform);
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.offsetMin = new Vector2(-200, -200);
        rectTransform.offsetMax = new Vector2(200, 200);
        Text text = textObject.AddComponent<Text>();
        text.text = "";
        text.font = Resources.Load<Font>("timesnewarial");
        text.fontSize = 30;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f); // 初始设置为完全不透明，逐字显示完后就固定显示了
        return text;
    }
} 