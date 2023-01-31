using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TempData", menuName = "Save/Scene", order = 5)]
public class SceneHolder : ScriptableObject
{
    [SerializeField] private OverworldMapLayout[] overworld_maps;
    [SerializeField] private BattleMapLayout[] battle_maps;

    public OverworldMapLayout GetOverworldMapLayout(int index)
    {
        return overworld_maps[index];
    }

    public BattleMapLayout GetBattleMapLayout(int index)
    {
        return battle_maps[index];
    }
}