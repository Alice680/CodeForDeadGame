using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TraversabilityChecker
{
    public static MovePace Check(GroundType g, MoveType m)
    {
        switch (g)
        {
            case GroundType.Soil:
                if (m == MoveType.Land)
                    return MovePace.Full;
                break;

            case GroundType.Gras:
                if (m == MoveType.Land)
                    return MovePace.Full;
                break;
        }
        return MovePace.None;
    }
}