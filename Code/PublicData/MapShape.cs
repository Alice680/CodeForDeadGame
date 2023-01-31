using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Shapes { Point, Cube, Blast, Cross, Corner, Line }

[Serializable]
public class MapShape
{
    public Shapes shape;
    public int size;
    public int length;
    public int rotation;

    public List<Coordinate> GetArea()
    {
        List<Coordinate> list = new List<Coordinate>();

        switch (shape)
        {
            case Shapes.Point:
                list.Add(new Coordinate(0, 0));
                return list;

            case Shapes.Cross:
                for (int i = -size; i <= size; ++i)
                {
                    list.Add(new Coordinate(i, 0));
                    list.Add(new Coordinate(0, i));
                }
                return list;

            case Shapes.Blast:
                for (int i = -size; i <= size; ++i)
                    for (int I = -size; I <= size; ++I)
                        if (Mathf.Abs(i) + Mathf.Abs(I) <= size)
                            list.Add(new Coordinate(i, I));
                return list;
            case Shapes.Line:
                for (int i = -size; i <= size; ++i)
                    for (int I = 0; I < length; ++I)
                    {
                        if (rotation == 1)
                            list.Add(new Coordinate(i, I));
                        else if (rotation == 2)
                            list.Add(new Coordinate(I, i));
                        else if (rotation == 3)
                            list.Add(new Coordinate(i, -I));
                        else if (rotation == 4)
                            list.Add(new Coordinate(-I, i));
                    }
                return list;
        }

        return null;
    }
}
