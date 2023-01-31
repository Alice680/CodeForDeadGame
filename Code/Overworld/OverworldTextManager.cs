using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldTextManager : MonoBehaviour
{
    private OverworldText text_refrence;

    private KeyboardInputter inputter;

    private bool release_key;

    private int current_pointer = 0;

    private float last_input;

    private bool option;

    [SerializeField] private GameObject obj;
    [SerializeField] private Text text_name;
    [SerializeField] private Text text_body;


    public void Open(OverworldText text)
    {
        text_refrence = text;

        inputter = new KeyboardInputter();
        release_key = false;

        obj.SetActive(true);

        current_pointer = 0;

        last_input = Time.time;

        option = true;

        SetText();
    }

    public void Run(OverworldManager manager)
    {
        inputter.Run();

        if (!inputter.GetEnter())
            release_key = true;

        if (inputter.GetDir() % 2 == 1 && Time.time - last_input > 0.15F)
        {
            last_input = Time.time;

            option = !option;

            SetQuestion();
        }

        if (release_key && inputter.GetEnter())
        {
            release_key = false;

            if (text_refrence.GetInstanceType(current_pointer) == OverworldText.InstanceType.End)
            {
                CloseMap(manager);
            }
            else
            {
                current_pointer = text_refrence.GetNext(current_pointer, option);

                OverworldText.InstanceType instance = text_refrence.GetInstanceType(current_pointer);

                if (instance == OverworldText.InstanceType.Jump || instance == OverworldText.InstanceType.End)
                    SetText();
                else if (instance == OverworldText.InstanceType.Split)
                    SetQuestion();

                option = true;
            }
        }
    }

    private void SetText()
    {
        text_name.text = "" + text_refrence.GetName(current_pointer);
        text_body.text = "" + text_refrence.GetBody(current_pointer);
    }

    private void SetQuestion()
    {
        text_name.text = "" + text_refrence.GetName(current_pointer);
        if (option)
            text_body.text = "" + text_refrence.GetBody(current_pointer) + "\n   Yes   <-" + "\n   No";
        else
            text_body.text = "" + text_refrence.GetBody(current_pointer) + "\n   Yes" + "\n   No     <-";
    }

    private void CloseMap(OverworldManager manager)
    {
        text_refrence.UpdateData(current_pointer);

        text_name.text = "";
        text_body.text = "";

        obj.SetActive(false);

        if(text_refrence.GetBattle(current_pointer) != -1)
            manager.LoadBattle(text_refrence.GetBattle(current_pointer));
        else
            manager.CloseText();
    }
}