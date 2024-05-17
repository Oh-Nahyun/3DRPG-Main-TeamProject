using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDownloader : NPCBase
{
    public Sprite sprite;


    protected override void Awake()
    {
        base.Awake();
        isTextObject = true;
        isNPC = false;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        OpenChest(isTalk);
    }

    /// <summary>
    /// ���ڸ� ������ �� ó���ϴ� �Լ�
    /// </summary>
    /// <param name="isOpen">���� ����</param>
    private void OpenChest(bool isOpen)
    {
        if (isOpen)
        {
            if(id == 301)
            {
                GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.RemoteBomb_Cube);
                GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.RemoteBomb);
                Debug.Log("��������ź ���");
            }
            else if (id == 302)
            {
                GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.IceMaker);
                Debug.Log("���̽�����Ŀ ���");
            }
            else if (id == 303)
            {
                GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.MagnetCatch);
                Debug.Log("���׳�ĳġ ���");
            }
            else if (id == 304)
            {
                GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.TimeLock);
                Debug.Log("Ÿ�ӷ� ���");
            }
            gameObject.layer = 0;
        }
    }
}
