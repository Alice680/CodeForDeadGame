using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OverworldPlayerData
{
    public GameObject player_model;

    public OverworldPlayerData(GameObject model)
    {
        player_model = model;
    }

    public OverworldPlayerData Copy()
    {
        return new OverworldPlayerData(player_model);
    }
}