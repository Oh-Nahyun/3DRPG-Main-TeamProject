using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBase : NPCBase
{
    readonly int IsOpenHash = Animator.StringToHash("Open");
    public ParticleSystem lightParticle;

    // Scriptable Object�� ������ �ʵ�
    public ItemData scriptableObject;

    /// <summary>
    /// ���ھ��� ������ ����
    /// </summary>
    [Tooltip("������ ���� �Է�")]
    public int itemCount = 1;

    protected override void Awake()
    {
        base.Awake();
        isTextObject = true;
        isNPC = false;
    }

    protected override void Start()
    {
        base.Start();
        lightParticle = GetComponentInChildren<ParticleSystem>();
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
            animator.SetBool(IsOpenHash, true);
            id = 199;
            gameObject.tag = "Untagged";
        }
    }


}