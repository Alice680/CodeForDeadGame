using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : BattleActor
{
    /* Data Types */
    protected struct PlayerInputs
    {
        public bool enter;
        public bool cancel;
        public int dir;
    }

    private struct Values
    {
        public Coordinate start;
        public Coordinate current;

        public float last_input;

        public int battle_menu_value;
    }

    private enum State { setup, menu, move, attack, end }

    /* Data Storage */
    private string[] battle_menu_options = new string[8] { "MOV", "ATK", "SUM", "STA", "ITM", "SPC", "EVO", "END" };

    /* Variables */
    private int current_id;

    private State state = new State();

    protected PlayerInputs inputs;

    private Values values;

    //private List<Coordinate> attack_area = new List<Coordinate>();

    public BattlePlayer(BattleManager g, GameObject gm) : base(g, gm)
    {

    }

    /*State Machine*/
    public override bool Run()
    {
        switch (state)
        {
            case State.setup:
                Setup();
                return false;
            case State.menu:
                Menu();
                return false;
            case State.move:
                Move();
                return false;
            case State.attack:
                Attack();
                return false;
            case State.end:
                End();
                return true;
        }
        return false;
    }

    /* Basic States */
    private void Setup()
    {
        gm.SetMenu(true);
        gm.UpdateMinorStatsUI();
        gm.SetActionCounter(-1);
        gm.SetStepCounter(-1);
        gm.SetCam(gm.GetPositionFromID(gm.GetIDFromActive()));

        gm.OpenBattleMenu(battle_menu_options, 0);

        current_id = gm.GetIDFromActive();

        values.battle_menu_value = 0;

        state = State.menu;
        return;
    }

    private void End()
    {
        state = State.setup;
    }

    /* Menu */
    private void StartMenu()
    {
        values.start = null;
        values.current = null;

        gm.SetCam(gm.GetPositionFromID(gm.GetIDFromActive()));
        gm.SetActionCounter();
        gm.ClearOverlayMap();
        gm.ClearTrail();

        gm.OpenBattleMenu(battle_menu_options, values.battle_menu_value);

        state = State.menu;
    }

    private void Menu()
    {
        if (inputs.dir != 0 && Time.time - values.last_input > 0.1F)
        {
            values.battle_menu_value = UpdateMenu(values.battle_menu_value, inputs.dir);
            gm.SetBattleMenu(values.battle_menu_value);

            values.last_input = Time.time;
        }

        if (inputs.enter)
        {
            switch (values.battle_menu_value)
            {
                case 0:
                    StartMove();
                    break;
                case 1:
                    StartAttack();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    state = State.end;
                    break;
            }
        }
    }

    /* Move */
    private void StartMove()
    {
        gm.CloseBattleMenu();

        values.start = gm.GetPositionFromID(gm.GetIDFromActive());
        values.current = gm.GetPositionFromID(gm.GetIDFromActive());

        gm.AddToOverlayMap(values.current, 1);

        state = State.move;
    }

    private void Move()
    {
        //gm.SetStatsText(gm.GetIDFromPosition(gm.GetMouseCordinate()), 1);

        if (inputs.enter)
        {
            BattlePathfinder.Path path = gm.GetPath(values.start, values.current);

            if (path == null)
                return;

            Coordinate temp;

            if (path.GetValue() > CalculateTotalSteps())
                temp = path.GetNodeAtValue(CalculateTotalSteps());
            else
                temp = values.current;

            if (gm.MoveUnit(temp))
                StartMenu();
            return;
        }

        if (inputs.cancel)
        {
            StartMenu();
            return;
        }

        if (inputs.dir != 0 && Time.time - values.last_input > 0.05F)
        {
            if (!gm.WithinMap(IncrementPosition(values.current, inputs.dir)))
                return;

            values.last_input = Time.time;

            values.current = IncrementPosition(values.current, inputs.dir);

            gm.ClearTrail();
            gm.ClearOverlayMap();

            gm.AddToOverlayMap(values.start, 0);
            gm.AddToOverlayMap(values.current, 1);

            gm.SetCam(values.current);

            BattlePathfinder.Path path = gm.GetPath(values.start, values.current);

            if (path != null)
            {
                gm.GenerateTrail(path, Mathf.Min(CalculateTotalSteps(), path.GetValue()));

                gm.SetStepCounter(CalculateStepsLeft(path.GetValue()));
                gm.SetActionCounter(CalculateActionsLeft(path.GetValue()));

                if (path.GetValue() > CalculateTotalSteps())
                    gm.AddToOverlayMap(path.GetNodeAtValue(CalculateTotalSteps()), 3);
            }
            else
            {
                gm.AddToOverlayMap(values.current, 2);

                gm.SetStepCounter();
                gm.SetActionCounter();
            }

            return;
        }
    }

    /* Attack */
    private void StartAttack()
    {
        gm.CloseBattleMenu();

        values.start = gm.GetPositionFromID(gm.GetIDFromActive());
        values.current = gm.GetPositionFromID(gm.GetIDFromActive());

        gm.AddToOverlayMap(values.current, 1);

        bool inrange = false;

        foreach (Coordinate area in gm.GetAttackAreaFromID(gm.GetIDFromActive()))
        {
            area.Add(gm.GetPositionFromID(gm.GetIDFromActive()));
            gm.AddToOverlayMap(area, 3);
            if (area.Equals(values.current))
                inrange = true;
        }

        if (inrange)
        {
            foreach (Coordinate area in gm.GetDamageAreaFromID(gm.GetIDFromActive()))
            {
                area.Add(values.current);
                gm.AddToOverlayMap(area, 2);
            }
        }

        state = State.attack;
    }

    private void Attack()
    {
        if (inputs.enter)
        {
            if (gm.Attack(values.current))
                StartMenu();
            return;
        }

        if (inputs.cancel)
        {
            StartMenu();
            return;
        }

        if (inputs.dir != 0 && Time.time - values.last_input > 0.05F)
        {
            if (!gm.WithinMap(IncrementPosition(values.current, inputs.dir)))
                return;

            values.last_input = Time.time;

            values.current = IncrementPosition(values.current, inputs.dir);

            gm.ClearTrail();
            gm.ClearOverlayMap();
            gm.AddToOverlayMap(values.current, 1);
            gm.SetCam(values.current);

            bool inrange = false;

            foreach (Coordinate area in gm.GetAttackAreaFromID(gm.GetIDFromActive()))
            {
                area.Add(gm.GetPositionFromID(gm.GetIDFromActive()));
                gm.AddToOverlayMap(area, 3);
                if (area.Equals(values.current))
                    inrange = true;
            }

            if (inrange)
            {
                foreach (Coordinate area in gm.GetDamageAreaFromID(gm.GetIDFromActive()))
                {
                    area.Add(values.current);
                    gm.AddToOverlayMap(area, 2);
                }
            }
        }
    }

    /* Math */
    private int UpdateMenu(int value, int input)
    {
        if (input == 1 || input == 3)
        {
            if (value < 4)
                value += 4;
            else
                value -= 4;
        }
        else if (input == 2)
        {
            value += 1;

            if (value == 8)
                value = 0;
        }
        else
        {
            value -= 1;

            if (value == -1)
                value = 7;
        }

        return value;
    }

    private Coordinate IncrementPosition(Coordinate c, int dir)
    {
        if (dir == 1)
            return new Coordinate(c.GetX(), c.GetY() + 1);
        else if (dir == 2)
            return new Coordinate(c.GetX() + 1, c.GetY());
        else if (dir == 3)
            return new Coordinate(c.GetX(), c.GetY() - 1);
        else if (dir == 4)
            return new Coordinate(c.GetX() - 1, c.GetY());

        return new Coordinate(-1, -1);
    }

    private int CalculateTotalSteps()
    {
        return gm.GetSteps() + (gm.GetActions() * gm.GetMoveFromID(current_id));
    }

    private int CalculateStepsLeft(int i)
    {
        int temp = gm.GetActions();
        i = gm.GetSteps() - i;

        while (i < 0)
        {
            i += gm.GetMoveFromID(current_id);
            --temp;
        }

        if (temp < 0)
            return 0;
        return i;
    }

    private int CalculateActionsLeft(int i)
    {
        int temp = gm.GetActions();
        i = gm.GetSteps() - i;

        while (i < 0)
        {
            --temp;
            i += gm.GetMoveFromID(current_id);
        }

        return Mathf.Max(0, temp);
    }
}