using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualMobGenerator : MonoBehaviour
{
    public Mob mob;

    public HumanGenerator human;
    public MonsterGenerator monster;

    private void Start()
    {
        if (human != null)
            human.Run((Human)mob);
    }
}
