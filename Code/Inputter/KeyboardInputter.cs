using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputter
{
    private int dir;
    private bool enter;
    private bool exit;

    public KeyboardInputter()
    {
        dir = 0;
        enter = false;
        exit = false;
    }

    public void Run()
    {
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Return))
            enter = true;
        else
            enter = false;

        if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.Backspace))
            exit = true;
        else
            exit = false;

        if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0)
        {
            if (Random.Range(0, 2) == 1)
            {
                if (Input.GetAxisRaw("Horizontal") == 1)
                    dir = 2;
                else
                    dir = 4;
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") == 1)
                    dir = 1;
                else
                    dir = 3;
            }
        }
        else if (Input.GetAxisRaw("Horizontal") != 0 ^ Input.GetAxisRaw("Vertical") != 0)
        {
            if (Input.GetAxisRaw("Vertical") == 1)
                dir = 1;
            else if (Input.GetAxisRaw("Horizontal") == 1)
                dir = 2;
            else if (Input.GetAxisRaw("Vertical") == -1)
                dir = 3;
            else if (Input.GetAxisRaw("Horizontal") == -1)
                dir = 4;
        }
        else
        {
            dir = 0;
        }
    }

    public int GetDir()
    {
        return dir;
    }

    public bool GetEnter()
    {
        return enter;
    }

    public bool GetExit()
    {
        return exit;
    }
}
