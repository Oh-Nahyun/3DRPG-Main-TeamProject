using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpBase : NPCBase
{
    Interaction interaction;

    public Transform warpPoint; // ������ ��ġ
    public Transform player; // ������ ��ġ
    public bool warpReady;

    protected override void Awake()
    {
        isWarp = true;
        interaction = FindObjectOfType<Interaction>();
        player = interaction.gameObject.transform;
    }

    public void WarpToWarpPoint()
    {
        if (warpPoint != null)
        {
            player.position = warpPoint.position;
        }
    }


}
