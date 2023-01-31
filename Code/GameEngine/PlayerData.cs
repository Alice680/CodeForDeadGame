using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "POD", menuName = "GameData/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Serializable]
    private struct Team
    {
        public Human human_refrence;
        public Monster[] front_line_refrence;
        public Monster[] back_line_refrence;

        public HumanData human_data;
    }

    [Serializable]
    private struct HumanData
    {
        public Trait empty_trait;

        public int current_c_class;

        public CClassData[] c_class_data; //0 Warrior, 1 Archer, 2 Mage

        public EquipmentHolder equipment_holder;
    }

    [Serializable]
    private struct CClassData
    {
        public CoreClass refrence;

        public bool unlocked;

        public int rank;

        public bool[] unlocked_trait;
        public int[] equipped_trait;
    }

    [Serializable]
    private struct GearData
    {
        public Gear refrence;

        public bool unlocked;

        public bool[] upgrades;
    }

    [Serializable]
    private struct EquipmentHolder
    {
        public int[] weapon;
        public int[] armor;

        public int summoner;
        public int accessory_a;
        public int accessory_b;

        public GearData[] weapon_data;
        public GearData[] armor_data;

        public GearData[] summoner_data;
        public GearData[] accessory_data;

    }

    /* Save Data */
    [SerializeField] private Team team;

    /* Reload Data */
    private void ReloadHuman()
    {
        for (int i = 0; i < 3; ++i)
            team.human_refrence.SetCTrait(i, -1);

        if (team.human_data.current_c_class == -1)
        {
            team.human_refrence.SetCoreClass(null, 0);
            return;
        }

        team.human_refrence.SetCoreClass(team.human_data.c_class_data[team.human_data.current_c_class].refrence, team.human_data.c_class_data[team.human_data.current_c_class].rank);

        for (int i = 0; i < 3; ++i)
            team.human_refrence.SetCTrait(i, team.human_data.c_class_data[team.human_data.current_c_class].equipped_trait[i]);

        team.human_refrence.SetGear(team.human_data.equipment_holder.summoner_data[team.human_data.equipment_holder.summoner].refrence, 0);
        team.human_refrence.SetGear(team.human_data.equipment_holder.weapon_data[team.human_data.equipment_holder.weapon[team.human_data.current_c_class]].refrence, 1);
        team.human_refrence.SetGear(team.human_data.equipment_holder.armor_data[team.human_data.equipment_holder.armor[team.human_data.current_c_class]].refrence, 2);
        team.human_refrence.SetGear(team.human_data.equipment_holder.accessory_data[team.human_data.equipment_holder.accessory_a].refrence, 3);
        team.human_refrence.SetGear(team.human_data.equipment_holder.accessory_data[team.human_data.equipment_holder.accessory_b].refrence, 4);
    }

    /* CClassData */
    public void SetCClass(int i)
    {
        team.human_data.current_c_class = i;

        ReloadHuman();
    }

    public void SetCClassTrait(int slot, int num)
    {
        for (int i = 0; i < 3; ++i)
            if (team.human_data.c_class_data[team.human_data.current_c_class].equipped_trait[i] == num)
                team.human_data.c_class_data[team.human_data.current_c_class].equipped_trait[i] = -1;

        team.human_data.c_class_data[team.human_data.current_c_class].equipped_trait[slot] = num;

        ReloadHuman();
    }

    public int GetCurrentCClass()
    {
        return team.human_data.current_c_class;
    }

    public CoreClass GetCClass(int i)
    {
        return team.human_data.c_class_data[i].refrence;
    }

    public bool GetCClassUnlocked(int i)
    {
        return team.human_data.c_class_data[i].unlocked;
    }

    public int GetCClassRank(int i)
    {
        return team.human_data.c_class_data[i].rank;
    }

    public int GetCClassTraitSlot(int clas, int slot)
    {
        return team.human_data.c_class_data[clas].equipped_trait[slot];
    }

    public bool GetCClassTraitUnlocked(int clas, int trait)
    {
        return team.human_data.c_class_data[clas].unlocked_trait[trait];
    }

    /* Gear Data */
    public Summoner GetSummoner(int slot)
    {
        return (Summoner)team.human_data.equipment_holder.summoner_data[slot].refrence;
    }

    public Weapon GetWeapon(int slot)
    {
        return (Weapon)team.human_data.equipment_holder.weapon_data[Mathf.Max(0, slot + (team.human_data.current_c_class * 5))].refrence;
    }

    public Armor GetArmor(int slot)
    {
        return (Armor)team.human_data.equipment_holder.armor_data[Mathf.Max(0, slot + (team.human_data.current_c_class * 5))].refrence;
    }

    public Accessory GetAccessory(int slot)
    {
        return (Accessory)team.human_data.equipment_holder.accessory_data[slot].refrence;
    }

    public bool SummonerUnlocked(int slot)
    {
        return team.human_data.equipment_holder.summoner_data[slot].unlocked;
    }

    public bool WeaponUnlocked(int slot)
    {
        return team.human_data.equipment_holder.weapon_data[Mathf.Max(0, slot + (team.human_data.current_c_class * 5))].unlocked;
    }

    public bool ArmorUnlocked(int slot)
    {
        return team.human_data.equipment_holder.armor_data[Mathf.Max(0, slot + (team.human_data.current_c_class * 5))].unlocked;
    }

    public bool AccessoryUnlocked(int slot)
    {
        return team.human_data.equipment_holder.accessory_data[slot].unlocked;
    }

    public Summoner GetCurrentSummoner()
    {
        return (Summoner)team.human_data.equipment_holder.summoner_data[team.human_data.equipment_holder.summoner].refrence;
    }

    public Weapon GetCurrentWeapon()
    {
        return (Weapon)team.human_data.equipment_holder.weapon_data[Mathf.Max(0, team.human_data.equipment_holder.weapon[team.human_data.current_c_class] + (team.human_data.current_c_class * 5))].refrence;
    }

    public Armor GetCurrentArmor()
    {
        return (Armor)team.human_data.equipment_holder.armor_data[Mathf.Max(0, team.human_data.equipment_holder.armor[team.human_data.current_c_class] + (team.human_data.current_c_class * 5))].refrence;
    }

    public Accessory GetCurrentAccessoryA()
    {

        return (Accessory)team.human_data.equipment_holder.accessory_data[team.human_data.equipment_holder.accessory_a].refrence;
    }

    public Accessory GetCurrentAccessoryB()
    {

        return (Accessory)team.human_data.equipment_holder.accessory_data[team.human_data.equipment_holder.accessory_b].refrence;
    }

    public void SetSummoner(int gear)
    {
        team.human_data.equipment_holder.summoner = gear;
        ReloadHuman();
    }

    public void SetWeapon(int gear)
    {
        team.human_data.equipment_holder.weapon[team.human_data.current_c_class] = gear;
        ReloadHuman();
    }

    public void SetArmor(int gear)
    {
        team.human_data.equipment_holder.armor[team.human_data.current_c_class] = gear;
        ReloadHuman();
    }

    public void SetAccessoryA(int gear)
    {
        team.human_data.equipment_holder.accessory_a = gear;
        ReloadHuman();
    }

    public void SetAccessoryB(int gear)
    {
        team.human_data.equipment_holder.accessory_b = gear;
        ReloadHuman();
    }
}