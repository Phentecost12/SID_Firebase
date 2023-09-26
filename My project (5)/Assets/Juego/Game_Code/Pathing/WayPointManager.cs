using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    private Path_Container[] path_Containers;
    public static List<Transform>[] Paths {get; private set;}

    void Awake()
    {

        path_Containers = GameObject.FindObjectsOfType<Path_Container>();
        Array.Sort(path_Containers, delegate(Path_Container a , Path_Container b){ return a.Index.CompareTo(b.Index);});

        Paths = new List<Transform>[path_Containers.Length];
        for (int i = 0; i < Paths.Length; i++)
        {
            Paths[i] = new List<Transform>();
            for (int j = 0; j < path_Containers[i].transform.childCount; j++)
            {
                Paths[i].Add(path_Containers[i].transform.GetChild(j));
                
            }
        }
    }
}
