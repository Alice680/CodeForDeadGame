using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Human/CClass", order = 2)]
public class CoreClass : ScriptableObject
{
    [SerializeField] private string class_name;

    [SerializeField] private int[] stat_growth = new int[4];
    [SerializeField] private int[] stat_bonus = new int[4];

    [SerializeField] private Trait[] main_traits = new Trait[5];
    [SerializeField] private Trait[] class_traits = new Trait[25];

    [SerializeField] private GameObject spirt;
    [SerializeField] private GameObject turnicon;

    public int[] GetStats(int main_level, int class_level)
    {
        int[] temp = new int[4];

        for (int i = 0; i < 4; ++i)
            temp[i] = (stat_growth[i] * main_level) + (stat_bonus[i] * class_level);

        return temp;
    }

    public Trait GetMainTrait(int i)
    {
        if (i < 0 || i > 20)
            return null;

        return main_traits[i / 5];
    }

    public Trait GetLevelTrait(int i)
    {
        if (i < 0 || i > 20)
            return null;

        return class_traits[i];
    }

    public Trait GetUnlockTrait(int i)
    {
        if (i < 0 || i > 3)
            return null;

        return class_traits[21 + i];
    }

    public string GetClassName()
    {
        return class_name;
    }

    public GameObject GetSprite()
    {
        return spirt;
    }

    public GameObject GetTurnicon()
    {
        return turnicon;
    }
}