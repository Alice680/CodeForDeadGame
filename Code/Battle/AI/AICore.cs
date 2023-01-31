using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICore : ScriptableObject
{
    protected enum target { };

    public virtual void Reset()
    {

    }

    public virtual bool Run(BattleManager manager, BattleAI parent)
    {
        return false;
    }

    /* Targating */
    public Coordinate GetNearEnemy(BattleManager manager, BattleAI parent, int range)
    {
        int length = -1;

        Coordinate start = manager.GetPositionFromID(manager.GetIDFromActive());
        Coordinate temp = null;
        Coordinate goal = null;

        foreach (int id in manager.GetIDs())
            if (!manager.OwnsID(parent, id))
                for (int i = -range; i <= range; ++i)
                    for (int I = -range; I <= range; ++I)
                    {
                        if (Mathf.Abs(i) + Mathf.Abs(I) > range)
                            continue;

                        temp = manager.GetPositionFromID(id);
                        temp.Add(i, I);

                        BattlePathfinder.Path path = manager.GetPath(start, temp);

                        if (path != null && (goal == null || path.GetValue() < length))
                        {
                            goal = temp;
                            length = path.GetValue();
                        }
                    }

        return goal;
    }
}