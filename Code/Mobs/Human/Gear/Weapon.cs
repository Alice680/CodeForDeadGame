using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Human/Gear/Weapon", order = 1)]
public class Weapon : Gear
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