using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "ScriptableObjects/Human/Gear/Armor", order = 2)]
public class Armor : Gear
{
    [SerializeField] protected int stat_increase;
    [SerializeField] protected Attack attack;
    [SerializeField] protected Trait trait;

    public int GetStat()
    {
        return stat_increase;
    }

    public Attack GetAttack()
    {
        return attack;
    }

    public Trait GetTrait()
    {
        return trait;
    }
}