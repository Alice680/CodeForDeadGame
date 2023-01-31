using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapGen : MonoBehaviour
{
    public Camera cam;

    public bool import;

    public int x_size;
    public int y_size;

    public int x_offset;
    public int y_offset;

    public BattleMapLayout map;
    private BattleMapLayout check;

    public GroundType ground;
    public int variant;

    private GameObject[,] layout;

    private void Start()
    {
        check = map;

        if (import)
            ImportMap();
        else
            SetUpMap();
    }

    private void ImportMap()
    {
        x_size = map.GetX();
        y_size = map.GetY();

        x_offset = map.GetXOff();
        y_offset = map.GetYOff();

        layout = new GameObject[x_size, y_size];

        for (int i = 0; i < x_size; ++i)
            for (int I = 0; I < y_size; ++I)
                PlaceTile(new Coordinate(i, I), map.GetGround(new Coordinate(i, I)), map.GetVariant(new Coordinate(i, I)));
    }

    private void SetUpMap()
    {
        map.Generate(x_size, y_size, x_offset, y_offset);

        layout = new GameObject[x_size, y_size];

        for (int i = 0; i < x_size; ++i)
            for (int I = 0; I < y_size; ++I)
                AddTile(new Coordinate(i, I), ground, variant);
    }

    private void AddTile(Coordinate c, GroundType g, int i)
    {
        if (c.GetX() < 0 || c.GetY() < 0 || c.GetX() >= x_size || c.GetY() >= y_size)
            return;

        int x = c.GetX();
        int y = c.GetY();

        Destroy(layout[x, y]);
        map.SetTile(c, g, i);
        layout[x, y] = (GameObject)Instantiate(Resources.Load("BattleMapSprites/Tiles/" + g.ToString() + i), CordToWorld(c), new Quaternion());
    }
    private void PlaceTile(Coordinate c, GroundType g, int i)
    {
        int x = c.GetX();
        int y = c.GetY();
        Destroy(layout[x, y]);
        layout[x, y] = (GameObject)Instantiate(Resources.Load("BattleMapSprites/Tiles/" + g.ToString() + i), CordToWorld(c), new Quaternion());
    }

    private void Update()
    {
        if (map != check)
            return;

        if (Input.GetMouseButton(0))
            AddTile(GetMouseCordinate(), ground, variant);
    }

    //private void
    private Coordinate GetMouseCordinate()
    {
        Vector3 vec = Input.mousePosition;
        vec.z = 10;
        Vector2 temp = cam.ScreenToWorldPoint(vec);
        return new Coordinate(Math.drop(temp.x + 0.5f) - x_offset, Math.drop(temp.y + 0.5f) - y_offset);
    }
    public Coordinate WorldToCord(Vector2 v)
    {
        return new Coordinate((int)v.x - x_offset, (int)v.y - y_offset);
    }
    private Vector2 CordToWorld(Coordinate c)
    {
        return new Vector2(c.GetX() + x_offset, c.GetY() + y_offset);
    }
}