using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Map/OverworldMapData", order = 1)]
public class OverworldMapLayout : ScriptableObject
{
    [Serializable]
    private struct TileNode
    {
        public TileNode(string tile_name, TileType tile_type)
        {
            this.tile_name = tile_name;
            this.tile_type = tile_type;
        }

        public string tile_name;
        public TileType tile_type;
    }

    [Serializable]
    private struct UnitLocation
    {
        public OverworldAICore ai;
        public Coordinate location;
        public int rotation;
    }

    [Serializable]
    private struct SpawnLocation
    {
        public int x;
        public int y;
        public int rot;

        public Coordinate GetCoordinate()
        {
            return new Coordinate(x, y);
        }
    }

    [Serializable]
    private struct ExitLocation
    {
        public Coordinate location;
        public int map;
        public int spawn_location;
    }

    [Serializable]
    private struct EncounterLocation
    {
        public int encounter_rate;
        public List<Coordinate> locations;
        public int[] battles;
    }

    /* Map Core */
    [SerializeField] private int x_size;
    [SerializeField] private int x_offset;

    [SerializeField] private int y_size;
    [SerializeField] private int y_offset;

    /* Tiles */
    [SerializeField] private TileNode[] tiles;

    /* Player Data */
    [SerializeField] private SpawnLocation[] spawns;

    /* Map Data */
    [SerializeField] private ExitLocation[] exits;
    [SerializeField] private EncounterLocation[] encounters;
    [SerializeField] private UnitLocation[] units;

    /* Set Data */
    public void Setup()
    {
        tiles = new TileNode[x_size * y_size];

        UnityEditor.EditorUtility.SetDirty(this);
    }

    public void Load()
    {
        UnityEditor.EditorUtility.SetDirty(this);
    }

    public void SetTile(Coordinate pos, string tile_name, TileType tile_type)
    {
        if (pos.GetX() < 0 || pos.GetY() < 0 || pos.GetX() >= x_size || pos.GetY() >= y_size)
            return;

        tiles[pos.GetX() + (x_size * pos.GetY())] = new TileNode(tile_name, tile_type);
    }

    public void SetEncounter(Coordinate pos, int index)
    {
        foreach (Coordinate cord in encounters[index].locations)
            if (cord.Equals(pos))
                return;

        encounters[index].locations.Add(pos);
    }

    public bool HasEncounter(Coordinate pos)
    {
        foreach (EncounterLocation encounter in encounters)
            foreach (Coordinate cord in encounter.locations)
                if (cord.Equals(pos))
                    return true;

        return false;
    }

    public void RemoveEncounter(Coordinate pos, int index)
    {
        foreach (Coordinate cord in encounters[index].locations)
            if (cord.Equals(pos))
            {
                encounters[index].locations.Remove(cord);
                return;
            }
    }

    /* Get Tile Data */
    public int GetXSize()
    {
        return x_size;
    }

    public int GetYSize()
    {
        return y_size;
    }

    public int GetXOffset()
    {
        return x_offset;
    }

    public int GetYOffset()
    {
        return y_offset;
    }

    /* Tile data */
    public string GetTileName(Coordinate pos)
    {
        return tiles[pos.GetX() + (x_size * pos.GetY())].tile_name;
    }

    public TileType GetTileType(Coordinate pos)
    {
        return tiles[pos.GetX() + (x_size * pos.GetY())].tile_type;
    }

    public string[] GetTilesNames()
    {
        List<string> list = new List<string>();

        foreach (TileNode node in tiles)
            list.Add(node.tile_name);

        return list.ToArray();
    }

    public TileType[] GetTileTypes()
    {
        List<TileType> list = new List<TileType>();

        foreach (TileNode node in tiles)
            list.Add(node.tile_type);

        return list.ToArray();
    }

    /* Set Units */
    public List<OverworldUnit> SetUnits(OverworldManager manager)
    {
        List<OverworldUnit> unit_list = new List<OverworldUnit>();

        foreach (UnitLocation unit in units)
            unit_list.Add(new OverworldUnit(manager, unit.ai, unit.location, unit.rotation));

        return unit_list;
    }

    public Coordinate GetSpawnPosition(int i)
    {
        if (i < 0 || i > spawns.Length)
            return null;

        return spawns[i].GetCoordinate();
    }

    public int GetSpawnRotation(int i)
    {
        if (i < 0 || i > spawns.Length)
            return -1;

        return spawns[i].rot;
    }

    public OverworldMap SetMap(GameObject cam)
    {
        OverworldMap map = new OverworldMap(GetXSize(), GetYSize(), GetXOffset(), GetYOffset(), GetTilesNames(), GetTileTypes(), cam);

        foreach (ExitLocation exit in exits)
            map.SetTransition(exit.location, exit.map, exit.spawn_location);

        foreach (EncounterLocation encounter in encounters)
            foreach (Coordinate location in encounter.locations)
                map.SetEncounter(location, encounter.encounter_rate, encounter.battles);

        return map;
    }
}