using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PBD", menuName = "GameData/BattleData", order = 3)]
public class PlayerBattleData : ScriptableObject
{
    public GameObject icon;
    public Human player;
    public Monster[] back_row;

    public void SetUpUnits(BattleManager manager, List<BattleActor> actors, List<BattleUnit> units, List<Mob> backrow, Coordinate start)
    {
        BattlePlayer controller = new BattleInputKeybored(manager, icon);

        actors.Add(controller);

        units.Add(new BattleUnit(manager, controller, player, start));

        foreach (Mob m in back_row)
            backrow.Add(m);
    }
}