using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NextOverworldData
{
    [SerializeField] private int index;

    [SerializeField] private bool fresh_scene;

    [SerializeField] private int location;

    [SerializeField] private Coordinate position;
    [SerializeField] private int rotation;

    //Store Data
    public NextOverworldData(int index, int locations)
    {
        this.index = index;
        fresh_scene = true;
        this.location = locations;
        this.position = null;
        this.rotation = 0;
    }

    public NextOverworldData(int index, Coordinate position, int rotation)
    {
        this.index = index;
        fresh_scene = false;
        this.location = -1;
        this.position = new Coordinate(position);
        this.rotation = rotation;
    }

    //Retrive
    public OverworldMapLayout GetLayout()
    {
        return ((SceneHolder)Resources.Load("GenericData/Scenes")).GetOverworldMapLayout(index);
    }

    public int GetIndex()
    {
        return index;
    }

    public bool IsFreshScene()
    {
        return fresh_scene;
    }

    public int Location()
    {
        return location;
    }

    public Coordinate Position()
    {
        return position;
    }

    public int Rotation()
    {
        return rotation;
    }

    //Copy
    public NextOverworldData Copy()
    {
        if (fresh_scene)
            return new NextOverworldData(index, location);
        else
            return new NextOverworldData(index, position, rotation);
    }
}