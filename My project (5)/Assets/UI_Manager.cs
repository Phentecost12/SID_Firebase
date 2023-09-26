using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject Game_Panel,Main_Panel,End_Panel,Score_Panel;
    [SerializeField] private List<TextMeshProUGUI> Leader_Board;
    [SerializeField] private User_Info user_Info;
    [SerializeField] private TextMeshProUGUI Final_Score_TXT;
    public static UI_Manager Instance {get;private set;} = null;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Game_Panel_Activation()
    {
        if(Game_Panel.activeInHierarchy)
        {
            Game_Panel.SetActive(false);
        }
        else
        {
            Game_Panel.SetActive(true);
        }
    }

    public void Main_Panel_Activation()
    {
        if(Main_Panel.activeInHierarchy)
        {
            Main_Panel.SetActive(false);
        }
        else
        {
            Main_Panel.SetActive(true);
        }
    }

    public void End_Panel_Activation()
    {
        if(End_Panel.activeInHierarchy)
        {
            End_Panel.SetActive(false);
        }
        else
        {
            End_Panel.SetActive(true);
        }
    }
    
    public void Score_Panel_Activation()
    {
        if(Score_Panel.activeInHierarchy)
        {
            Score_Panel.SetActive(false);
        }
        else
        {
            Score_Panel.SetActive(true);
        }
    }

    public void Change_Leader_Board(List<User> users)
    {   
        int j = users.Count;
        for(int i = 0; i < Leader_Board.Count;i++)
        {
            if(i<j)
            {
                Leader_Board[i].gameObject.SetActive(true);
                Leader_Board[i].text = users[i].user_Username + " Puntaje: " + users[i].user_Score;
            }
            else
            {
                Leader_Board[i].gameObject.SetActive(false);
            }
        }
    }

    public void New_Record(int i)
    {
        user_Info.Record_TXT.text = ""+i;
    }

    public void Set_User_Info(User user)
    {
        user_Info.Username_TXT.text = user.user_Username;
        user_Info.ID_TXT.text = user.userID;
        user_Info.Record_TXT.text = user.user_Score.ToString();
    }

    public void Final_Score(int i)
    {
        Final_Score_TXT.text = "Puntaje: " + i;
    }
}

[System.Serializable]
public class User_Info
{
    public TextMeshProUGUI Username_TXT,ID_TXT,Record_TXT;
    public Sprite Profile_Pic;
}
