using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Human/Attack", order = 4)]
public class Attack : ScriptableObject
{
    [Serializable]
    private enum EffectType { TrueDamage, PercentDamage, ScalingDamage, Heal, Buff, Debuff };
    [Serializable]
    private struct Target
    {
        public Targeting targets;

        public MapShape range;
        public MapShape area_of_effect;
    }

    [Serializable]
    private struct Effect
    {
        public EffectType effect_type;

        public int i;
        public int[] i_array;
    }

    [SerializeField] private GameObject sprite;

    [SerializeField] private int cost;

    [SerializeField] private Target target;

    [SerializeField] private Effect[] effects;

    public bool HasAttackArea()
    {
        return target.targets == Targeting.Area;
    }

    public int GetCost()
    {
        return cost;
    }

    public List<Coordinate> GetAttackArea()
    {
        return target.range.GetArea();
    }

    public List<Coordinate> GetDamageArea()
    {
        return target.area_of_effect.GetArea();
    }

    public List<Coordinate> GetTargets()
    {
        return null;
    }

    public GameObject GetAnimation()
    {
        return sprite;
    }

    /* Perform Attack */
    public void Hit(BattleUnit unit)
    {
        foreach (Effect effect in effects)
        {
            switch (effect.effect_type)
            {
                case EffectType.TrueDamage:
                    unit.Damage(effect.i);
                    break;
            }
        }
    }
}