using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TempData", menuName = "Dev/ObjectSave", order = 1)]
public class ObjectSave : ScriptableObject
{
    public EventData event_data;

    public NextOverworldData next_overworld;

    public OverworldPlayerData overworld_player_data;
    public BattlePlayerData battle_player_data;
}