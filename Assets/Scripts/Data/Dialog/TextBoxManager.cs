using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxManager : MonoBehaviour
{
    /// <summary>
    /// ��� ������ ���� ��ųʸ�
    /// </summary>
    Dictionary<int, string[]> talkData;

    private void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    /// <summary>
    /// ��縦 �����ϴ� �Լ�
    /// </summary>
    void GenerateData()
    {
        talkData.Add(0, new string[] { "�ʱⰪ" });
        // ��ü ������Ʈ
        talkData.Add(100, new string[] { "������� ��¦��¦ ������ ����" });
        talkData.Add(199, new string[] { "�̹� �������� ȹ���� �����̴�" });
        talkData.Add(298, new string[] { "���� ���ȴ�" });
        talkData.Add(299, new string[] { "����ִ� ���̴�" });

        // NPC
        // ����
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

        // �ù�
        talkData.Add(2000, new string[] { "�����ٶ󸶹ٻ�  ������īŸ����  �����ٶ󸶹ٻ�  ������īŸ����  �����ٶ󸶹ٻ�  ������īŸ����" });

        // �ù�
        talkData.Add(3000, new string[] { "������ �� ������..." });

        // ����
        talkData.Add(4000, new string[] { "��ɼ�!!" });
        talkData.Add(4010, new string[] { "��ɼ�!!" });
        talkData.Add(4011, new string[] { "����"});
        talkData.Add(4012, new string[] { "�Ǹ�"});
        talkData.Add(4013, new string[] { "�ȳ��� ���ʼ�!!" });

        talkData.Add(4014, new string[] { "�����ϱ�", "�Ǹ��ϱ�","������" });

        // ����
        talkData.Add(5000, new string[] { "....." });
    }

    /// <summary>
    /// �� id�� �ش��ϴ� ��ȭ ���� �������� �Լ�
    /// </summary>
    /// <param name="id">�ش� ������Ʈ�� Id Ű��</param>
    /// <returns></returns>
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