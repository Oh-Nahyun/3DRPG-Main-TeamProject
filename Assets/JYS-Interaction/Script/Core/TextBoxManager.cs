using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(0, new string[] { "�ʱⰪ" });
        talkData.Add(1000, new string[] { "�ֱ���(����ʰ)�� �� �״�� '���� ����ϴ� �뷡'�� ���Ѵ�.", "1896�� '�����Ź�' â���� ���� ���� ������ �ֱ��� ���簡 �Ź��� ����Ǳ� �����ߴµ�", "�� �뷡���� � ������ �ҷ��°��� ��Ȯ���� �ʴ�.", "�ٸ� ��������(�������)�� ������ ���Ǵ븦 ������ 1902�� '�������� �ֱ���'��� �̸��� ������ ����� ������ �ֿ� ��翡 ����ߴٴ� ����� ���ݵ� ���� �ִ�." });
        talkData.Add(2000, new string[] { "�����ٶ󸶹ٻ�  ������īŸ����  �����ٶ󸶹ٻ�  ������īŸ����  �����ٶ󸶹ٻ�  ������īŸ����" });
    }

    public string GetTalk(int id, int talkIndex)
    {
        return talkData[id][talkIndex];
    }

}
