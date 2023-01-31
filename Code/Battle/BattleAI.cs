using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAI : BattleActor
{
    private AICore ai;

    public BattleAI(BattleManager g, GameObject ga, AICore a) : base(g, ga)
    {
        ai = a;
        ai.Reset();
    }

    public override bool Run() 
    { 
        return ai.Run(gm, this); 
    }
}