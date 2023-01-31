using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Text", menuName = "Text", order = 7)]
public class OverworldText : ScriptableObject
{
    public enum InstanceType { Jump, Split, End }

    [Serializable] private enum DataChangeType { npc }

    [Serializable]
    private struct TextInstance
    {
        public InstanceType type;

        public string entity_name;
        public string body;

        public int option_a;
        public int option_b;

        public bool has_battle;

        public int battle_num;

        public DataChange[] data_change;

        public void ChangeData()
        {
            if (data_change != null)
                foreach (DataChange data in data_change)
                    data.Run();
        }
    }

    [Serializable]
    private struct DataChange
    {
        public DataChangeType data_type;
        public int slot;
        public int value;

        public void Run()
        {
            switch (data_type)
            {
                case DataChangeType.npc:
                    DataCore.event_data.SetNPCState(slot, value);
                    break;
            }
        }
    }

    [SerializeField] private TextInstance[] text_instance;

    public InstanceType GetInstanceType(int index)
    {
        return text_instance[index].type;
    }

    public string GetName(int index)
    {
        return text_instance[index].entity_name;
    }

    public string GetBody(int index)
    {
        return text_instance[index].body;
    }

    public int GetNext(int index, bool next)
    {
        if (next)
            return text_instance[index].option_a;
        else
            return text_instance[index].option_b;
    }

    public int GetBattle(int index)
    {
        if (text_instance[index].has_battle)
            return text_instance[index].battle_num;
        else
            return -1;
    }

    public void UpdateData(int index)
    {
        text_instance[index].ChangeData();
    }
}