using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMapGenerator : MonoBehaviour
{
    public bool load_in_data;

    public Camera cam;

    public OverworldMapLayout layout;

    private int x_size;
    private int x_offset;

    private int y_size;
    private int y_offset;

    public string tile_name;
    public TileType tile_type;

    private GameObject[,] tiles;

    public int encounter_num;

    void Start()
    {
        x_size = layout.GetXSize();
        y_size = layout.GetYSize();
        x_offset = layout.GetXOffset();
        y_offset = layout.GetYOffset();

        if (load_in_data)
            Load();
        else
            Generate();

        for (int i = x_size; i >= 0; --i)
            for (int I = y_size; I >= 0; --I)
            {
                Debug.DrawLine(new Vector2(i - 0.5f, I - 0.5f), new Vector2(i + 0.5f, I - 0.5f), Color.black, Mathf.Infinity);
                Debug.DrawLine(new Vector2(i - 0.5f, I - 0.5f), new Vector2(i - 0.5f, I + 0.5f), Color.black, Mathf.Infinity);
            }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            LoadTile(GetMouseCordinate(), tile_name, tile_type);

        if (Input.GetMouseButton(1))
            Debug.Log(GetMouseCordinate());

        if (Input.GetKey(KeyCode.E))
            LoadEncounter(GetMouseCordinate(), encounter_num);

        if (Input.GetKey(KeyCode.Q))
            RemoveEncounter(GetMouseCordinate(), encounter_num);

        if (Input.GetKey(KeyCode.I))
            ShowData();

    }

    /* Startup */
    private void Load()
    {
        layout.Load();

        tiles = new GameObject[x_size, y_size];

        for (int i = 0; i < x_size; ++i)
            for (int I = 0; I < y_size; ++I)
                LoadTile(new Coordinate(i, I), layout.GetTileName(new Coordinate(i, I)), layout.GetTileType(new Coordinate(i, I)));
    }

    private void Generate()
    {
        layout.Setup();

        tiles = new GameObject[x_size, y_size];

        for (int i = 0; i < x_size; ++i)
            for (int I = 0; I < y_size; ++I)
                LoadTile(new Coordinate(i, I), tile_name, tile_type);
    }

    private void ShowData()
    {
        for (int i = x_size - 1; i >= 0; --i)
            for (int I = y_size - 1; I >= 0; --I)
            {
                if (layout.GetTileType(new Coordinate(i, I)) == TileType.Grnd)
                    Debug.DrawLine(new Vector2(i - 0.5f, I - 0.5f), new Vector2(i + 0.5f, I + 0.5f), Color.green);
                else if (layout.GetTileType(new Coordinate(i, I)) == TileType.Wall)
                    Debug.DrawLine(new Vector2(i - 0.5f, I - 0.5f), new Vector2(i + 0.5f, I + 0.5f), Color.red);
                else if (layout.GetTileType(new Coordinate(i, I)) == TileType.Watr)
                    Debug.DrawLine(new Vector2(i - 0.5f, I - 0.5f), new Vector2(i + 0.5f, I + 0.5f), Color.blue);

                if(layout.HasEncounter(new Coordinate(i, I)))
                    Debug.DrawLine(new Vector2(i - 0.5f, I + 0.5f), new Vector2(i + 0.5f, I - 0.5f), Color.white);
            }
    }

    /* Run Time */
    private void LoadTile(Coordinate cord, string tilename, TileType tiletype)
    {
        if (cord.GetX() < 0 || cord.GetY() < 0 || cord.GetX() >= x_size || cord.GetY() >= y_size)
            return;

        if (tiles[cord.GetX(), cord.GetY()] != null)
            GameObject.Destroy(tiles[cord.GetX(), cord.GetY()]);

        layout.SetTile(cord, tilename, tiletype);

        tiles[cord.GetX(), cord.GetY()] = (GameObject)GameObject.Instantiate(Resources.Load("OverworldMapSprites/Tiles/" + tilename), CordToWorld(cord), new Quaternion());
    }

    private void LoadEncounter(Coordinate cord, int area)
    {
        if (cord.GetX() < 0 || cord.GetY() < 0 || cord.GetX() >= x_size || cord.GetY() >= y_size)
            return;

        layout.SetEncounter(cord, area);
    }

    private void RemoveEncounter(Coordinate cord, int area)
    {
        if (cord.GetX() < 0 || cord.GetY() < 0 || cord.GetX() >= x_size || cord.GetY() >= y_size)
            return;

        layout.RemoveEncounter(cord, area);
    }

    /* Coordinate Calcs */
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