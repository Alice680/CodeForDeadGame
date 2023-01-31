using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Monster/Unit", order = 1)]
public class Monster : Mob
{
    //
    [SerializeField] private int lv;

    //
    [SerializeField] private int[] stats = new int[10];

    //
    [SerializeField] private EvolutionBase[] chosen_evolution = new EvolutionBase[3];

    //
    [SerializeField] private Move basic_move;
    [SerializeField] private Move[] advanced_move = new Move[8];

    //
    [SerializeField] private Ability[] abilities = new Ability[3];

    //
    [SerializeField] private HeldItem[] items = new HeldItem[2];
}
