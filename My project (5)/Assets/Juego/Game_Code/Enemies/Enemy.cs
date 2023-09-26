using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        protected float speed;
        protected float door_Damage;
        protected float door_Timer;
        protected int Life;
        [SerializeField] private EnemyConfig config;
        protected int way_Point_Index;
        protected Transform target;
        private int path_Index;
        private Action<Enemy,int> _OnDeath,_OnReach;
        private int points;
        private int damage_To_Player;

        protected enum BehaviourParams
        {
            Recognize_The_Area, Moving_Towars_Target, Breaking_In
        }

        protected BehaviourParams currentBehaviour;
        protected virtual void Behaviour()
        {
            switch(currentBehaviour)
            {
                case BehaviourParams.Recognize_The_Area:

                    if(way_Point_Index == WayPointManager.Paths[path_Index].Count -1)
                    {
                      /*transform.position = Vector2.zero; 
                      way_Point_Index = 0;
                      target = WayPointManager.Paths[path_Index][way_Point_Index];
                      currentBehaviour = BehaviourParams.Moving_Towars_Target;*/
                      Reach();
                      break;
                    }

                    way_Point_Index ++;
                    target = WayPointManager.Paths[path_Index][way_Point_Index];
                    currentBehaviour = BehaviourParams.Moving_Towars_Target;

                    break;
                
                case BehaviourParams.Moving_Towars_Target:

                    Vector2 dir = target.position - transform.position;
                    transform.Translate(dir.normalized * speed *Time.deltaTime,Space.World);

                    if(Vector2.Distance(transform.position,target.position) <= 0.1f)
                    {
                        currentBehaviour = BehaviourParams.Recognize_The_Area;
                    }

                    break;
                case BehaviourParams.Breaking_In:

                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(Life > 0)
            {
                Behaviour();
            }
            else if(Life <= 0)
            {
                Death();
            }
        }

        private void Death(){_OnDeath(this,points);}
        private void Reach(){_OnReach(this,damage_To_Player);}
        public void Config(Action<Enemy, int> _OnDeath, Action<Enemy, int> _OnReach)
        {
            this.speed = config.speed;
            this.door_Damage = config.door_Damage;
            this.Life = config.Life;
            this.damage_To_Player = config.damage_To_Player;
            way_Point_Index = 0;
            path_Index = config.path_Index;
            transform.position = WayPointManager.Paths[path_Index][way_Point_Index].position;
            way_Point_Index++; 
            target = WayPointManager.Paths[path_Index][way_Point_Index];
            this._OnDeath = _OnDeath;
            this._OnReach = _OnReach;
            this.points = config.points;
            currentBehaviour = BehaviourParams.Moving_Towars_Target;
        }

        public void OnTouched()
        {
            Life --;
            Debug.Log("Me dieron");
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            Life -= 1;
            if(Life <= 0)
            {
                Death();
            }
        }
    }
}

