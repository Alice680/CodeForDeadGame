using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/BattleMapData", order = 1)]
public class BattleMapLayout : ScriptableObject
{
    [Serializable]
    private class Tile
    {
        public GroundType type;
        public int variant;

        public Tile()
        {

        }

        public Tile(GroundType t, int i)
        {
            Debug.Log(i);
            type = t;
            variant = i;
        }
    }
    [Serializable]
    private class Controller
    {
        public AICore actor;
        public GameObject sprite;

        [SerializeField] public Spawn[] spawns;
    }
    [Serializable]
    private class Spawn
    {
        public Coordinate spawn_location;
        public Mob mob;
        public HumanGenerator genA;
    }

    [SerializeField] private int x, y, x_offset, y_offset;

    [SerializeField] private Coordinate player_start;
    [Space(10f)]
    [SerializeField] private Tile[] tiles;
    [SerializeField] private Controller[] controllers;

    /*Set Data*/
    public void Generate(int i1, int i2, int i3, int i4)
    {
        x = i1;
        y = i2;

        x_offset = i3;
        y_offset = i4;

        tiles = new Tile[x * y];

        for (int i = 0; i < x; ++i)
            for (int I = 0; I < y; ++I)
                SetTile(new Coordinate(i, I));

        UnityEditor.EditorUtility.SetDirty(this);
    }

    public void SetTile(Coordinate c, GroundType g = GroundType.Soil, int i = 0)
    {
        tiles[c.GetX() + (c.GetY() * x)] = new Tile(g, i);
    }


    /*Get Data*/
    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public int GetXOff()
    {
        return x_offset;
    }

    public int GetYOff()
    {
        return y_offset;
    }

    public Coordinate GetPlayerStart()
    {
        return player_start;
    }

    public GroundType GetGround(Coordinate c)
    {
        return tiles[c.GetX() + (c.GetY() * x)].type;
    }

    public int GetVariant(Coordinate c)
    {
        return tiles[c.GetX() + (c.GetY() * x)].variant;
    }

    /* Set Up Units */
    public void SetUpUnits(BattleManager manager, List<BattleActor> actors, List<BattleUnit> units, List<Mob> backrow)
    {
        foreach (Controller c in controllers)
        {
            BattleAI ai = new BattleAI(manager, c.sprite, c.actor);
            actors.Add(ai);

            foreach (Spawn s in c.spawns)
                units.Add(new BattleUnit(manager, ai, s.mob, s.spawn_location));
        }
    }
}