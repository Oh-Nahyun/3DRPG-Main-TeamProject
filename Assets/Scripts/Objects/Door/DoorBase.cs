using System.Collections.Generic;
using UnityEngine;

public class DoorBase : NPCBase
{
    readonly int IsOpenHash = Animator.StringToHash("Open");
    public bool open = false;
    public bool isLock = false;

    protected override void Awake()
    {
        base.Awake();
        isNPC = false;
        otherObject = true;
        id = 299;
    }

    protected override void Start()
    {
        base.Start();
        if (isLock)
        {
            otherObject = false;
        }
        else
        {
            otherObject = true;
        }
    }

    /// <summary>
    /// �� ���� ó���Լ�
    /// </summary>
    public void OpenDoor()
    {
        if (otherObject)
        {
            open = !open;
            animator.SetBool(IsOpenHash, open);

            if (open)
            {
                gameObject.tag = "DoorOpen";
            }
            else
            {
                gameObject.tag = "DoorClose";
            }
        }
    }

    protected override void Update()
    {

        if (isLock)
        {
            otherObject = false;
        }
        else
        {
            if (!otherObject)
            {
                {
                    otherObject = true;
                }
            }
        }
    }

}
