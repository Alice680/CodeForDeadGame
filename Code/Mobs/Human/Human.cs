using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Human/Unit", order = 1)]
public class Human : Mob
{
    /* Set */
    [SerializeField] string human_name;

    //0 Charter, 1 Core Class, 2 Tammer Class
    [SerializeField] private int[] lv = new int[3];

    [SerializeField] private CoreClass c_class;
    [SerializeField] private TamerClass t_class;

    [SerializeField] private GenericTraits generic_traits;

    //1 class main 2-4 class, 5-7 tamer, 8-10 generic, 11-14 gear
    [SerializeField] private Trait[] traits = new Trait[14];

    //Gear
    [SerializeField] private Summoner summoner;
    [SerializeField] private Weapon weapon;
    [SerializeField] private Armor armor;
    [SerializeField] private Accessory accessory_a;
    [SerializeField] private Accessory accessory_b;

    /* Derived */
    //0 Max hp, 1 Atk, 2 Def, 3 Spd, 4 Mov, 5 Act
    [SerializeField] private int[] stats = new int[6];

    [SerializeField] private MoveType move_type;

    [SerializeField] private Attack basic_attack;
    [SerializeField] private Attack[] special_attacks = new Attack[8];

    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject turnicon;

    /* Core Methods  */
    public void Generate(int i)
    {
        Clear();

        basic_attack = (Attack)Resources.Load("MobDefault/Punch");
        sprite = (GameObject)Resources.Load("MobDefault/Classless");
        turnicon = (GameObject)Resources.Load("MobDefault/IClassless");

        for (int I = 0; I < traits.Length; ++I)
            traits[I] = generic_traits.GetEmptyTrait();

        CalculateStats();

        UnityEditor.EditorUtility.SetDirty(this);
    }

    private void CalculateStats()
    {
        traits[10] = weapon.GetTrait();
        traits[11] = armor.GetTrait();
        traits[12] = accessory_a.GetTrait();
        traits[13] = accessory_b.GetTrait();

        int[] stat_mod = new int[6] { 0, 0, 0, 0, 0, 0 };
        int[] stat_mult = new int[4] { 100, 100, 100, 100 };

        if (c_class != null)
        {
            int[] temp = c_class.GetStats(lv[0], lv[1]);

            for (int i = 0; i < 4; ++i)
                stat_mod[i] += temp[i];

            move_type = MoveType.Land;
        }

        for (int i = 0; i < 14; ++i)
            if (traits[i] != null)
            {
                for (int I = 0; I < 6; ++I)
                    stat_mod[I] += traits[i].GetEffectStatsIncrease()[I];

                for (int I = 0; I < 4; ++I)
                    stat_mult[I] += traits[i].GetEffectStatsMultiplier()[I];
            }

        stats[1] += weapon.GetStat();
        stats[2] += armor.GetStat();
        stats[3] += accessory_a.GetStat() + accessory_b.GetStat();

        for (int i = 0; i < 4; ++i)
            stats[i] = (stat_mod[i] + (2 * lv[0]) + 10) * stat_mult[i] / 100;

        stats[4] = 4 + stat_mod[4];
        stats[5] = 1 + stat_mod[5];


        basic_attack = weapon.GetAttack();

        int attack_num = 0;

        foreach (Trait t in traits)
        {
            if(t.GetAttackFromEffect() != null)
            {
                special_attacks[attack_num] = t.GetAttackFromEffect();
                ++attack_num;
            }
        }

        special_attacks[attack_num] = armor.GetAttack();
    }

    private void Clear()
    {
        for (int i = 0; i < 3; ++i)
            lv[i] = 0;

        for (int i = 0; i < 6; ++i)
            stats[i] = 0;

        c_class = null;
        t_class = null;

        basic_attack = null;

        for (int i = 0; i < 14; ++i)
            traits[i] = null;

        sprite = null;
        turnicon = null;
    }

    /* Classes */
    public void SetCoreClass(CoreClass type, int rank)
    {
        c_class = null;
        lv[1] = 0;

        basic_attack = (Attack)Resources.Load("MobDefault/Punch");
        sprite = (GameObject)Resources.Load("MobDefault/Classless");
        turnicon = (GameObject)Resources.Load("MobDefault/IClassless");

        for (int i = 0; i < 4; ++i)
            traits[i] = generic_traits.GetEmptyTrait();

        CalculateStats();

        if (type == null)
            return;

        c_class = type;
        lv[1] = rank;

        traits[0] = c_class.GetMainTrait(lv[0]);

        sprite = c_class.GetSprite();
        turnicon = c_class.GetTurnicon();

        CalculateStats();
    }

    public void SetCTrait(int slot, int trait)
    {
        if (slot < 0 || slot > 2 || trait < -1 || trait > 24 || c_class == null)
            return;

        if (trait == -1)
            traits[slot + 1] = generic_traits.GetEmptyTrait();
        else if (trait <= 20)
            traits[slot + 1] = c_class.GetLevelTrait(trait);
        else
            traits[slot + 1] = c_class.GetUnlockTrait(trait - 21);

        CalculateStats();
    }

    public void RemoveCTrait(int slot)
    {
        if (slot < 0 || slot > 2)
            return;

        traits[1 + slot] = null;
    }

    public void SetTamerClass(TamerClass c, int i)
    {
        t_class = c;
        CalculateStats();
    }

    public void SetTTrait(int slot, int trait)
    {
        if (slot < 0 || slot > 2 || trait < -1 || trait > 24 || c_class == null)
            return;

        if (trait == -1)
            traits[slot + 1] = null;
        else if (trait <= 20)
            traits[slot + 1] = c_class.GetLevelTrait(trait);
        else
            traits[slot + 1] = c_class.GetUnlockTrait(trait - 21);

        CalculateStats();
    }

    /* Gear */
    public void SetGear(Gear gear, int type)
    {
        if (type == 0)
            summoner = (Summoner)gear;
        else if (type == 1)
            weapon = (Weapon)gear;
        else if (type == 2)
            armor = (Armor)gear;
        else if (type == 3)
            accessory_a = (Accessory)gear;
        else if (type == 4)
            accessory_b = (Accessory)gear;

        CalculateStats();
    }

    public Gear GetGear(int type)
    {
        return null;
    }

    /* Names */
    public string GetHumanName() { return human_name; }

    public string GetCClassName()
    {
        if (c_class != null)
            return c_class.GetClassName();
        return "None";
    }

    public string GetTClassName()
    {
        if (t_class != null)
            return ""; // add in method
        return "None";
    }

    /* Levels */
    public int GetLevel() { return lv[0]; }

    public int GetCRank() { return lv[1]; }

    public int GetTRank() { return lv[2]; }

    /* Sttats */
    public int GetHp() { return stats[0]; }

    public int GetAp() { return 10; }

    public int GetApRegen() { return 2; }

    public int GetDmg() { return stats[1]; }

    public int GetDef() { return stats[2]; }

    public int GetSpd() { return stats[3]; }

    public int GetMov() { return stats[4]; }

    public int GetAct() { return stats[5]; }

    /* Sprites */
    public GameObject GetCombatSprite() { return sprite; }

    public GameObject GetTurnicon() { return turnicon; }

    /* Other */
    public MoveType GetMoveType() { return move_type; }

    public Attack GetBaseAttack() { return basic_attack; }

    public Attack GetSpecialAttack() { return null; }
}