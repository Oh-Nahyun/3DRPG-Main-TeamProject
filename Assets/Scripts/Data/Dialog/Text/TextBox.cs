using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;


public class TextBox : MonoBehaviour
{
    public float alphaChangeSpeed = 5.0f;

    TextMeshProUGUI talkText;
    TextMeshProUGUI nameText;
    CanvasGroup canvasGroup;
    Image endImage;
    public GameObject scanObject;
    Animator endImageAnimator;
    WarpBase warpBase;

    TextSelect textSelet;
    Interaction interaction;
    PlayerController controller;

    public string talkString;
    public int talkIndex = 0;
    public float charPerSeconds = 0.05f;

    private bool talkingEnd;
    private bool talking;
    public bool TalkingEnd => talkingEnd;

    private bool typingTalk;
    private bool typingStop;

    public NPCBase NPCdata;

    TextBoxManager textBoxManager; // TextBoxManager�� ���� ����
    QuestManager questManager;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(1);
        talkText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        nameText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        endImage = child.GetComponent<Image>();
        endImageAnimator = child.GetComponent<Animator>();

        child = transform.GetChild(4);
        textSelet = child.GetComponent<TextSelect>();

        interaction = FindObjectOfType<Interaction>();

        textBoxManager = FindObjectOfType<TextBoxManager>();
        questManager = FindObjectOfType<QuestManager>();
        warpBase = FindObjectOfType<WarpBase>();

        controller = FindAnyObjectByType<PlayerController>(FindObjectsInactive.Include);
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        endImageAnimator.speed = 0.0f;

        controller.onInteraction += () =>
        {
            if (scanObject != null)
            {
                Action();
            }
        };
    }

    private void Update()
    {
        if (interaction != null)
        {
            scanObject = interaction.scanIbgect; // scanIbgect ���� ������
        }
    }

    /// <summary>
    /// ��ȣ�ۿ� �Է½� ��� �к� �Լ�
    /// </summary>
    public void Action()
    {
        
        talkText.text = "";
        nameText.text = "";
        if (scanObject != null)
        {
            NPCdata = scanObject.GetComponent<NPCBase>();
        }
        else
        {
            NPCdata = null;
        }


        if (typingTalk == false && NPCdata != null && !NPCdata.isTextObject && !NPCdata.otherObject)
        {
            endImageAnimator.speed = 0.0f;
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 0f);
            StartCoroutine(TalkStart());
        }
        else if (typingTalk == true && NPCdata != null && !NPCdata.isTextObject && !NPCdata.otherObject)
        {
            typingStop = true;
            NPCdata = scanObject.GetComponent<NPCBase>();
            if (!talkingEnd)
            {
                talkIndex--;
            }
            Talk(NPCdata.id);
            if (NPCdata.isNPC)
            {
                nameText.text = $"{NPCdata.nameNPC}";
            }
            endImageAnimator.speed = 1.0f;
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);
            typingTalk = false;
        }
        else if (NPCdata != null && NPCdata.otherObject)
        {
            isOtherObject();
        }
        else
        {
            Debug.Log("����� ����");
        }
    }

    /// <summary>
    /// ��ȭ ����, ����, ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator TalkStart()
    {
        if (!talking && !talkingEnd)
        {

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            Talk(NPCdata.id);

            SetTalkText();

            NPCdata.isTalk = true;

            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
        }
        else if (talking && !talkingEnd)
        {
            Talk(NPCdata.id);

            SetTalkText();
        }
        else
        {
            talkingEnd = false;
            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            talkText.text = "";
            nameText.text = "";
            talkIndex = 0;
            talking = false;
            NPCdata.isTalk = false;
        }
    }

    /// <summary>
    /// ��� Ÿ���� ȿ�� �ڷ�ƾ
    /// </summary>
    /// <param name="text">Ÿ���� ȿ���� �� �ؽ�Ʈ</param>
    /// <returns></returns>
    IEnumerator TypingText(string text)
    {
        typingStop = false;
        typingTalk = true;
        talkText.text = null;
        for (int i = 0; i < text.Length; i++)
        {
            if (typingStop)
            {
                talkText.text = $"{talkString}";
                break;
            }
            talkText.text += text[i];
            yield return new WaitForSeconds(charPerSeconds);
            if (i + 2 > text.Length)
            {
                endImageAnimator.speed = 1.0f;
                endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);
                typingTalk = false;
            }
        }
    }

    /// <summary>
    /// �̸� �� ��� ��� �Լ�
    /// </summary>
    void SetTalkText()
    {
        if (talkText != null)
        {
            talkText.text = $"{talkString}";
            if (NPCdata.isNPC)
            {
                nameText.text = $"{NPCdata.nameNPC}";
                StartCoroutine(TypingText(talkText.text));
            }
            else
            {
                endImageAnimator.speed = 1.0f;
                endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);
            }


            if (textBoxManager.GetTalkData(NPCdata.id + 4) != null)
            {
                string buttonText0 = textBoxManager.GetTalkData(NPCdata.id + 4)[0];
                string buttonText1 = textBoxManager.GetTalkData(NPCdata.id + 4)[1];
                string buttonText2 = textBoxManager.GetTalkData(NPCdata.id + 4)[2];

                textSelet.setButtonText(buttonText0, buttonText1, buttonText2);
                textSelet.onSeletStart();

            }
            else
            {
                textSelet.onSeletEnd();
            }


        }
    }

    /// <summary>
    /// ���� ��ȭ ���� �ҷ����� �Լ�
    /// </summary>
    /// <param name="id">��ȭ ����� ID</param>
    void Talk(int id)
    {
        if ((talkIndex + 1) == textBoxManager.GetTalkData(id).Length)
        {
            talkString = textBoxManager.GetTalkData(id)[talkIndex];
            talkingEnd = true;
            return;
        }
        talkString = textBoxManager.GetTalkData(id)[talkIndex];
        talking = true;
        talkIndex++;
    }

    /// <summary>
    /// ������ �޾ƿ��� �Լ�
    /// </summary>
    /// <param name="selectId">�޾ƿ� ������</param>
    public void OnSelect(int selectId)
    {
        NPCdata.id += selectId; // �޾ƿ� �������� ���� Id���� �������� ���� ���� ����
        talkingEnd = false;
        Action();
        textSelet.onSeletEnd();
    }

    /// <summary>
    /// ���â�� ������� �ʴ� ������Ʈ ó�� �Լ�
    /// </summary>
    void isOtherObject()
    {
        warpBase = scanObject.GetComponent<WarpBase>();
        DoorBase door = scanObject.GetComponent<DoorBase>();
        Lever lever = scanObject.GetComponent<Lever>();
        if (warpBase != null)
        {
            Debug.Log("����");
            warpBase.WarpToWarpPoint();
        }

        if (door != null)
        {
            door.OpenDoor();
        }

        if (lever != null)
        {
            lever.Use();
        }
    }

}
