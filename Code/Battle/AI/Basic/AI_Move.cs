using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Charge", menuName = "AI/Basic/Move", order = 2)]
public class AI_Move : AICore
{
    /* States */
    private enum State { instance, move, end }
    private State state;
    public override void Reset()
    {
        wait_time.Reset();
        base.Reset();
    }

    /* Preset data */
    [SerializeField] private AICore wait_time;

    public override bool Run(BattleManager manager, BattleAI parent)
    {
        switch (state)
        {
            case State.instance:
                Instance(manager, parent);
                return false;
            case State.move:
                Move(manager, parent);
                return false;
            case State.end:
                End(manager, parent);
                return true;
        }
        return false;
    }

    private void Instance(BattleManager manager, BattleAI parent)
    {
        state = State.move;
    }

    private void Move(BattleManager manager, BattleAI parent)
    {
        if (!wait_time.Run(manager, parent))
            return;

        BattlePathfinder.Path path = manager.GetPath(manager.GetPositionFromID(manager.GetIDFromActive()), GetNearEnemy(manager, parent, 1));

        if (path != null)
            manager.MoveUnit(path.GetNodeAtValue(manager.GetMaxSteps()));

        state = State.end;
    }

    private void End(BattleManager manager, BattleAI parent)
    {
        state = State.instance;
    }
}