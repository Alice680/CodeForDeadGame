using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Human/GenericTraits", order = 8)]
public class GenericTraits : ScriptableObject
{
    [SerializeField] private Trait empty_trait;
    [SerializeField] private Trait[] generic_traits;

    public Trait GetEmptyTrait()
    {
        return empty_trait;
    }

    public Trait GetGenericTrait(int slot)
    {
        return generic_traits[slot];
    }
}