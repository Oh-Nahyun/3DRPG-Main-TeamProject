using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    public float alphaChangeSpeed = 5.0f;

    TextMeshProUGUI talkText;
    TextMeshProUGUI nameText;
    CanvasGroup canvasGroup;
    public GameObject scanObject;

    public string talkString;
    public int talkIndex = 1;
    public float charPerSeconds = 0.05f;

    private bool talkingEnd;
    private bool talking;
    private bool typingTalk;
    private bool typingStop;


    public NPCBase NPCdata;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Transform child = transform.GetChild(0);

        talkText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);

        nameText = child.GetComponent<TextMeshProUGUI>();
        talkData = new Dictionary<int, string[]>();

        StopAllCoroutines();
        GenerateData();
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        
        GameManager.Instance.onTalk += () =>
        {
            Action();
        };
       

    }

    public void Action()
    {
        talkText.text = "";
        nameText.text = "";
        //scanObject = gameObject;
        if (typingTalk == false)
        {
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

            StartCoroutine(TypingText(talkText.text));

        }
        else if (talking && !talkingEnd) 
        {
            Talk(NPCdata.id);

            nameText.text = $"{NPCdata.nameNPC}";
            talkText.text = $"{talkString}";

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
        talkData.Add(1000, new string[] { "�ֱ����� �� �״�� '���� ����ϴ� �뷡'�� ���Ѵ�.", "1896�� '�����Ź�' â���� ���� ���� ������ �ֱ��� ���簡 �Ź��� ����Ǳ� �����ߴµ�", "�� �뷡���� � ������ �ҷ��°��� ��Ȯ���� �ʴ�.", "�ٸ� ���������� ������ ���Ǵ븦 ������ 1902�� '�������� �ֱ���'��� �̸��� ������ �����" ," ������ �ֿ� ��翡 ����ߴٴ� ����� ���ݵ� ���� �ִ�." });
        talkData.Add(1010, new string[] { "�������" });
        talkData.Add(2000, new string[] { "�����ٶ󸶹ٻ�  ������īŸ����  �����ٶ󸶹ٻ�  ������īŸ����  �����ٶ󸶹ٻ�  ������īŸ����" });
    }


}
