using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxItem : MonoBehaviour
{
    public float alphaChangeSpeed = 5.0f;
    TextMeshProUGUI talkText;
    TextMeshProUGUI nameText;
    CanvasGroup canvasGroup;
    Image endImage;
    Image itemIcon;
    public GameObject scanObject;
    Animator animator;
    Animator endImageAnimator;

    Interaction interaction;

    public string talkString;
    public int talkIndex = 0;
    public float charPerSeconds = 0.05f;

    private bool talking;

    public NPCBase NPCdata;
    ChestBase Chestdata;
    Inventory inventory;
    TextBoxManager textBoxManager; // TextBoxManager�� ���� ����

    readonly int IsOnTextBoxItemHash = Animator.StringToHash("OnTextBoxItem");

    private void Awake()
    {

        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();

        Transform child = transform.GetChild(2);
        talkText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        nameText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(4);
        endImage = child.GetComponent<Image>();
        endImageAnimator = child.GetComponent<Animator>();

        interaction = FindObjectOfType<Interaction>();

        child = transform.GetChild(6);
        itemIcon = child.GetComponent<Image>();

        // TextBoxManager�� ���� ���� ��������
        textBoxManager = FindObjectOfType<TextBoxManager>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        endImageAnimator.speed = 0.0f;

        GameManager.Instance.onTalkNPC += () =>
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

        if (NPCdata != null && NPCdata.isTextObject)
        {
            StartCoroutine(TalkStart());
        }
    }

    /// <summary>
    /// ��ȭ ����, ����, ���� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator TalkStart()
    {
        if (!talking)
        {
            animator.SetBool(IsOnTextBoxItemHash, true);

            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            endImageAnimator.speed = 1.0f;
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);

            Talk(NPCdata.id);
 
            if (NPCdata.isTextObject)
            {
                Chestdata = scanObject.GetComponent<ChestBase>();
                Chestdata.lightParticle.Play();
                itemIcon.sprite = Chestdata.scriptableObject.itemIcon;
                nameText.text = $"{Chestdata.scriptableObject.itemName}";
                talkText.text = $"{Chestdata.scriptableObject.desc}";
                inventory.AddSlotItem(Chestdata.itemCode, Chestdata.itemCount + 1);
            }
            else
            {
                nameText.text = $"{NPCdata.nameNPC}";
                talkText.text = $"{talkString}";
            }
            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }

            NPCdata.isTalk = true;
        }
        else
        {
            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
            if (NPCdata.isTextObject)
            {
                Chestdata = scanObject.GetComponent<ChestBase>();
                Chestdata.lightParticle.Stop();
            }
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            talkText.text = "";
            nameText.text = "";
            talkIndex = 0;
            endImageAnimator.speed = 0.0f;
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 0f);
            talking = false;
            NPCdata.isTalk = false;
            NPCdata.isTextObject = false;
            animator.SetBool(IsOnTextBoxItemHash, false);

        }
    }

    /// <summary>
    /// ���� ��ȭ ���� �ҷ����� �Լ�
    /// </summary>
    /// <param name="id">��ȭ ����� ID</param>
    void Talk(int id)
    {
        talkString = textBoxManager.GetTalkData(id)[talkIndex];
        talking = true;
    }


}
