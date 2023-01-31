using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "OverworldAI/Core", order = 5)]
public class OverworldAICore : ScriptableObject
{
    [SerializeField] private GameObject model;

    [SerializeField] private Coordinate[] FixedMoveTrigger;
    [SerializeField] private Coordinate[] FreeMoveTrigger;
    [SerializeField] private Coordinate[] FixedInteractTrigger;
    [SerializeField] private Coordinate[] FreeInteractTrigger;

    public GameObject GetModel()
    {
        return (GameObject)GameObject.Instantiate(model);
    }

    /* Run AI */
    public virtual void RunAwaken(OverworldManager manager, OverworldUnit unit)
    {

    }

    public virtual void RunPassive(OverworldManager manager, OverworldUnit unit)
    {

    }

    public virtual void RunMove(OverworldManager manager, OverworldUnit unit)
    {

    }

    public virtual void RunInteract(OverworldManager manager, OverworldUnit unit)
    {

    }

    /* Get Location */
    public Coordinate[] GetFixedMoveTrigger()
    {
        Coordinate[] cords = new Coordinate[FixedMoveTrigger.Length];

        for (int i = 0; i < FixedMoveTrigger.Length; ++i)
            cords[i] = new Coordinate(FixedMoveTrigger[i]);

        return cords;
    }

    public Coordinate[] GetFreeMoveTrigger()
    {
        Coordinate[] cords = new Coordinate[FreeMoveTrigger.Length];

        for (int i = 0; i < FreeMoveTrigger.Length; ++i)
            cords[i] = new Coordinate(FreeMoveTrigger[i]);

        return cords;
    }

    public Coordinate[] GetFixedInteractTrigger()
    {
        Coordinate[] cords = new Coordinate[FixedInteractTrigger.Length];

        for (int i = 0; i < FixedInteractTrigger.Length; ++i)
            cords[i] = new Coordinate(FixedInteractTrigger[i]);

        return cords;
    }

    public Coordinate[] GetFreeInteractTrigger()
    {
        Coordinate[] cords = new Coordinate[FreeInteractTrigger.Length];

        for (int i = 0; i < FreeInteractTrigger.Length; ++i)
            cords[i] = new Coordinate(FreeInteractTrigger[i]);

        return cords;
    }
}