using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    private enum State { Running, Text, Menu }

    private State state;

    private OverworldMap map;

    private OverworldPlayer player;

    private List<OverworldUnit> units;

    private float cool_down;

    [SerializeField] private GameObject cam;

    [SerializeField] private OverworldTextManager text_manager;

    private void Start()
    {
        state = State.Running;

        OverworldMapLayout layout = DataCore.next_overworld.GetLayout();

        map = layout.SetMap(cam);

        if (DataCore.next_overworld.IsFreshScene())
        {
            int spawn_location = DataCore.next_overworld.Location();
            player = new OverworldPlayer(this, layout.GetSpawnPosition(spawn_location), layout.GetSpawnRotation(spawn_location));
            map.SetCamPosition(layout.GetSpawnPosition(spawn_location));
        }
        else
        {
            player = new OverworldPlayer(this, DataCore.next_overworld.Position(), DataCore.next_overworld.Rotation());
            map.SetCamPosition(DataCore.next_overworld.Position());
        }

        map.SetPlayer(player);

        units = layout.SetUnits(this);

        foreach (OverworldUnit unit in units)
            map.SetUnit(unit);

        cool_down = Time.time;
    }

    private void Update()
    {
        if (state == State.Running)
        {
            player.Run();

            foreach (OverworldUnit unit in units)
                unit.RunPassive();
        }
        else if(state == State.Text)
        {
            text_manager.Run(this);
        }
        else if (state == State.Menu)
        {

        }
    }

    /* ID */
    private OverworldUnit FindUnitByID(int id)
    {

        foreach (OverworldUnit u in units)
            if (u.CheckID(id))
                return u;

        return null;
    }

    public int GetPlayerID()
    {
        return units[0].GetID();
    }

    public Coordinate GetCordByID(int i)
    {
        return FindUnitByID(i).GetPosition();
    }

    /* Player */
    public bool CanMovePlayer(int x, int y, Coordinate position, TileMove move_type)
    {
        if (!map.Traversable(new Coordinate(x + position.GetX(), y + position.GetY()), move_type))
            return false;
        else
            return true;
    }

    public void EnterPlayer(Coordinate cord)
    {
        map.SetCamPosition(cord);

        OverworldUnit temp_unit = map.SearchForMoveTrigger(cord);

        if (temp_unit != null)
            temp_unit.RunMoveTrigger();

        if (map.HasTransition(cord))
        {
            map.TriggerTransition(cord);
            SceneManager.LoadScene(1);
        }

        if(map.TryEncounter(cord))
        {
            map.TriggerTransition();
            DataCore.next_overworld = new NextOverworldData(DataCore.next_overworld.GetIndex(), player.GetPosition(), player.GetRotation());
            SceneManager.LoadScene(2);
        }
    }

    public void InteractPlayer(Coordinate cord)
    {
        OverworldUnit temp_unit = map.SearchForInteractTrigger(cord);

        if (temp_unit != null)
            temp_unit.RunInteractionTrigger();
    }

    /* Edit Game State */

    public void OpenText(int index)
    {
        if (Time.time - cool_down < 0.35f)
            return;

        text_manager.Open(((TextHolder)Resources.Load("GenericData/Texts")).GetText(index));
        state = State.Text;
    }

    public void CloseText()
    {
        cool_down = Time.time;

        state = State.Running;
    }

    /* Change Scene */
    public void LoadOverworld()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadBattle(int index)
    {
        DataCore.next_overworld = new NextOverworldData(DataCore.next_overworld.GetIndex(), player.GetPosition(), player.GetRotation());

        DataCore.next_battle = new NextBattleData(index);

        SceneManager.LoadScene(2);
    }

    /* Main Menu */
    /*public void OpenMenu(string[] captions, int start)
    {
        if (captions.Length != 8)
            return;

        menu_data.body.SetActive(true);
        menu_data.highlight.SetActive(true);

        for (int i = 0; i < 8; ++i)
            menu_data.text[i].text = captions[i];

        SetMenu(start);
    }

    public void SetMenu(int value)
    {
        if (value < 0 || value > 7)
            return;

        menu_data.highlight.transform.localPosition = new Vector2(-1.5f + (value % 4), 0.25f - ((value / 4) * 0.5F));
    }

    public void CloseMenu()
    {
        menu_data.body.SetActive(false);
        menu_data.highlight.SetActive(false);

        for (int i = 0; i < 8; ++i)
            menu_data.text[i].text = "";

        SetMenu(0);
    }*/

    /* Stats Menu */
    /* public void OpenStatsMenu()
     {
         stats_info.body.SetActive(true);

         Human player = player_battle_data.player;

         //Core
         stats_info.core_text[0].text = "Name\n" + player.GetHumanName();
         stats_info.core_text[1].text = "LV\n" + player.GetLevel();
         stats_info.core_text[2].text = "Core\n" + player.GetCClassName();
         stats_info.core_text[3].text = "Class\n" + player.GetCRank();
         stats_info.core_text[4].text = "Tamr\n" + player.GetTClassName();
         stats_info.core_text[5].text = "Class\n" + player.GetTRank();

         //stats
         stats_info.stats_text[0].text = "Stats\nNumeric";
         stats_info.stats_text[1].text = "HP\n" + player.GetHp();
         stats_info.stats_text[2].text = "ATK\n" + player.GetDmg();
         stats_info.stats_text[3].text = "DEF\n" + player.GetDef();
         stats_info.stats_text[4].text = "SPD\n" + player.GetSpd();
         stats_info.stats_text[5].text = "MOV\n" + player.GetMov();
         stats_info.stats_text[6].text = "ACT\n" + player.GetAct();
     }

     public void CloseStatsMenu()
     {
         stats_info.body.SetActive(false);

         foreach (Text t in stats_info.core_text)
             t.text = "";

         foreach (Text t in stats_info.stats_text)
             t.text = "";
     }*/

    /* Core Class Menu */
    /*public void OpenCClassMenu(int tier)
    {
        if (tier == 0)
        {
            c_class_info.body.SetActive(true);
            c_class_info.highlight[0].SetActive(true);

            for (int i = 0; i < 3; ++i)
            {
                if (player_data.GetCClassUnlocked(i))
                    c_class_info.main_text[i].text = player_data.GetCClass(i).GetClassName();
                else
                    c_class_info.main_text[i].text = "Locked";
            }
            c_class_info.main_text[3].text = "None";

            if (player_data.GetCurrentCClass() != -1)
                c_class_info.main_text[4].text = "Equiped\n" + player_data.GetCClass(player_data.GetCurrentCClass()).GetClassName();
            else
                c_class_info.main_text[4].text = "Equiped\nClassles";


            SetCClassMenu(0, player_data.GetCurrentCClass());
        }
        else if (tier == 1)
        {
            c_class_info.highlight[1].SetActive(true);
            SetCClassMenu(1, 0);
        }
        else if (tier == 2)
        {
            c_class_info.highlight[2].SetActive(true);
            SetCClassMenu(2, 0);
        }
    }

    public void SetCClassMenu(int tier, int i)
    {
        if (tier == 0)
        {
            foreach (Text t in c_class_info.core_text)
                t.text = "";

            foreach (Text t in c_class_info.trait_text)
                t.text = "";

            c_class_info.highlight[0].transform.localPosition = new Vector2(-5.85f, 1.5f - (i * 1.5f));

            if (i == -1)
                return;

            if (!player_data.GetCClassUnlocked(i))
                return;

            CoreClass temp_class = player_data.GetCClass(i);
            int temp_level = player_data.GetCClassRank(i);

            c_class_info.core_text[0].text = "Rank\n" + temp_level;
            c_class_info.core_text[1].text = "M Trait\n" + temp_class.GetMainTrait(temp_level).GetTraitName();
            for (int I = 0; I < 3; ++I)
            {
                int temp_id = player_data.GetCClassTraitSlot(i, I);

                if (temp_id == -1)
                    c_class_info.core_text[2 + I].text = "Trait " + I;
                else if (temp_id < 21)
                    c_class_info.core_text[2 + I].text = "Trait " + I + "\n" + temp_class.GetLevelTrait(temp_id).GetTraitName();
                else
                    c_class_info.core_text[2 + I].text = "Trait " + I + "\n" + temp_class.GetUnlockTrait(temp_id - 21).GetTraitName();
            }

            for (int I = 0; I <= temp_level; ++I)
                c_class_info.trait_text[I].text = "" + temp_class.GetLevelTrait(I).GetTraitName();

            for (int I = temp_level + 1; I <= 20; ++I)
                c_class_info.trait_text[I].text = "Lv: " + I;

            for (int I = 0; I < 4; ++I)
            {
                if (player_data.GetCClassTraitUnlocked(i, I))
                    c_class_info.trait_text[21 + I].text = temp_class.GetUnlockTrait(i).GetTraitName();
                else
                    c_class_info.trait_text[21 + I].text = "locked";
            }
        }
        else if (tier == 1)
        {
            c_class_info.highlight[1].transform.localPosition = new Vector2(-4.1f, i * -1.5f);
        }
        else if (tier == 2)
        {
            c_class_info.highlight[2].transform.localPosition = new Vector2(-2.5f + (2f * (i / 5)), 3f - (1.5f * (i % 5)));
        }
    }

    public void CloseCClassMenu(int tier)
    {
        if (tier == 0)
        {
            c_class_info.body.SetActive(false);
            c_class_info.highlight[0].SetActive(false);

            SetCClassMenu(0, 0);

            foreach (Text t in c_class_info.main_text)
                t.text = "";

            foreach (Text t in c_class_info.core_text)
                t.text = "";

            foreach (Text t in c_class_info.trait_text)
                t.text = "";
        }
        else if (tier == 1)
        {
            c_class_info.highlight[1].SetActive(false);
        }
        else if (tier == 2)
        {
            c_class_info.highlight[2].SetActive(false);
        }
    }*/

    /* Gear Menu */
    /*public void OpenGearMenu(int tier)
    {
        if (tier == 0)
        {
            gear_info.body.SetActive(true);
            gear_info.type_marker.SetActive(true);

            gear_info.type_text[0].text = "Sumon\n" + player_data.GetCurrentSummoner().GetName();
            gear_info.type_text[1].text = "Weapon\n" + player_data.GetCurrentWeapon().GetName();
            gear_info.type_text[2].text = "Armor\n" + player_data.GetCurrentArmor().GetName();
            gear_info.type_text[3].text = "AsoryA\n" + player_data.GetCurrentAccessoryA().GetName();
            gear_info.type_text[4].text = "AsoryB\n" + player_data.GetCurrentAccessoryB().GetName();

            SetGearMenu(0, 0);
        }
        else
        {
            gear_info.item_marker.SetActive(true);
            SetGearMenu(1,0);
        }
    }

    public void SetGearMenu(int tier, int i)
    {
        if (tier == 0)
        {
            gear_info.type_marker.transform.localPosition = new Vector2(-1.5f, 3f - (i * 1.5f));

            for (int I = 0; I < 5; ++I)
            {
                if (i == 0)
                {
                    if (player_data.SummonerUnlocked(I))
                        gear_info.item_text[I].text = "" + player_data.GetSummoner(I).GetName();
                    else
                        gear_info.item_text[I].text = "Locked";
                }
                else if (player_data.GetCurrentCClass() == -1 && I > 0)
                {
                    if (player_data.SummonerUnlocked(I))
                        gear_info.item_text[I].text = "" + player_data.GetSummoner(I).GetName();
                    else
                        gear_info.item_text[I].text = "Locked";
                }
                else if (i == 1)
                {
                    if (player_data.WeaponUnlocked(I))
                        gear_info.item_text[I].text = "" + player_data.GetWeapon(I).GetName();
                    else
                        gear_info.item_text[I].text = "Locked";
                }
                else if (i == 2)
                {
                    if (player_data.ArmorUnlocked(I))
                        gear_info.item_text[I].text = "" + player_data.GetArmor(I).GetName();
                    else
                        gear_info.item_text[I].text = "Locked";
                }
                else if (i == 3 || i == 4)
                {
                    if (player_data.AccessoryUnlocked(I))
                        gear_info.item_text[I].text = "" + player_data.GetAccessory(I).GetName();
                    else
                        gear_info.item_text[I].text = "Locked";
                }
            }
        }
        else
        {
            gear_info.item_marker.transform.localPosition = new Vector2(0.25f, 3f - (i * 1.5f));
        }
    }

    public void CloseGearMenu(int tier)
    {
        if (tier == 0)
        {
            gear_info.body.SetActive(false);
            gear_info.type_marker.SetActive(false);

            SetGearMenu(0, 0);

            for (int i = 0; i < 5; ++i)
            {
                gear_info.type_text[i].text = "";
                gear_info.item_text[i].text = "";
            }
        }
        else
        {
            gear_info.item_marker.SetActive(false);
            OpenGearMenu(0);
        }
    }*/


    /* Get Data */
    public PlayerData GetPlayerData()
    {
        return null;
    }

    /* Calculate Coordinate */
    public Coordinate GetMouseCordinate()
    {
        Vector3 vec = Input.mousePosition;
        vec.z = 10;
        Vector2 temp = GetComponent<Camera>().ScreenToWorldPoint(vec);
        return map.WorldToCord(new Vector2(Math.drop(temp.x + 0.5f), Math.drop(temp.y + 0.5f)));
    }

    public Coordinate WorldToCord(Vector2 vec)
    {
        return map.WorldToCord(vec);
    }

    public Vector2 CordToWorld(Coordinate cord)
    {
        return map.CordToWorld(cord);
    }
}