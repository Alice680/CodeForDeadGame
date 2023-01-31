using System;
using UnityEngine;

[Serializable]
public class Coordinate
{
    [SerializeField] private int x;
    [SerializeField] private int y;

    public Coordinate(int i, int I)
    {
        x = i;
        y = I;
    }

    public Coordinate(Vector2 v)
    {
        x = (int)v.x;
        y = (int)v.y;
    }

    public Coordinate (Coordinate c)
    {
        x = c.GetX();
        y = c.GetY();
    }

    public Coordinate( Coordinate a, Coordinate b)
    {
        x = a.GetX() + b.GetX();
        y = a.GetY() + b.GetY();
    }

    public int GetX() { return x; }
    public int GetY() { return y; }

    public void Add(Coordinate c)
    {
        Add(c.GetX(), c.GetY());
    }
    public void Add(int i1, int i2)
    {
        x += i1;
        y += i2;
    }

    public override bool Equals(object obj)
    {
        if (this == null || obj == null)
            return false;

        return x == ((Coordinate)obj).GetX() && y == ((Coordinate)obj).GetY();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString() { return "(" + x + "," + y + ")"; }
}