using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [Serializable]
    private struct TokenHolder
    {

        public GameObject[] step_token;
        public GameObject[] action_token;
    }

    [Serializable]
    private struct BattleMenu
    {
        public GameObject body;
        public GameObject highlight;
        public Text[] text;
    }

    [Serializable]
    private struct MinorStatsMenu
    {
        public Text[] text;
    }

    [Serializable]
    private struct FullHumanStatsMenu
    {

    }

    [Serializable]
    private struct FullMonsterStatsMenu
    {

    }

    /* Battle Data */
    private BattleMapLayout map_layout;

    private BattleMap map;
    private BattlePathfinder pathfinder;

    private BattleTurnKeeper turn;

    private List<BattleActor> actors;
    private List<BattleUnit> units;
    private List<Mob> backrow;

    private List<GameObject> trail;

    private bool victory;
    private bool lose;

    /* Current Turn Data */
    private BattleUnit current_unit;

    private GameObject[] mob_turn_icons;
    private GameObject[] actor_turn_icons;

    private int step_value;
    private int action_value;

    /* UI Refrences */
    [SerializeField] private GameObject menu;

    [SerializeField] private TokenHolder token_holder;
    [SerializeField] private BattleMenu battle_menu;
    [SerializeField] private MinorStatsMenu minor_stats_menu;

    [SerializeField] private Camera cam;

    [SerializeField] private Text[] text;

    /* Setup */
    private void Start()
    {
        DataCore.dev_save = true;
        DataCore.NewData();

        map_layout = DataCore.next_battle.GetLayout();

        trail = new List<GameObject>();

        mob_turn_icons = new GameObject[11];
        actor_turn_icons = new GameObject[11];

        turn = new BattleTurnKeeper();
        map = new BattleMap(map_layout);
        pathfinder = new BattlePathfinder(map);

        actors = new List<BattleActor>();
        units = new List<BattleUnit>();
        backrow = new List<Mob>();

        map_layout.SetUpUnits(this, actors, units, backrow);

        foreach (BattleUnit u in units)
        {
            map.AddUnit(u);
            turn.AddUnit(u);
        }

        //UpdateTurn();
    }

    /* Delegate Controll */
    private void Update()
    {
        //if (current_unit.Run())
            //UpdateTurn();
    }

    /* Actions */
    public bool MoveUnit(Coordinate c)
    {
        if (!map.Contains(c) || map.HasUnit(c))
            return false;

        if (GetPath(current_unit.GetPosition(), c) == null)
            return false;

        int i = GetPath(current_unit.GetPosition(), c).GetValue();

        if (i > step_value + (action_value * current_unit.GetMov()))
            return false;

        step_value -= i;

        while (step_value < 0)
        {
            --action_value;
            step_value += current_unit.GetMov();
        }

        SetPosition(current_unit, c);

        return true;
    }

    public bool Attack(Coordinate target = null, int attack_num = -1)
    {
        if (target == null || action_value == 0 || !map.Contains(target) || !current_unit.IsHuman())
            return false;

        Attack temp_attack = current_unit.GetAttack(-1);

        bool valid_attack = false;

        if (temp_attack.HasAttackArea())
            valid_attack = HitArea(current_unit, target, temp_attack);
        else
            valid_attack = HitTargets();

        if (!valid_attack)
            return false;

        --action_value;

        BurnAther(current_unit, temp_attack.GetCost());

        return true;
    }

    /* Core Methods */
    private void UpdateTurn()
    {
        //Change data to next turn
        SetStepCounter(0);
        SetActionCounter(0);

        SetMenu(false);

        turn.CalculateTurn();

        current_unit = turn.PeakNextTurn();

        step_value = current_unit.GetMov();
        action_value = current_unit.GetAct();

        current_unit = turn.PeakNextTurn();

        //Update Ather
        if (current_unit.IsHuman())
        {
            current_unit.RegenAP();
        }
        else
        {

        }

        //Update UI
        ClearMinorStatsUI();

        SetTurncounter();

        CheckConditions();
    }

    private void SetPosition(BattleUnit u, Coordinate c)
    {
        u.SetPosition(c);

        CheckConditions();
    }

    private bool HitTargets()
    {
        return false;
        //current_unit.SpendAP(temp_attack.GetCost());
    }

    private bool HitArea(BattleUnit caster, Coordinate target, Attack attack)
    {
        foreach (Coordinate c1 in GetAttackAreaFromID(current_unit.GetID()))
        {
            c1.Add(caster.GetPosition());
            if (c1.Equals(target))
            {
                foreach (Coordinate c2 in GetDamageAreaFromID(current_unit.GetID()))
                {
                    c2.Add(target);
                    if (map.Contains(c2))
                    {
                        Instantiate(current_unit.GetAttack().GetAnimation(), map.CordToWorld(c2), new Quaternion());
                        HitUnit(map.GetUnit(c2), attack);
                    }
                }
                return true;
            }
        }
        return false;
        //current_unit.SpendAP(temp_attack.GetCost());
    }

    private void HitUnit(BattleUnit unit, Attack attack)
    {
        if (unit == null || attack == null)
            return;

        attack.Hit(unit);

        if (unit.GetHP() <= 0)
            RemoveUnit(unit);

        CheckConditions();
    }

    private void BurnAther(BattleUnit unit, int i)
    {
        unit.SpendAP(i);

        if (unit.GetHP() < 1)
            RemoveUnit(unit);
    }

    private void RemoveUnit(BattleUnit u)
    {
        map.RemoveUnit(u);
        turn.RemoveUnit(u);
        units.Remove(u);

        u.Remove();

        SetTurncounter();

        CheckConditions();
    }

    private void BattleWon()
    {
        SceneManager.LoadScene(2);
    }

    /* Conditions */
    private void CheckConditions()
    {
        if (units.Count == 1)
            victory = true;
    }

    /* Internal ID commands */
    private BattleUnit FindUnitFromID(int i)
    {
        foreach (BattleUnit u in units)
            if (u.IsID(i))
                return u;
        return null;
    }

    /* Retrive ID */
    public int[] GetIDs()
    {
        List<int> temp = new List<int>();

        foreach (BattleUnit u in units)
            temp.Add(u.GetID());

        return temp.ToArray();
    }

    public int GetIDFromActive()
    {
        return current_unit.GetID();
    }

    public int GetIDFromPosition(Coordinate c)
    {
        foreach (BattleUnit u in units)
            if (u.GetPosition().Equals(c))
                return u.GetID();

        return -1;
    }

    /* Actor ID */
    public bool OwnsID(BattleActor a, int i)
    {
        if (a == null)
            return false;
        return FindUnitFromID(i).IsActor(a);
    }

    /* Retrive Data From ID */
    public Coordinate GetPositionFromID(int i)
    {
        return new Coordinate(FindUnitFromID(i).GetPosition());
    }

    public int GetMoveFromID(int id)
    {
        return FindUnitFromID(id).GetMov();
    }

    public List<Coordinate> GetAttackAreaFromID(int id, int i = -1)
    {
        return FindUnitFromID(id).GetAttack().GetAttackArea();
    }

    public List<Coordinate> GetDamageAreaFromID(int id, int i = -1)
    {
        return FindUnitFromID(id).GetAttack().GetDamageArea();
    }

    /*External Data*/

    public BattlePathfinder.Path GetPath(Coordinate c1, Coordinate c2)
    {
        return pathfinder.GetPath(c1, c2, current_unit);
    }

    public Coordinate GetMouseCordinate()
    {
        Vector3 vec = Input.mousePosition;
        vec.z = 10;
        Vector2 temp = cam.GetComponent<Camera>().ScreenToWorldPoint(vec);
        return new Coordinate(Math.drop(temp.x + 0.5f) - map.x_offset, Math.drop(temp.y + 0.5f) - map.x_offset);
    }

    public bool WithinMap(Coordinate c)
    {
        return map.Contains(c);
    }

    public bool HasUnit(Coordinate c)
    {
        return map.GetUnit(c) != null;
    }

    public int GetSteps()
    {
        return step_value;
    }

    public int GetMaxSteps()
    {
        return step_value + (action_value * current_unit.GetMov());
    }

    public int GetActions()
    {
        return action_value;
    }

    public Coordinate WorldToCord(Vector2 v)
    {
        return map.WorldToCord(v);
    }

    public Vector2 CordToWorld(Coordinate c)
    {
        return map.CordToWorld(c);
    }

    /* UI */
    public void SetTurncounter()
    {
        BattleUnit[] temp = turn.PeakFutureTurns();

        for (int i = 0; i < 11; ++i)
        {
            Destroy(mob_turn_icons[i]);
            Destroy(actor_turn_icons[i]);
        }

        for (int i = 0; i < 11; ++i)
        {
            if (i == 0)
            {
                mob_turn_icons[i] = Instantiate(current_unit.GetTurnIcon());
                actor_turn_icons[i] = Instantiate(current_unit.GetActorIcon());
            }
            else
            {
                mob_turn_icons[i] = Instantiate(temp[i - 1].GetTurnIcon());
                actor_turn_icons[i] = Instantiate(temp[i - 1].GetActorIcon());
            }
            mob_turn_icons[i].transform.parent = cam.gameObject.transform;
            actor_turn_icons[i].transform.parent = cam.gameObject.transform;

            mob_turn_icons[i].transform.localPosition = new Vector3(-6.625f, 5f - i + (i == 0 ? 1 : 0), 10);
            actor_turn_icons[i].transform.localPosition = new Vector3(-7.625f, 5f - i + (i == 0 ? 1 : 0), 10);
        }
    }

    public void SetMenu(bool b)
    {
        menu.SetActive(b);
    }

    public void SetStepCounter(int i = -1)
    {
        if (i == -1)
        {
            SetStepCounter(step_value);
            return;
        }

        foreach (GameObject g in token_holder.step_token)
            g.SetActive(false);

        for (int I = 0; I < i; ++I)
            token_holder.step_token[I].SetActive(true);
    }

    public void SetActionCounter(int i = -1)
    {
        if (i == -1)
        {
            SetActionCounter(action_value);
            return;
        }

        foreach (GameObject g in token_holder.action_token)
            g.SetActive(false);

        for (int I = 0; I < i; ++I)
            token_holder.action_token[I].SetActive(true);
    }

    public void SetCam(Coordinate c)
    {
        Vector3 vec = CordToWorld(c);

        vec.z = -10;

        cam.transform.position = vec;
    }

    /* Battle Menu */
    public void OpenBattleMenu(string[] captions, int start)
    {
        if (captions.Length != 8)
            return;

        battle_menu.body.SetActive(true);
        battle_menu.highlight.SetActive(true);

        for (int i = 0; i < 8; ++i)
            battle_menu.text[i].text = captions[i];

        SetBattleMenu(start);
    }

    public void SetBattleMenu(int value)
    {
        if (value < 0 || value > 7)
            return;

        battle_menu.highlight.transform.localPosition = new Vector2(-1.5f + (value % 4), 0.25f - ((value / 4) * 0.5F));
    }

    public void CloseBattleMenu()
    {
        battle_menu.body.SetActive(false);
        battle_menu.highlight.SetActive(false);

        for (int i = 0; i < 8; ++i)
            battle_menu.text[i].text = "";

        SetBattleMenu(0);
    }

    /* Map UI */
    public void GenerateTrail(BattlePathfinder.Path p, int I)
    {
        if (p == null)
            return;

        Coordinate end = p.GetNodeAtValue(I);
        Coordinate[] c = p.GetCoordinates();

        for (int i = 0; c[i] != end; ++i)
        {
            char path_type = c[i].GetX() != c[i + 1].GetX() ? 'H' : 'V';
            float pos_x = (c[i].GetX() + c[i + 1].GetX()) / 2f + map.x_offset;
            float pos_y = (c[i].GetY() + c[i + 1].GetY()) / 2f + map.y_offset;

            trail.Add(Instantiate(Resources.Load<GameObject>("BattleMapSprites/Path/" + path_type), new Vector2(pos_x, pos_y), new Quaternion()));
        }
    }

    public void ClearTrail()
    {
        foreach (GameObject g in trail)
            Destroy(g);
        trail.Clear();
    }

    public void AddToOverlayMap(Coordinate c, int i)
    {
        if (c == null)
            return;

        map.SetOverlay(c, i);
    }

    public void RemoveOverlayFromMap(Coordinate c)
    {
        if (c == null)
            return;

        map.RemoveOverlay(c);
    }

    public void ClearOverlayMap()
    {
        map.ClearOverlay();
    }

    public void UpdateMinorStatsUI()
    {
        BattleUnit temp_unit = FindUnitFromID(GetIDFromActive());

        minor_stats_menu.text[0].text = "" + temp_unit.GetName();
        minor_stats_menu.text[1].text = "HP: " + temp_unit.GetHP() + "/" + temp_unit.GetMaxHP();

        if (temp_unit.IsHuman())
        {
            minor_stats_menu.text[2].text = "AP: " + temp_unit.GetAP() + "/" + temp_unit.GetMaxAP();
            minor_stats_menu.text[3].text = "AAP";
        }
        else
        {
            minor_stats_menu.text[2].text = "";
            minor_stats_menu.text[3].text = "";
        }
    }

    public void ClearMinorStatsUI()
    {
        minor_stats_menu.text[0].text = "";
        minor_stats_menu.text[1].text = "";
        minor_stats_menu.text[2].text = "";
        minor_stats_menu.text[3].text = "";
    }

    public void LoadFullStatsUI(Coordinate position)
    {

    }

    public void CloseFullStatsUI()
    {

    }
}