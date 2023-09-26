using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Enemies;
using UnityEngine;

public class Touch_Manager : MonoBehaviour
{
    public static Touch_Manager Instance {get;private set;} = null;

    private Vector2 _screen_Position;
    private Vector2 _world_Position;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if( Input.GetMouseButton(0))
        {
            Vector3 mouse_Pos = Input.mousePosition;
            _screen_Position = new Vector2(mouse_Pos.x,mouse_Pos.y);
            
        }
        else
        {
            return;
        }

        _world_Position = Camera.main.ScreenToWorldPoint(_screen_Position);
        

        RaycastHit2D hit = Physics2D.Raycast(_world_Position,Vector2.zero);

        if(hit.collider != null)
        {
            Enemy x = hit.collider.GetComponent<Enemy>();

            if(x != null)
            {
                x.OnTouched();
            }
        }
    }
}
