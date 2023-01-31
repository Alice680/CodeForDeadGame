using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInputKeybored : BattlePlayer
{
    public BattleInputKeybored(BattleManager g, GameObject gm) : base(g, gm)
    {

    }

    public override bool Run()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            inputs.enter = true;
        else
            inputs.enter = false;

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Backspace))
            inputs.cancel = true;
        else
            inputs.cancel = false;

        inputs.dir = 0;

        if (Input.GetAxisRaw("Horizontal") != 0 ^ Input.GetAxisRaw("Vertical") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
                SetHorizontal();
            else
                SetVertical();
        }
        else if(Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0)
        {
            if (Random.Range(0,2) == 1)
                SetHorizontal();
            else
                SetVertical();
        }

        return base.Run();
    }

    private void SetHorizontal()
    {
        if (Input.GetAxisRaw("Horizontal") == 1)
            inputs.dir = 2;
        else
            inputs.dir = 4;
    }

    private void SetVertical()
    {
            if (Input.GetAxisRaw("Vertical") == 1)
                inputs.dir = 1;
            else
                inputs.dir = 3;
    }
}