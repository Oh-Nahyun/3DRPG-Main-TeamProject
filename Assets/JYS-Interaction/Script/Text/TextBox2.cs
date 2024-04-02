using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBox2 : MonoBehaviour
{
    
    Dictionary<int, string[]> talkData;

    public float alphaChangeSpeed = 5.0f;

    TextMeshProUGUI talkText;
    TextMeshProUGUI nameText;
    CanvasGroup canvasGroup;
    Image endImage;
    public GameObject scanObject;

    TextSelect textSelet;
    Interaction interaction;

    public string talkString;
    public int talkIndex = 0;
    public float charPerSeconds = 0.05f;

    private bool talkingEnd;
    private bool talking;
    private bool typingTalk;
    private bool typingStop;
    public bool onScanObject;

    //private bool isNpc;

    public NPCBase NPCdata;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(1);
        talkText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        nameText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        endImage = child.GetComponent<Image>();

        child = transform.GetChild(4);
        textSelet = child.GetComponent<TextSelect>();

        interaction = FindObjectOfType<Interaction>();

        talkData = new Dictionary<int, string[]>();
        StopAllCoroutines();
        GenerateData();
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if(scanObject != null) 
        {
            GameManager_JYS.Instance.onTalkNPC += () =>
            {      
                Action();
            };
        }

        /*
        GameManager_JYS.Instance.onTalkObj += () =>
        {
            //isNpc = false;
            ObjAction();
        };*/
}

    private void Update()
    {
        if (interaction != null)
        {
            scanObject = interaction.scanIbgect; // scanIbgect ���� ������
        }

    }

    public void Action()
    {
        talkText.text = "";
        nameText.text = "";
        //scanObject = gameObject;
        if (typingTalk == false)
        {
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 0f);
            StartCoroutine(TalkStart());
        }
        else
        {
            typingStop = true;
            //StopCoroutine(TypingText(talkText.text));
            NPCdata = scanObject.GetComponent<NPCBase>();
            if (!talkingEnd)
            {
                talkIndex--;
            }
            Talk(NPCdata.id);
            nameText.text = $"{NPCdata.nameNPC}";
            //talkText.text = $"{talkString}";
            endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);
            typingTalk = false;
        }
    }

    public void ObjAction()
    {
        
        talkText.text = "";
        nameText.text = "";
        if (typingTalk == false)
        {
            StartCoroutine(TalkStart());
        }
        else
        {
            typingStop = true;
            NPCdata = scanObject.GetComponent<NPCBase>();
            if (!talkingEnd)
            {
                talkIndex--;
            }
            Talk(NPCdata.id);
            typingTalk = false;
        }
    }

    IEnumerator TalkStart()
    {

        if (!talking && !talkingEnd)
        {
            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            NPCdata = scanObject.GetComponent<NPCBase>();
            Talk(NPCdata.id);

            nameText.text = $"{NPCdata.nameNPC}";
            talkText.text = $"{talkString}";

            if (NPCdata.selectId)
            {
                textSelet.onSeletStart();
            }
            else
            {
                textSelet.onSeletEnd();
            }


            StartCoroutine(TypingText(talkText.text));

        }
        else if (talking && !talkingEnd) 
        {
            Talk(NPCdata.id);

            nameText.text = $"{NPCdata.nameNPC}";
            talkText.text = $"{talkString}";

            if (NPCdata.selectId)
            {
                textSelet.onSeletStart();
            }
            else
            {
                textSelet.onSeletEnd();
            }

            StartCoroutine(TypingText(talkText.text));
        }
        else
        {
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
            talkingEnd = false;

        }
    }

    /// <summary>
    /// �ؽ�Ʈ Ÿ���� ȿ��
    /// </summary>
    /// <param name="text"></param>
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
                endImage.color = new Color(endImage.color.r, endImage.color.g, endImage.color.b, 1f);
                typingTalk = false;
            }
        }
    }

    void Talk(int id)
    {
        if ((talkIndex + 1) == talkData[id].Length)
        {
            talkString = talkData[id][talkIndex];
            talkingEnd = true;
            return;
        }
        talkString = talkData[id][talkIndex];
        talking = true;
        talkIndex++;
    }

    void GenerateData()
    {
        talkData.Add(0, new string[] { "�ʱⰪ" });
        talkData.Add(100, new string[] { "�������� ȹ���ߴ�." });
        talkData.Add(110, new string[] { "�̹� �������� ȹ���� �����̴�." });

        talkData.Add(1000, new string[] { "�ֱ����� �� �״�� '���� ����ϴ� �뷡'�� ���Ѵ�.", "1896�� '�����Ź�' â���� ���� ���� ������ �ֱ��� ���簡 �Ź��� ����Ǳ� �����ߴµ�", "�� �뷡���� � ������ �ҷ��°��� ��Ȯ���� �ʴ�.", "�ٸ� ���������� ������ ���Ǵ븦 ������ 1902�� '�������� �ֱ���'��� �̸��� ������ �����" ," ������ �ֿ� ��翡 ����ߴٴ� ����� ���ݵ� ���� �ִ�." });
        talkData.Add(1010, new string[] { "�������"});
        talkData.Add(1011, new string[] { "������ 11 ���ÿϷ�", "AAAAA" });
        talkData.Add(1012, new string[] { "������ 12 ���ÿϷ�", "BBBBB" });
        talkData.Add(1013, new string[] { "������ 13 ���ÿϷ�", "CCCCC" });

        talkData.Add(1020, new string[] { "�ٴ������" });
        talkData.Add(1021, new string[] { "������ 21 ���ÿϷ�", "AAAAA" });
        talkData.Add(1022, new string[] { "������ 22 ���ÿϷ�", "BBBBB" });
        talkData.Add(1023, new string[] { "������ 23 ���ÿϷ�", "CCCCC" });

        talkData.Add(1100, new string[] { "������ ���� �������" });
        talkData.Add(1110, new string[] { "������ �ִ� �ٴ������" });
        talkData.Add(1111, new string[] { "������ 111 ���ÿϷ�", "AAAAA" });
        talkData.Add(1112, new string[] { "������ 112 ���ÿϷ�", "BBBBB" });
        talkData.Add(1113, new string[] { "������ 113 ���ÿϷ�", "CCCCC" });
        talkData.Add(1200, new string[] { "������ ���� �ٴ������" });

        talkData.Add(2000, new string[] { "�����ٶ󸶹ٻ�  ������īŸ����  �����ٶ󸶹ٻ�  ������īŸ����  �����ٶ󸶹ٻ�  ������īŸ����" });
    }

    public void OnSelect(int selectId)
    {
        NPCdata.id += selectId;
        talkingEnd = false;
        Action();
        textSelet.onSeletEnd();
    }


   
} 
