using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPlayer
{
    private enum State { idle, moving };

    private State state;

    private float input_time;

    private KeyboardInputter inputter;

    private GameObject player_sprite;

    private OverworldManager manager;

    private Coordinate position;

    private int direction;

    private TileMove move_type;

    public OverworldPlayer(OverworldManager manager, Coordinate position, int rotation)
    {
        this.manager = manager;

        player_sprite = (GameObject)GameObject.Instantiate(Resources.Load("PlayerModel"));
        player_sprite.transform.position = manager.CordToWorld(position);

        inputter = new KeyboardInputter();
        input_time = Time.time;

        this.position = position;
        direction = rotation;
        move_type = TileMove.Land;

        state = State.idle;
    }

    public void Run()
    {
        inputter.Run();

        switch (state)
        {
            case State.idle:
                Idle();
                break;
            case State.moving:
                Moving();
                break;
        }
    }

    /* Get Data */
    public Coordinate GetPosition()
    {
        return position;
    }

    public int GetRotation()
    {
        return 0;
    }

    /* states */
    private void Idle()
    {
        if (inputter.GetEnter())
        {
            int x = 0, y = 0;

            if (direction % 2 == 0)
                x = direction == 2 ? 1 : -1;
            else
                y = direction == 1 ? 1 : -1;


            manager.InteractPlayer(new Coordinate(position, new Coordinate(x, y)));
            return;
        }

        if (inputter.GetExit())
        {
            //menu
            return;
        }

        if (inputter.GetDir() != 0)
        {
            int dir = inputter.GetDir();
            int x = 0;
            int y = 0;

            if (dir == 1)
                y = 1;
            else if (dir == 2)
                x = 1;
            else if (dir == 3)
                y = -1;
            else if (dir == 4)
                x = -1;

            direction = dir;

            if (manager.CanMovePlayer(x, y, position, move_type))
            {
                input_time = Time.time;

                position.Add(x, y);

                player_sprite.transform.position = manager.CordToWorld(position);

                state = State.moving;
            }
        }
    }

    private void Moving()
    {
        //Vector2 start_pos = manager.CordToWorld();
        //player_sprite.transform.position = Vector2.Lerp( );
        if (Time.time - input_time > 0.15F)
        {
            manager.EnterPlayer(position);
            state = State.idle;
        }
    }
}