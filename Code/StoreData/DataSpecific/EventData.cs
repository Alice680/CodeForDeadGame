using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventData
{
    [SerializeField] private int[] npc_state;
    [SerializeField] private int[] quest_state;

    public EventData(int[] npc, int[] quest)
    {
        npc_state = npc;
        quest_state = quest;
    }

    public int GetNPCState(int index)
    {
        if (index < 0 || index >= npc_state.Length)
            return -1;

        return npc_state[index];
    }

    public void SetNPCState(int index, int value)
    {
        if (index >= 0 && index < npc_state.Length)
            npc_state[index] = value;
    }


    public EventData Copy()
    {
        return new EventData(npc_state, quest_state);
    }
}
