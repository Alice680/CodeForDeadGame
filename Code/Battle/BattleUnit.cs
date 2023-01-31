using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BattleUnit
{
    private BattleManager battle_manager;

    private static int id_counter;
    private int id;

    public BattlePlayer player;
    private BattleAI ai;

    private Human human;
    private Monster monster;

    private Coordinate cord;

    private GameObject model;

    private int hp;
    private int ap;
    private int mp;
    private int sp;

    public BattleUnit(BattleManager manager, BattleActor actor, Mob mob, Coordinate spawn)
    {
        id = id_counter++;

        battle_manager = manager;

        if (actor is BattlePlayer)
            player = (BattlePlayer)actor;
        else
            ai = (BattleAI)actor;

        if (mob is Human)
        {
            human = (Human)mob;
            model = GameObject.Instantiate(human.GetCombatSprite());

            hp = human.GetHp();
        }
        else
        {
            monster = (Monster)mob;
        }

        SetPosition(spawn);
    }

    /** Core mathods **/
    /*Run*/
    public bool Run()
    {
        if (player != null)
            return player.Run();
        return ai.Run();
    }

    /*Delete*/
    public void Remove()
    {
        GameObject.Destroy(model);
    }


    /** Set Data **/
    /* Universal */
    public void SetPosition(Coordinate c)
    {
        cord = c;
        model.transform.position = battle_manager.CordToWorld(c);
    }

    public void Damage(int damage)
    {
        hp -= damage;
    }

    public void Heal(int heal)
    {

    }

    /* Human */
    public void RegenAP()
    {
        ap += human.GetApRegen();

        if (ap > human.GetAp())
            ap = human.GetAp();
    }

    public void SpendAP(int cost)
    {
        ap -= cost;

        if (ap < 0)
        {
            Damage(-ap);
            ap = 0;
        }
    }


    /** Get Data **/
    /*Universal*/
    public int GetID()
    {
        return id;
    }

    public bool IsID(int i)
    {
        return id == i;
    }

    public bool IsActor(BattleActor a)
    {
        if (IsPlayer())
            return player.Equals(a);
        else
            return ai.Equals(a);
    }

    public bool IsPlayer()
    {
        return player != null;
    }

    public bool IsHuman()
    {
        return human != null;
    }

    public string GetName()
    {
        if (IsHuman())
            return "human";
        else
            return "monster";
    }

    public int GetHP()
    {
        return hp;
    }

    public int GetMaxHP()
    {
        if (IsHuman())
            return human.GetHp();
        else
            return 0;
    }

    public int GetSpd()
    {
        if (IsHuman())
            return human.GetSpd();
        else
            return 0;
    }

    public int GetMov()
    {
        if (IsHuman())
            return human.GetMov();
        else
            return 0;
    }

    public int GetAct()
    {
        if (IsHuman())
            return human.GetAct();
        else
            return 0;
    }

    public GameObject GetSprite()
    {
        return human.GetCombatSprite();
    }

    public GameObject GetTurnIcon()
    {
        return human.GetTurnicon();
    }

    public GameObject GetActorIcon()
    {
        if (IsPlayer())
            return player.GetIcon();
        else
            return ai.GetIcon();
    }

    public MoveType GetMoveType()
    {
        return MoveType.Land;
    }

    public Attack GetAttack(int i = -1)
    {
        if (human != null)
            return human.GetBaseAttack();
        else
            return null;
    }

    public Coordinate GetPosition()
    {
        return cord;
    }

    /*Human*/
    public int GetAP()
    {
        return ap;
    }

    public int GetAPRegen()
    {
        return human.GetApRegen();
    }

    public int GetMaxAP()
    {
        return human.GetAp();
    }

    public int GetDmg()
    {
        if (!IsHuman())
            return 0;

        return human.GetDmg();
    }

    public int GetDef()
    {
        if (!IsHuman())
            return 0;

        return human.GetDef();
    }

    /* Overides */
    public override bool Equals(object other)
    {
        if (other == null || !(other is BattleUnit))
            return false;

        return id.Equals(((BattleUnit)other).id);
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override string ToString()
    {
        return "ID:" + id;
    }
}