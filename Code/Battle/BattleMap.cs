using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMap
{
    private class Node
    {
        public int x;
        public int y;

        public GroundType ground_type;

        public GameObject ground;
        public GameObject effect;
        public GameObject overlay;

        public Node(int i0, int i1)
        {
            x = i0;
            y = i1;
        }

        public override string ToString()
        {
            return "(" + x + "," + y + ")";
        }
    }

    public int length;
    public int height;

    public int x_offset;
    public int y_offset;

    private Node[,] map;

    private List<BattleUnit> units;

    public BattleMap(BattleMapLayout m)
    {
        length = m.GetX();
        height = m.GetY();

        x_offset = m.GetXOff();
        y_offset = m.GetYOff();

        map = new Node[length, height];

        for (int i = 0; i < length; ++i)
            for (int I = 0; I < height; ++I)
            {
                map[i, I] = new Node(i + x_offset, I + y_offset);
                Coordinate temp = new Coordinate(i, I);
                SetTile(temp, m.GetGround(temp), m.GetVariant(temp));
            }

        units = new List<BattleUnit>();
    }

    public void SetTile(Coordinate c, GroundType gt, int i)
    {
        GameObject temp = (GameObject)GameObject.Instantiate(Resources.Load("BattleMapSprites/Tiles/" + gt.ToString() + i), CordToWorld(c), new Quaternion());

        map[c.GetX(), c.GetY()].ground_type = gt;
        map[c.GetX(), c.GetY()].ground = temp;
    }

    public void SetOverlay(Coordinate c, int i)
    {
        if (c == null || !Contains(c) || i < 0)
            return;

        RemoveOverlay(c);
        map[c.GetX(), c.GetY()].overlay = GameObject.Instantiate(Resources.Load<GameObject>("BattleMapSprites/Overlay/" + i), CordToWorld(c), new Quaternion());
    }
    public void RemoveOverlay(Coordinate c)
    {
        if (c == null || !Contains(c))
            return;

        GameObject.Destroy(map[c.GetX(), c.GetY()].overlay);
    }
    public void ClearOverlay()
    {
        for (int i = 0; i < length; ++i)
            for (int I = 0; I < length; ++I)
                RemoveOverlay(new Coordinate(i, I));
    }

    public bool Contains(Coordinate c)
    {
        if (c == null || c.GetX() < 0 || c.GetY() < 0 || c.GetX() >= length || c.GetY() >= height)
            return false;

        return true;
    }

    public void SetTraversability(Coordinate c, GroundType g)
    {
        map[c.GetX(), c.GetY()].ground_type = g;
    }
    public MovePace CheckTraversability(Coordinate c, BattleUnit u)
    {
        if (HasUnit(c) && !u.Equals(GetUnit(c)))
            return MovePace.None;

        return TraversabilityChecker.Check(map[c.GetX(), c.GetY()].ground_type, u.GetMoveType());
    }

    public void AddUnit(BattleUnit u)
    {
        units.Add(u);
    }
    public void RemoveUnit(BattleUnit u)
    {
        units.Remove(u);
    }
    public bool HasUnit(Coordinate c)
    {
        foreach (BattleUnit u in units)
            if (u.GetPosition().Equals(c))
                return true;

        return false;
    }
    public BattleUnit GetUnit(Coordinate c)
    {
        foreach (BattleUnit u in units)
            if (u.GetPosition().Equals(c))
                return u;

        return null;
    }

    public Coordinate WorldToCord(Vector2 v)
    {
        return new Coordinate((int)v.x - x_offset, (int)v.y - y_offset);
    }
    public Vector2 CordToWorld(Coordinate c)
    {
        return new Vector2(c.GetX() + x_offset, c.GetY() + y_offset);
    }
}