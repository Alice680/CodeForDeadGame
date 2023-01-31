using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Human/Trait", order = 5)]
public class Trait : ScriptableObject
{
    [Serializable] private enum Effect_Type { stats_increase, stats_multiplier, GainAttack, OnHit, OnKill };

    [Serializable]
    private class Effect
    {
        public Effect_Type type;

        public Attack attack;

        public int i;
        public int[] i_array;

        public Impact impact;
    }

    [SerializeField] private string trait_name;
    [SerializeField] private string description;

    [SerializeField] private Effect[] effects;

    /* Data */
    public string GetTraitName()
    {
        return trait_name;
    }

    public string GetDescription()
    {
        return description;
    }

    /* Use Trait */
    public int[] GetEffectStatsIncrease()
    {
        foreach (Effect e in effects)
            if (e.type == Effect_Type.stats_increase)
                return e.i_array;

        return new int[6] { 0, 0, 0, 0, 0, 0 };
    }

    public int[] GetEffectStatsMultiplier()
    {
        foreach (Effect e in effects)
            if (e.type == Effect_Type.stats_multiplier)
                return e.i_array;

        return new int[4] { 0, 0, 0, 0 };
    }

    public Attack GetAttackFromEffect()
    {
        foreach (Effect e in effects)
            if (e.type == Effect_Type.GainAttack)
                return e.attack;

        return null;
    }
}