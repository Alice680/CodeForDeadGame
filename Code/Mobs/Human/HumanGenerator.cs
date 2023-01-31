using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gen", menuName = "ScriptableObjects/Human/Generator", order = 2)]
public class HumanGenerator : ScriptableObject
{
    public int level;

    public CoreClass c_class;
    public int c_level;

    public TamerClass t_class;
    public int t_level;

    public void Run(Human h)
    {
        h.Generate(level);

        if (c_class != null)
            h.SetCoreClass(c_class, c_level);

        if (t_class != null)
            h.SetTamerClass(t_class, t_level);
    }
}