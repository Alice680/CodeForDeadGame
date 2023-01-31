using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleActor
{
    protected BattleManager gm;
    protected GameObject icon;

    public BattleActor(BattleManager g, GameObject ga)
    {
        gm = g;
        icon = ga;
    }

    public virtual bool Run() { return true; }

    public GameObject GetIcon()
    {
        return icon;
    }
}