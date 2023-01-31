using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Idle", menuName = "AI/Idle", order = 1)]
public class AI_Idle : AICore
{
    /* States */
    private enum State { instance, idle, end }
    private State state;

    /* Preset data */
    [SerializeField] private float wait_time;

    /* Variables */
    private float start_time;

    public override void Reset()
    {
        start_time = -wait_time;
        state = State.instance;
    }

    public override bool Run(BattleManager manager, BattleAI parent)
    {
        switch (state)
        {
            case State.instance:
                Instance(manager, parent);
                return false;
            case State.idle:
                Idle(manager, parent);
                return false;
            case State.end:
                End(manager, parent);
                return true;
        }
        return false;
    }

    private void Instance(BattleManager manager, BattleAI parent)
    {
        start_time = Time.time;
        state = State.idle;
    }

    private void Idle(BattleManager manager, BattleAI parent)
    {
        if (Time.time - start_time > wait_time)
            state = State.end;
    }

    private void End(BattleManager manager, BattleAI parent)
    {
        start_time = -wait_time;
        state = State.instance;
    }
}
