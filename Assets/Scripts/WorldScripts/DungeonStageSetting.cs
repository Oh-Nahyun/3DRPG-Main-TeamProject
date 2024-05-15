using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStageSetting : MonoBehaviour
{
    public NPCBase dungeonNPC;

    public GameObject exitZone;

    public ParticleSystem[] winEffect;

    private void Start()
    {
        foreach (var item in winEffect)
        {
            item.Stop();
        }
    }

    private void Update()
    {
        if(dungeonNPC.id == 5001) // NPC�� ��ȭ�� �������� ��Ż Ȱ��ȭ
        {
            exitZone.SetActive(true);
            
            foreach(var item in winEffect)
            {
                item.Play();
            }
        }
    }
}