using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TempData", menuName = "Save/Text", order = 5)]
public class TextHolder : ScriptableObject
{
    [SerializeField] private OverworldText[] text;

    public OverworldText GetText(int index)
    {
        return text[index];
    }
}