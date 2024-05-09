using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpBase : NPCBase
{
    Interaction interaction;

    /// <summary>
    /// ������ ��ġ
    /// </summary>
    public Transform warpPoint;
    public Transform player;

    protected override void Awake()
    {
        otherObject = true;
        interaction = FindObjectOfType<Interaction>();
        player = interaction.gameObject.transform;
    }

    /// <summary>
    /// �÷��̾ ������Ű�� �Լ�
    /// </summary>
    public void WarpToWarpPoint()
    {
        if (warpPoint != null)
        {
            Vector3 warpPosition = warpPoint.position;
            warpPosition.y += player.transform.parent.position.y;
            player.transform.parent.position = warpPosition;
        }
    }



}
