using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math
{
    public static Vector2 restrain(Vector2 v, float x_max, float y_max, float x_min, float y_min)
    {
        v.x = Mathf.Min(v.x, x_max);
        v.x = Mathf.Max(v.x, x_min);

        v.y = Mathf.Min(v.y, y_max);
        v.y = Mathf.Max(v.y, y_min);

        return v;
    }

    public static int drop(float f)
    {
        if (f >= -0)
            return (int)f;
        else
            return ((int)f) - 1;
    }

    public static int LoopValue(int i, int max)
    {
        if (i < 0)
            return max;
        else if (i > max)
            return 0;
        else
            return i;
    }
}