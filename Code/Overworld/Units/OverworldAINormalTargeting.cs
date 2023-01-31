using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "OverworldAI/NormalTargating", order = 5)]
public class OverworldAINormalTargeting : OverworldAICore
{
    [SerializeField] private int event_num;
    [SerializeField] private int[] texts; //Walk, Talk, Post Fight, Post Fight Talk

    public override void RunAwaken(OverworldManager manager, OverworldUnit unit)
    {

    }

    public override void RunMove(OverworldManager manager, OverworldUnit unit)
    {
        if (DataCore.event_data.GetNPCState(event_num) == 0)
            manager.OpenText(texts[0]);
    }
    public override void RunInteract(OverworldManager manager, OverworldUnit unit)
    {
        if (DataCore.event_data.GetNPCState(event_num) == 0)
            manager.OpenText(texts[1]);
    }
}