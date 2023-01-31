using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BS", menuName = "GameData/BattleStart", order = 2)]
public class BattleStart : ScriptableObject
{
    [SerializeField] private BattleMapLayout layout;

    public void SetLayout(BattleMapLayout layout)
    {
        this.layout = layout;
    }

    public BattleMapLayout GetLayout()
    {
        return layout;
    }
}