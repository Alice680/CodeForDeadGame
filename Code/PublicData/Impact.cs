using System;
using UnityEngine;

[Serializable]
public struct Impact
{
    public enum ImpactType { none, buff, debuff, regen, evade };
    public enum ImpactTarget { self, other, all_other, all };

    public ImpactType type;

    public ImpactTarget target;

    public bool b;

    public int i1;
    public int i2;
}
