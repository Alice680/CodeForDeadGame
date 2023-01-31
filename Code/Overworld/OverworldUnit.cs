using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldUnit
{
    private OverworldManager manager;
    private static int previous_id;

    private int id;

    private OverworldAICore ai;

    private Coordinate position;
    private int rotation;

    private GameObject model;

    public OverworldUnit(OverworldManager man, OverworldAICore ai, Coordinate position, int rotation)
    {
        id = ++previous_id;

        manager = man;

        this.ai = ai;

        this.ai.RunAwaken(manager, this);

        model = ai.GetModel();

        SetPosition(position, rotation);
    }

    public int GetID()
    {
        return id;
    }

    public bool CheckID(int id)
    {
        return this.id == id;
    }

    public void RunPassive()
    {
        ai.RunPassive(manager, this);
    }

    public void RunMoveTrigger()
    {
        ai.RunMove(manager, this);
    }

    public void RunInteractionTrigger()
    {
        ai.RunInteract(manager, this);
    }

    /* Position */
    public Coordinate GetPosition()
    {
        return position;
    }

    public void SetPosition(Coordinate pos, int rot)
    {
        position = pos;
        rotation = rot;

        model.transform.position = manager.CordToWorld(pos);
    }

    public void ChangePosition(int x, int y)
    {
        if (x != 0 && y != 0)
            return;

        int dir = 0;

        if (x != 0)
            dir = x == 1 ? 2 : 4;
        else
            dir = y == 1 ? 1 : 3;

        SetPosition(new Coordinate(position.GetX() + x, position.GetY() + y), dir);
    }

    /* Triggers */
    public Coordinate[] GetMoveTrigger()
    {
        List<Coordinate> cords = new List<Coordinate>();

        foreach (Coordinate cord in ai.GetFixedMoveTrigger())
            cords.Add(cord);

        foreach (Coordinate cord in ai.GetFreeMoveTrigger())
            cords.Add(new Coordinate(cord, position));

        return cords.ToArray();
    }

    public Coordinate[] GetInteractTrigger()
    {
        List<Coordinate> cords = new List<Coordinate>();

        foreach (Coordinate cord in ai.GetFixedInteractTrigger())
            cords.Add(cord);

        foreach (Coordinate cord in ai.GetFreeInteractTrigger())
            cords.Add(new Coordinate(cord, position));

        return cords.ToArray();
    }
}