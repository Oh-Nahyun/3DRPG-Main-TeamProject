using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDownloader : NPCBase
{

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
            id = 299;
        }
    }
}
