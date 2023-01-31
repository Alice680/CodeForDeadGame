using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SaveOnLoad : MonoBehaviour
{
    [SerializeField] private BattleStart battle_start;
    [SerializeField] private OverworldStart overworld_start;
    [SerializeField] private PlayerBattleData battle_data;
    [SerializeField] private PlayerOverworldData overworld_data;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(2);
    }
}