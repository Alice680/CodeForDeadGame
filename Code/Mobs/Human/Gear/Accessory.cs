using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "ScriptableObjects/Human/Gear/Accessory", order = 3)]
public class Accessory : Gear
{
    [SerializeField] protected int stat_increase;
    [SerializeField] protected Trait trait;

    public int GetStat()
    {
        return stat_increase;
    }

    public Trait GetTrait()
    {
        return trait;
    }
}