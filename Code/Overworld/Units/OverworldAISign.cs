using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "OverworldAI/Sign", order = 5)]
public class OverworldAISign : OverworldAICore
{
    public int text_value;

    public override void RunInteract(OverworldManager manager, OverworldUnit unit)
    {
        manager.OpenText(text_value);
    }
}