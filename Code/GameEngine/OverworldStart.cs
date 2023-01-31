using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OS", menuName = "GameData/OverworldStart", order = 0)]
public class OverworldStart : ScriptableObject
{
    [SerializeField] private bool reload_map;

    [SerializeField] private OverworldMapLayout layout;

    [SerializeField] private int spawn_point;

    [SerializeField] private Coordinate spawn_location;
    [SerializeField] private int spawn_rotation;

    public OverworldMapLayout GetLayout()
    {
        return layout;
    }

    public bool SceneReloaded()
    {
        return reload_map;
    }

    public int GetSpawnPoint()
    {
        return spawn_point;
    }

    public Coordinate GetSpawnLocation()
    {
        return spawn_location;
    }

    public int GetSpawnRotation()
    {
        return spawn_rotation;
    }

    public void NewScene(OverworldMapLayout layout, int point)
    {
        reload_map = false;
        this.layout = layout;
        spawn_point = point;
    }

    public void StoreScene(Coordinate positon, int rotation)
    {
        reload_map = true;
        spawn_location = positon;
        spawn_rotation = rotation;
    }
}