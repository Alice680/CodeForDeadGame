using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMap
{
    private class Node
    {
        public GameObject tile_sprite;
        public TileType tile_type;

        public int next_scene;
        public int next_location;

        public int encounter_rate;
        public int[] encounters;

        public Node(int x, int y, string name, TileType type)
        {
            tile_sprite = (GameObject)GameObject.Instantiate(Resources.Load("OverworldMapSprites/Tiles/" + name), new Vector2(x, y), new Quaternion());
            tile_type = type;

            next_scene = -1;
            next_location = 0;

            encounter_rate = 0;
            encounters = new int[0];
        }
    }

    private int x_size;
    private int y_size;

    private int x_offset;
    private int y_offset;

    private Node[,] nodes;

    private OverworldPlayer player;

    private List<OverworldUnit> units;

    private GameObject cam;

    public OverworldMap(int x, int y, int x_off, int y_off, string[] names, TileType[] types, GameObject cam)
    {
        x_size = x;
        y_size = y;

        x_offset = x_off;
        y_offset = y_off;

        nodes = new Node[x, y];

        for (int i = 0; i < x; ++i)
            for (int I = 0; I < y; ++I)
                nodes[i, I] = new Node(i + x_offset, I + y_offset, names[i + (I * x)], types[i + (I * x)]);

        units = new List<OverworldUnit>();

        this.cam = cam;
    }

    /* Check Data */
    public bool Traversable(Coordinate cord, TileMove move_type)
    {
        if (!HasCoordinate(cord))
            return false;

        if (cord.Equals(player.GetPosition()))
            return false;

        foreach(OverworldUnit unit in units)
            if (cord.Equals(unit.GetPosition()))
                return false;

        switch (nodes[cord.GetX(), cord.GetY()].tile_type)
        {
            case TileType.Grnd:
                if (move_type == TileMove.Watr)
                    return false;
                break;
            case TileType.Watr:
                if (move_type == TileMove.Land)
                    return false;
                break;
            case TileType.Wall:
                if (move_type != TileMove.Dev)
                    return false;
                break;
        }

        return true;
    }

    public bool HasCoordinate(Coordinate cord)
    {
        if (cord == null || cord.GetX() < 0 || cord.GetY() < 0 || cord.GetX() >= x_size || cord.GetY() >= y_size)
            return false;
        else
            return true;
    }

    /* Cam Data */
    public void SetCamPosition(Coordinate cord)
    {
        cord = new Coordinate(Mathf.Clamp(cord.GetX(), x_offset + 8, x_size + x_offset - 8), cord.GetY());
        cord = new Coordinate(cord.GetX(), Mathf.Clamp(cord.GetY(), y_offset + 8, y_size + y_offset - 8));

        Vector2 vec = CordToWorld(cord);

        cam.transform.position = new Vector3(vec.x - 0.5F, vec.y - 0.5F, -10);
    }

    /* Scene Transition */
    public void SetTransition(Coordinate cord, int scene, int location)
    {
        nodes[cord.GetX(), cord.GetY()].next_scene = scene;
        nodes[cord.GetX(), cord.GetY()].next_location = location;
    }

    public bool HasTransition(Coordinate cord)
    {
        return nodes[cord.GetX(), cord.GetY()].next_scene != -1;
    }

    public void TriggerTransition(Coordinate cord)
    {
        DataCore.next_overworld = new NextOverworldData(nodes[cord.GetX(), cord.GetY()].next_scene, nodes[cord.GetX(), cord.GetY()].next_location);
    }

    /* Encounters */
    public void SetEncounter(Coordinate cord, int rate, int[] encounters)
    {
        nodes[cord.GetX(), cord.GetY()].encounter_rate = rate;
        nodes[cord.GetX(), cord.GetY()].encounters = encounters;
    }

    public bool TryEncounter(Coordinate cord)
    {
        return Random.Range(0, 100) < nodes[cord.GetX(), cord.GetY()].encounter_rate;
    }

    public void TriggerTransition()
    {

    }

    /* Units */
    public void SetPlayer(OverworldPlayer player)
    {
        this.player = player;
    }

    public void SetUnit(OverworldUnit unit)
    {
        units.Add(unit);
    }

    public void RemoveUnit(OverworldUnit unit)
    {
        units.Remove(unit);
    }

    public OverworldUnit SearchForMoveTrigger(Coordinate cord)
    {
        foreach (OverworldUnit unit in units)
            foreach (Coordinate pos in unit.GetMoveTrigger())
                if (pos.Equals(cord))
                    return unit;

        return null;
    }

    public OverworldUnit SearchForInteractTrigger(Coordinate cord)
    {
        foreach (OverworldUnit unit in units)
            foreach (Coordinate pos in unit.GetInteractTrigger())
                if (pos.Equals(cord))
                    return unit;

        return null;
    }

    /* Convert Cord to World Space */
    public Coordinate WorldToCord(Vector2 vec)
    {
        return new Coordinate((int)vec.x - x_offset, (int)vec.y - y_offset);
    }

    public Vector2 CordToWorld(Coordinate c)
    {
        return new Vector2(c.GetX() + x_offset, c.GetY() + y_offset);
    }
}