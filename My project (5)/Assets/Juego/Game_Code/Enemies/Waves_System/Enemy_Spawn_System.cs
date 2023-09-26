using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class Enemy_Spawn_System : MonoBehaviour
{
    [Header("Bachitombo")]
    [Space(5)]
    [SerializeField] private Bachitombo _bachitombo_Prefab;
    [SerializeField] private int _bachitombo_Pool_Default_Capacity;
    [SerializeField] private int _bachitombo_Pool_Max_Capacity;
    [SerializeField] private bool _bachitombo_Pool_Collection_Check;
    private ObjectPool<Bachitombo> _bachitombo_Pool;

    [Header("Tombo")]
    [Space(5)]
    [SerializeField] private Tombo _tombo_Prefab;
    [SerializeField] private int _tombo_Pool_Default_Capacity;
    [SerializeField] private int _tombo_Pool_Max_Capacity;
    [SerializeField] private bool _tombo_Pool_Collection_Check;
    private ObjectPool<Tombo> _tombo_Pool;

    [Header("Tombo Tactico")]
    [Space(5)]
    [SerializeField] private Tombo_Tactico _tombo_Tactico_Prefab;
    [SerializeField] private int _tombo_Tactico_Pool_Default_Capacity;
    [SerializeField] private int _tombo_Tactico_Pool_Max_Capacity;
    [SerializeField] private bool _tombo_Tactico_Pool_Collection_Check;
    private ObjectPool<Tombo_Tactico> _tombo_Tactico_Pool;

    [Header("Tombo Con Perro")]
    [Space(5)]
    [SerializeField] private Tombo_Con_Perro _tombo_Con_Perro_Prefab;
    [SerializeField] private int _tombo_Con_Pero_Pool_Default_Capacity;
    [SerializeField] private int _tombo_Con_Pero_Pool_Max_Capacity;
    [SerializeField] private bool _tombo_Con_Perro_Pool_Collection_Check;
    private ObjectPool<Tombo_Con_Perro> _tombo_Con_Pero_Pool;
    
    [Header("Esmad")]
    [Space(5)]
    [SerializeField] private Esmad _esmad_Prefab;
    [SerializeField] private int _esmad_Pool_Default_Capacity;
    [SerializeField] private int _esmad_Pool_Max_Capacity;
    [SerializeField] private bool _esmad_Pool_Collection_Check;
    private ObjectPool<Esmad> _esmad_Pool;

    private List<Enemy> spawned_Enemies;
    public static Enemy_Spawn_System Instance {get; private set;} = null;

    public TextMeshProUGUI text;
    private int puntaje = 0;
    //public HTTP_Users_Manager HTTP;
    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        _bachitombo_Pool = new ObjectPool<Bachitombo>(()=> {
            return Instantiate(_bachitombo_Prefab);
        }, bachitombo => {
            bachitombo.gameObject.SetActive(true);
        }, bachitombo =>{
            bachitombo.gameObject.SetActive(false);
        }, bachitombo => {
            Destroy(bachitombo.gameObject);
        },_bachitombo_Pool_Collection_Check, _bachitombo_Pool_Default_Capacity,_bachitombo_Pool_Max_Capacity);

        _tombo_Pool = new ObjectPool<Tombo>(() => {
            return Instantiate(_tombo_Prefab);
        }, tombo => {
            tombo.gameObject.SetActive(true);
        }, tombo => {
            tombo.gameObject.SetActive(false);
        }, tombo => {
            Destroy(tombo.gameObject);
        }, _tombo_Pool_Collection_Check, _tombo_Pool_Default_Capacity,_tombo_Pool_Max_Capacity);

        _tombo_Tactico_Pool = new ObjectPool<Tombo_Tactico>(()=> {
            return Instantiate(_tombo_Tactico_Prefab);
        }, tombo_Tactico => {
            tombo_Tactico.gameObject.SetActive(true);
        }, tombo_Tactico => {
            tombo_Tactico.gameObject.SetActive(false);
        }, tombo_Tactico => {
            Destroy(tombo_Tactico.gameObject);
        }, _tombo_Tactico_Pool_Collection_Check,_tombo_Tactico_Pool_Default_Capacity,_tombo_Tactico_Pool_Max_Capacity);

        _tombo_Con_Pero_Pool = new ObjectPool<Tombo_Con_Perro>(()=> {
            return Instantiate(_tombo_Con_Perro_Prefab);
        }, tombo_Con_perro => {
            tombo_Con_perro.gameObject.SetActive(true);
        }, tombo_Con_perro =>{
            tombo_Con_perro.gameObject.SetActive(false);
        }, tombo_Con_Perro => {
            Destroy(tombo_Con_Perro.gameObject);
        }, _tombo_Con_Perro_Pool_Collection_Check, _tombo_Con_Pero_Pool_Default_Capacity, _tombo_Con_Pero_Pool_Max_Capacity);

        _esmad_Pool = new ObjectPool<Esmad>(() =>{
            return Instantiate(_esmad_Prefab);
        }, esmad => {
            esmad.gameObject.SetActive(true);
        }, esmad => {
            esmad.gameObject.SetActive(false);
        }, esmad=> {
            Destroy(esmad.gameObject);
        },_esmad_Pool_Collection_Check, _esmad_Pool_Default_Capacity, _esmad_Pool_Max_Capacity);

        spawned_Enemies = new List<Enemy>();

        text.text = "Points: " + puntaje;
    }


    public void SpawnWave ()
    {

        var enemy_Bachitombo = _bachitombo_Pool.Get();
        enemy_Bachitombo.Config(OnDeath,OnReach);
        spawned_Enemies.Add(enemy_Bachitombo);

        var enemy_Tombo = _tombo_Pool.Get();
        enemy_Tombo.Config(OnDeath,OnReach);
        spawned_Enemies.Add(enemy_Tombo);

        var enemy_Tactico = _tombo_Tactico_Pool.Get();
        enemy_Tactico.Config(OnDeath,OnReach);
        spawned_Enemies.Add(enemy_Tactico);

        var enemy_Perro = _tombo_Con_Pero_Pool.Get();
        enemy_Perro.Config(OnDeath,OnReach);
        spawned_Enemies.Add(enemy_Perro);

        var enemy = _esmad_Pool.Get();
        enemy.Config(OnDeath,OnReach);
        spawned_Enemies.Add(enemy);

    }

    private void OnDeath(Enemy enemy, int i)
    {
        puntaje += i;
        text.text = "Score: " + puntaje;

        RemoveEnemy(enemy);
    }

    private void OnReach(Enemy enemy, int i)
    {
        RemoveEnemy(enemy);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        if(enemy is Bachitombo){_bachitombo_Pool.Release((Bachitombo)enemy);}
        else if (enemy is Tombo){_tombo_Pool.Release((Tombo)enemy);}
        else if (enemy is Tombo_Tactico){_tombo_Tactico_Pool.Release((Tombo_Tactico)enemy);}
        else if (enemy is Tombo_Con_Perro){_tombo_Con_Pero_Pool.Release((Tombo_Con_Perro)enemy);}
        else if (enemy is Esmad){_esmad_Pool.Release((Esmad)enemy);}
    }

    public void RemoveAllEnemy()
    {
        foreach(Enemy enemy in spawned_Enemies)
        {
            if(enemy is Bachitombo){_bachitombo_Pool.Release((Bachitombo)enemy);}
            else if (enemy is Tombo){_tombo_Pool.Release((Tombo)enemy);}
            else if (enemy is Tombo_Tactico){_tombo_Tactico_Pool.Release((Tombo_Tactico)enemy);}
            else if (enemy is Tombo_Con_Perro){_tombo_Con_Pero_Pool.Release((Tombo_Con_Perro)enemy);}
            else if (enemy is Esmad){_esmad_Pool.Release((Esmad)enemy);}
        }

        FireBase_Interactions.Instance.Set_New_Score(puntaje);

        UI_Manager.Instance.Game_Panel_Activation();
        UI_Manager.Instance.End_Panel_Activation();
        UI_Manager.Instance.Final_Score(puntaje);
        //UI_Scene_Manager.Instance.Game_Panel_Activate();
        //UI_Scene_Manager.Instance.End_Panel_Activate(puntaje);
    }
}

[System.Serializable]
public class Wave
{
   public int _bachitombo_Count;
   public int _tombo_Count;
   public int _tombo_Tactico_Count;
   public int _tombo_Con_Perro_Count;
   public int _esmad_Count;
}
