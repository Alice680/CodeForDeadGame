using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataCore
{
    public static bool dev_save;

    public static OverworldPlayerData overworld_player_data;
    public static BattlePlayerData battle_player_data;

    public static EventData event_data;

    public static NextOverworldData next_overworld;
    public static NextBattleData next_battle;

    public static void NewData()
    {
        if (dev_save)
        {
            ObjectSave data = (ObjectSave)Resources.Load("TempLoad");
            next_overworld = data.next_overworld.Copy();
            overworld_player_data = data.overworld_player_data.Copy();
            event_data = data.event_data.Copy();

            next_battle = new NextBattleData();
        }
        else
        {

        }
    }

    public static void LoadData()
    {

    }

    public static void SaveData()
    {

    }
}
