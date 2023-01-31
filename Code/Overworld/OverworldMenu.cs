using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverworldMenu
{

    [Serializable]
    private struct MainMenuInfo
    {
        public GameObject body;
        public GameObject highlight;
        public Text[] text;
    }

    [Serializable]
    private struct StatsInfo
    {
        public GameObject body;

        public Text[] core_text;
        public Text[] stats_text;
        public Text[] Gear_text;
        public Text[] Monster_text;
        public Text[] Trait_text;
    }

    [Serializable]
    private struct CClassInfo
    {
        public GameObject body;
        public GameObject[] highlight;

        public Text[] main_text;
        public Text[] core_text;
        public Text[] trait_text;
    }

    [Serializable]
    private struct GearInfo
    {
        public GameObject body;

        public GameObject type_marker;
        public GameObject item_marker;

        public Text[] type_text;
        public Text[] item_text;
    }
    private enum State { setup, main, stats, monsters, c_class, t_class, gear, trait, item, food, end };

    public string[] main_text = new string[8] { "Stat", "C-C", "Ger", "Itm", "Mon", "T-C", "Trt", "Fod" };

    private State state;

    private float last_input;

    private int main_menu_location;

    private int sub_menu_level;
    private int[] sub_menu_location;

    public OverworldMenu()
    {
        main_menu_location = 0;
        state = State.setup;
    }

    public bool Run(OverworldManager manager, bool enter, bool close, int dir)
    {
        /*switch (state)
        {
            case State.setup:
                Setup(manager);
                return false;
            case State.end:
                End(manager);
                return true;

            case State.main:
                Main(manager, enter, close, dir);
                return false;

            case State.stats:
                Stats(manager, enter, close, dir);
                return false;
            case State.c_class:
                CClass(manager, enter, close, dir);
                return false;
            case State.gear:
                Gear(manager, enter, close, dir);
                return false;
            case State.item:
                Items(manager, enter, close, dir);
                return false;
            case State.monsters:
                Monsters(manager, enter, close, dir);
                return false;
            case State.t_class:
                TClass(manager, enter, close, dir);
                return false;
            case State.trait:
                Trait(manager, enter, close, dir);
                return false;
            case State.food:
                Food(manager, enter, close, dir);
                return false;
        }*/

        return false;
    }
    /* Entery/Exit Conditions */
    /*private void Setup(OverworldManager manager)
    {
        manager.OpenMenu(main_text, main_menu_location);

        state = State.main;
    }

    private void End(OverworldManager manager)
    {
        manager.CloseMenu();

        state = State.setup;
    }*/

    /* Core Manager */
    /*private void Main(OverworldManager manager, bool enter, bool close, int dir)
    {
        if (close)
        {
            state = State.end;
            return;
        }

        if (enter)
        {
            switch (main_menu_location)
            {
                case 0:
                    StartStats(manager);
                    break;
                case 1:
                    StartCClass(manager);
                    break;
                case 2:
                    StartGear(manager);
                    break;
                case 3:
                    StartItems(manager);
                    break;
                case 4:
                    StartMonsters(manager);
                    break;
                case 5:
                    StartTClass(manager);
                    break;
                case 6:
                    StartTrait(manager);
                    break;
                case 7:
                    StartFood(manager);
                    break;
            }

            return;
        }

        if (dir != 0 && Time.time - last_input > 0.25f)
        {
            main_menu_location = UpdateMenu(main_menu_location, dir);
            manager.SetMenu(main_menu_location);
            last_input = Time.time;
        }
    }*/

    /** States **/
    /* Stats */
    /*private void StartStats(OverworldManager manager)
    {
        manager.CloseMenu();

        manager.OpenStatsMenu();

        state = State.stats;
    }

    private void Stats(OverworldManager manager, bool enter, bool close, int dir)
    {
        if (close)
        {
            manager.CloseStatsMenu();

            manager.OpenMenu(main_text, main_menu_location);

            state = State.main;
            return;
        }

        if (enter)
        {
            return;
        }

        if (dir != 0 && Time.time - last_input > 0.25f)
        {
            last_input = Time.time;
            return;
        }
    }*/

    /* MClass */
    /*private void StartCClass(OverworldManager manager)
    {
        sub_menu_level = 0;
        sub_menu_location = new int[3];

        sub_menu_location[0] = manager.GetPlayerData().GetCurrentCClass();

        manager.CloseMenu();

        manager.OpenCClassMenu(0);

        state = State.c_class;
    }

    private void CClass(OverworldManager manager, bool enter, bool close, int dir)
    {
        if (close)
        {
            manager.CloseCClassMenu(sub_menu_level);
            sub_menu_location[sub_menu_level] = 0;
            if (sub_menu_level == 0)
            {
                manager.OpenMenu(main_text, main_menu_location);
                state = State.main;
            }
            else if (sub_menu_level == 1)
            {
                --sub_menu_level;
            }
            else if (sub_menu_level == 2)
            {
                --sub_menu_level;
                manager.GetPlayerData().SetCClassTrait(sub_menu_location[1], -1);
                manager.OpenCClassMenu(0);
            }
            return;
        }

        if (enter)
        {
            if (sub_menu_level == 0)
            {
                if (sub_menu_location[0] == -1)
                {
                    manager.GetPlayerData().SetCClass(-1);
                    manager.OpenCClassMenu(0);
                }
                else if (manager.GetPlayerData().GetCClassUnlocked(sub_menu_location[0]))
                {
                    manager.GetPlayerData().SetCClass(sub_menu_location[0]);
                    manager.OpenCClassMenu(0);
                    manager.OpenCClassMenu(1);
                    sub_menu_level = 1;
                }
            }
            else if (sub_menu_level == 1)
            {
                manager.OpenCClassMenu(2);
                sub_menu_level = 2;
                sub_menu_location[2] = 0;
            }
            else if (sub_menu_level == 2)
            {
                if (sub_menu_location[2] < 21 && manager.GetPlayerData().GetCClassRank(sub_menu_location[0]) >= sub_menu_location[2])
                    manager.GetPlayerData().SetCClassTrait(sub_menu_location[1], sub_menu_location[2]);
                else if (sub_menu_location[2] >= 21 && manager.GetPlayerData().GetCClassTraitUnlocked(sub_menu_location[1], sub_menu_location[2] - 21))
                    manager.GetPlayerData().SetCClassTrait(sub_menu_location[1], sub_menu_location[2]);
                else
                    return;

                manager.OpenCClassMenu(0);

                manager.CloseCClassMenu(2);

                sub_menu_level = 1;
            }
            return;
        }

        if (dir != 0 && Time.time - last_input > 0.25f)
        {
            if ((sub_menu_level == 0 || sub_menu_level == 1) && dir != 2 && dir != 4)
            {
                if (dir == 1)
                {
                    --sub_menu_location[sub_menu_level];

                    if (sub_menu_location[sub_menu_level] == (sub_menu_level == 0 ? -2 : -1))
                        sub_menu_location[sub_menu_level] = 2;
                }
                else if (dir == 3)
                {
                    ++sub_menu_location[sub_menu_level];

                    if (sub_menu_location[sub_menu_level] == 3)
                        sub_menu_location[sub_menu_level] = (sub_menu_level == 0 ? -1 : 0);
                }

                manager.SetCClassMenu(sub_menu_level, sub_menu_location[sub_menu_level]);
            }
            else if (sub_menu_level == 2)
            {
                if (dir % 2 == 1)
                    sub_menu_location[2] += (dir == 1 ? -1 : 1);
                else
                    sub_menu_location[2] += (dir == 2 ? 5 : -5);

                if (sub_menu_location[2] > 24)
                    sub_menu_location[2] -= 25;

                if (sub_menu_location[2] < 0)
                    sub_menu_location[2] += 25;

                manager.SetCClassMenu(2, sub_menu_location[2]);
            }

            last_input = Time.time;
        }
    }*/

    /* Gear */
    /*private void StartGear(OverworldManager manager)
    {
        sub_menu_level = 0;
        sub_menu_location = new int[3];

        manager.OpenGearMenu(0);

        state = State.gear;

        return;
    }

    private void Gear(OverworldManager manager, bool enter, bool close, int dir)
    {
        if (close)
        {
            if (sub_menu_level == 0)
            {
                manager.OpenMenu(main_text, main_menu_location);
                manager.CloseGearMenu(0);
                sub_menu_location[0] = 0;
                state = State.main;
            }
            else
            {
                sub_menu_level = 0;
                sub_menu_location[1] = 0;
                manager.CloseGearMenu(1);
            }
            return;
        }

        if (enter)
        {
            if (sub_menu_level == 0)
            {
                sub_menu_level = 1;
                manager.OpenGearMenu(1);
            }
            else
            {
                if(sub_menu_location[0] == 0)
                {
                    if (!manager.GetPlayerData().SummonerUnlocked(sub_menu_location[1]))
                        return;

                    manager.GetPlayerData().SetSummoner(sub_menu_location[1]);
                }
                else if(sub_menu_location[0] == 1)
                {
                    if (!manager.GetPlayerData().WeaponUnlocked(sub_menu_location[1]))
                        return;

                    manager.GetPlayerData().SetWeapon(sub_menu_location[1]);
                }
                else if (sub_menu_location[0] == 2)
                {
                    if (!manager.GetPlayerData().ArmorUnlocked(sub_menu_location[1]))
                        return;

                    manager.GetPlayerData().SetArmor(sub_menu_location[1]);
                }
                else if (sub_menu_location[0] == 3)
                {
                    if (!manager.GetPlayerData().AccessoryUnlocked(sub_menu_location[1]))
                        return;

                    manager.GetPlayerData().SetAccessoryA(sub_menu_location[1]);
                }
                else if (sub_menu_location[0] == 4)
                {
                    if (!manager.GetPlayerData().AccessoryUnlocked(sub_menu_location[1]))
                        return;

                    manager.GetPlayerData().SetAccessoryB(sub_menu_location[1]);
                }

                sub_menu_level = 0;
                sub_menu_location[1] = 0;
                manager.CloseGearMenu(1);
                manager.SetGearMenu(0, sub_menu_location[0]);
            }
            return;
        }

        if (dir != 0 && Time.time - last_input > 0.25f)
        {
            if (dir % 2 == 1)
            {
                if (dir == 3 && ++sub_menu_location[sub_menu_level] == 5)
                    sub_menu_location[sub_menu_level] = 0;
                else if (dir == 1 && --sub_menu_location[sub_menu_level] == -1)
                    sub_menu_location[sub_menu_level] = 4;

                manager.SetGearMenu(sub_menu_level, sub_menu_location[sub_menu_level]);
            }
            last_input = Time.time;
            return;
        }
    }*/

    /* Items */
    /*private void StartItems(OverworldManager manager)
    {
        return;
    }

    private void Items(OverworldManager manager, bool enter, bool close, int dir)
    {
        if (close)
        {
            manager.OpenMenu(main_text, main_menu_location);
            state = State.main;
            return;
        }

        if (enter)
        {
            return;
        }

        if (dir != 0 && Time.time - last_input > 0.25f)
        {

            last_input = Time.time;
        }
    }*/

    /* Monsters */
   /* private void StartMonsters(OverworldManager manager)
    {
        return;
    }

    private void Monsters(OverworldManager manager, bool enter, bool close, int dir)
    {
        if (close)
        {
            manager.OpenMenu(main_text, main_menu_location);
            state = State.main;
            return;
        }

        if (enter)
        {
            return;
        }

        if (dir != 0 && Time.time - last_input > 0.25f)
        {

            last_input = Time.time;
        }
    }*/

    /* TClass */
    /*private void StartTClass(OverworldManager manager)
    {

    }

    private void TClass(OverworldManager manager, bool enter, bool close, int dir)
    {
        if (close)
        {
            manager.OpenMenu(main_text, main_menu_location);
            state = State.main;
            return;
        }

        if (enter)
        {
            return;
        }

        if (dir != 0 && Time.time - last_input > 0.25f)
        {

            last_input = Time.time;
        }
    }*/

    /* Trait */
    /*private void StartTrait(OverworldManager manager)
    {
        return;
    }

    private void Trait(OverworldManager manager, bool enter, bool close, int dir)
    {
        if (close)
        {
            manager.OpenMenu(main_text, main_menu_location);
            state = State.main;
            return;
        }

        if (enter)
        {
            return;
        }

        if (dir != 0 && Time.time - last_input > 0.25f)
        {

            last_input = Time.time;
        }
    }*/

    /* Food */
    /*private void StartFood(OverworldManager manager)
    {
        return;
    }

    private void Food(OverworldManager manager, bool enter, bool close, int dir)
    {
        if (close)
        {
            manager.OpenMenu(main_text, main_menu_location);
            state = State.main;
            return;
        }

        if (enter)
        {
            return;
        }

        if (dir != 0 && Time.time - last_input > 0.25f)
        {

            last_input = Time.time;
        }
    }*/


    /* Math */
    /*private int UpdateMenu(int value, int input)
    {
        if (input == 1 || input == 3)
        {
            if (value < 4)
                value += 4;
            else
                value -= 4;
        }
        else if (input == 2)
        {
            value += 1;

            if (value == 8)
                value = 0;
        }
        else
        {
            value -= 1;

            if (value == -1)
                value = 7;
        }

        return value;
    }*/
}