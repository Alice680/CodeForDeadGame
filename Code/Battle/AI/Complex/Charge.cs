using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "AI/Complex/Charge", order = 2)]
public class Charge : AICore
{
    /* States */
    private enum State { instance, move, attack, end }
    private State state;

    /* Preset data */
    [SerializeField] private AICore move;
    [SerializeField] private AICore attack;

    public override void Reset()
    {
        move.Reset();
        attack.Reset();
        state = State.instance;
    }

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
        state = State.move;
    }

    private void Move(BattleManager manager, BattleAI parent)
    {
        if (!move.Run(manager,parent))
            return;

        state = State.attack;
    }

    private void Attack(BattleManager manager, BattleAI parent)
    {
        if (!attack.Run(manager, parent))
            return;

        state = State.end;
    }

    private void End(BattleManager manager, BattleAI parent)
    {
        state = State.instance;
    }
}