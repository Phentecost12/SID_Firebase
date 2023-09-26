using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Wave_Manager : MonoBehaviour
{
    public static Wave_Manager Instance {get;private set;} = null;

    public TextMeshProUGUI txt;

    private float PLayTime = 10;

    private float timer;

    private bool Playing = false;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if(!Playing) return;

        if(timer <= 0)
        {
            Playing = false;
            CancelInvoke();
            Enemy_Spawn_System.Instance.RemoveAllEnemy();
            txt.text = "Time: 0";
        }
        else
        {
            timer -= Time.deltaTime;
            txt.text = "Time: " + MathF.Round(timer).ToString();
        }
    }

    public void startGame()
    {
        timer = PLayTime;
        Playing = true;
        InvokeRepeating("StartWave",0.2f,1);
    }

    void StartWave()
    {
        Enemy_Spawn_System.Instance.SpawnWave();
    }


}
