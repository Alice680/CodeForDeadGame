using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Attack", menuName = "AI/Basic/Attack", order = 3)]
public class AI_Attack : AICore
{
    /* States */
    private enum State { instance, attack, end }
    private State state;

    /* Preset data */
    [SerializeField] private AICore wait_time;

    public override void Reset()
    {
        wait_time.Reset();
        base.Reset();
    }

    public override bool Run(BattleManager manager, BattleAI parent)
    {
        switch (state)
        {
            case State.instance:
                Instance(manager, parent);
                return false;
            case State.attack:
                Attack(manager, parent);
                return false;
            case State.end:
                End(manager, parent);
                return true;
        }
        return false;
    }

    private void Instance(BattleManager manager, BattleAI parent)
    {
        state = State.attack;
    }

    private void Attack(BattleManager manager, BattleAI parent)
    {
        if (!wait_time.Run(manager, parent))
            return;

        foreach (Coordinate c1 in manager.GetAttackAreaFromID(manager.GetIDFromActive()))
        {
            c1.Add(manager.GetPositionFromID(manager.GetIDFromActive()));
            foreach (Coordinate c2 in manager.GetDamageAreaFromID(manager.GetIDFromActive()))
            {
                c2.Add(c1);
                if (manager.HasUnit(c2) && !manager.OwnsID(parent, manager.GetIDFromPosition(c2)))
                {
                    if (manager.Attack(c2, -1))
                    {
                        state = State.end;
                        return;
                    }
                }
            }
        }

        state = State.end;
    }

    private void End(BattleManager manager, BattleAI parent)
    {
        state = State.instance;
    }
}