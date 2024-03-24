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
        talkData.Add(100, new string[] { "������� ��¦��¦ ������ ����" });
        talkData.Add(199, new string[] { "�̹� �������� ȹ���� �����̴�" });

        talkData.Add(1000, new string[] { "�ֱ����� �� �״�� '���� ����ϴ� �뷡'�� ���Ѵ�.", "1896�� '�����Ź�' â���� ���� ���� ������ �ֱ��� ���簡 �Ź��� ����Ǳ� �����ߴµ�", "�� �뷡���� � ������ �ҷ��°��� ��Ȯ���� �ʴ�.", "�ٸ� ���������� ������ ���Ǵ븦 ������ 1902�� '�������� �ֱ���'��� �̸��� ������ �����", " ������ �ֿ� ��翡 ����ߴٴ� ����� ���ݵ� ���� �ִ�." });
        talkData.Add(1010, new string[] { "�������" });
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

    public string[] GetTalkData(int id)
    {
        if (talkData.ContainsKey(id))
            return talkData[id];
        else
        {
            Debug.LogError("�ش� ID�� ���� ��ȭ �����͸� ã�� �� �����ϴ�: " + id);
            return null;
        }
    }

}
