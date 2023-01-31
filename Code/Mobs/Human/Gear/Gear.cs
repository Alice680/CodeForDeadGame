using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : ScriptableObject
{
    [SerializeField] protected string gear_name;

    public string GetName()
    {
        return gear_name;
    }
}