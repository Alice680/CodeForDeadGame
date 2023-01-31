using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject highlighter;

    private int current_option;

    private float last_input;

    private KeyboardInputter inputter;

    //Dev
    public bool dev_save;

    private void Start()
    {
        current_option = 3;
        inputter = new KeyboardInputter();
        last_input = Time.time;
        DataCore.dev_save = dev_save;
    }

    private void Update()
    {
        inputter.Run();

        if(inputter.GetEnter())
        {
            if(current_option == 3)
            {
                DataCore.NewData();
                SceneManager.LoadScene(1);
            }
            else if (current_option == 2)
            {
                DataCore.LoadData();
                SceneManager.LoadScene(1);
            }
            else if (current_option == 1)
            {

            }
            else if (current_option == 0)
            {
                Application.Quit();
            }
        }

        if (Time.time - last_input > 0.1F)
        {
            int dir = inputter.GetDir();

            if (dir == 1)
                current_option = Math.LoopValue(current_option + 1, 3);
            else if (dir == 3)
                current_option = Math.LoopValue(current_option - 1, 3);
            else
                return;

            highlighter.transform.localPosition = new Vector2(0, -3.9f + (current_option * 2.6f));

            last_input = Time.time;
        }
    }
}