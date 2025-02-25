using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxItem : MonoBehaviour
{
    Player player;

    public float alphaChangeSpeed = 5.0f;
    TextMeshProUGUI talkText;
    TextMeshProUGUI nameText;
    TextMeshProUGUI itemCountText;
    CanvasGroup canvasGroup;
    Image endImage;
    Image itemIcon;
    public GameObject scanObject;
    Animator animator;
    Animator endImageAnimator;
    PlayerController controller;

    Interaction interaction;
    public string talkString;
    public int talkIndex = 0;
    public float charPerSeconds = 0.05f;

    private bool talking;
    public bool Talking => talking;

    public NPCBase NPCdata;
    ChestBase Chestdata;
    SkillDownloader skillDownloader;
    Inventory inventory;
    TextBoxManager textBoxManager; // TextBoxManager에 대한 참조

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

        child = transform.GetChild(6);
        itemIcon = child.GetComponent<Image>();

        child = transform.GetChild(7);
        itemCountText = child.GetComponent<TextMeshProUGUI>();

        // TextBoxManager에 대한 참조 가져오기
        textBoxManager = FindObjectOfType<TextBoxManager>();      
    }

    private void OnEnable()
    {
        //interaction = GameManager.Instance.Player.GetComponent<Interaction>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (interaction != null)
        {
            scanObject = interaction.scanIbgect; // scanIbgect 값을 가져옴
        }
    }

    /// <summary>
    /// textboxitem 초기화 함수
    /// </summary>
    public void InitializeTextBoxItem()
    {
        gameObject.SetActive(true);
        interaction = GameManager.Instance.Player.GetComponent<Interaction>();
        textBoxManager = FindObjectOfType<TextBoxManager>();


        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        endImageAnimator.speed = 0.0f;

        controller = GameManager.Instance.Player.gameObject.GetComponent<PlayerController>();
        controller.onInteraction += () =>
        {
            if (scanObject != null)
            {
                Action();
            }
        };
    }

    /// <summary>
    /// 상호작용 입력시 대상 분별 함수
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
    /// 대화 시작, 진행, 종료 코루틴
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

                if (Chestdata != null)
                {
                    Chestdata.lightParticle.Play();
                    itemIcon.sprite = Chestdata.scriptableObject.itemIcon;
                    nameText.text = $"{Chestdata.scriptableObject.itemName}";
                    talkText.text = $"{Chestdata.scriptableObject.desc}";
                    itemCountText.text = $"X {Chestdata.itemCount}";

                    if (Chestdata.itemCount > 1)
                    {
                        itemCountText.gameObject.SetActive(true);
                    }
                    else
                    {
                        itemCountText.gameObject.SetActive(false);
                    }

                    if (inventory == null)
                    {
                        player = GameManager.Instance.Player;
                        inventory = player.Inventory;
                    }
                    inventory.AddSlotItem((uint)Chestdata.scriptableObject.itemCode, Chestdata.itemCount);
                }

                skillDownloader = scanObject.GetComponent<SkillDownloader>();
                if (skillDownloader != null)
                {                    
                    itemIcon.sprite = skillDownloader.sprite;
                    nameText.text = $"{skillDownloader.nameNPC}";
                    talkText.text = $"{talkString}";
                    itemCountText.gameObject.SetActive(false);
                }
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
                if (Chestdata != null)
                {
                    Chestdata.lightParticle.Stop();
                }
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
            Chestdata = null;
            skillDownloader = null;
            scanObject = null;
        }
    }

    /// <summary>
    /// 다음 대화 내용 불러오는 함수
    /// </summary>
    /// <param name="id">대화 대상의 ID</param>
    void Talk(int id)
    {
        talkString = textBoxManager.GetTalkData(id)[talkIndex];
        talking = true;
    }


}
