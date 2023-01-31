using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBattleData
{
    private int index;

    public NextBattleData()
    {
        index = 0;
    }

    public NextBattleData(int index)
    {
        this.index = index;
    }

    public BattleMapLayout GetLayout()
    {
        return ((SceneHolder)Resources.Load("GenericData/Scenes")).GetBattleMapLayout(index);
    }
}
