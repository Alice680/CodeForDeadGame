using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Form", menuName = "ScriptableObjects/Monster/Evolutin/First", order = 1)]
public class FirstEvolution : EvolutionBase
{
    [SerializeField] private int[] base_stats = new int[10];
    [SerializeField] private int[] growth_stats = new int[10];

    [SerializeField] private Move[] base_move = new Move[3];
}