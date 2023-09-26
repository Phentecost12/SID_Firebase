using System.Collections;
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Firebase.Extensions;
using System.Collections.Generic;
using System.Linq;

public class FireBase_Interactions : MonoBehaviour
{
    [SerializeField] private TMP_InputField Email_TXT,Username_TXT,Password_TXT;
    private DatabaseReference database;
    public User currentUser;
    public List<User> All_Users = new List<User>();
    private List<string> All_Users_Usernames = new List<string>();

    public static FireBase_Interactions Instance {get;private set;} = null;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += Check_Login;
        database = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void Check_Login(object sender, EventArgs e)
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            if(scene != 2)
            {
                Load_Sceen(2);
            }
            else
            {
                currentUser.userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
                GetUserScore();
                GetUser_Username();
            }
            
        }
    }

    public void Sing_Up()
    {
        string user_Email = Email_TXT.text;
        string user_Password = Password_TXT.text;
        StartCoroutine(RegisterUser(user_Email,user_Password));
    }
    
    public void Log_In()
    {
        var auth = FirebaseAuth.DefaultInstance;
        auth.SignInWithEmailAndPasswordAsync(Email_TXT.text, Password_TXT.text).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("Sign In With Email And Password Async was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Sign In With Email And Password Async encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
        });
    }

    public void Log_Out()
    {
        FirebaseAuth.DefaultInstance.SignOut();
        Load_Sceen(0);
    }

    public void Load_Sceen(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void Reset_Password()
    {
        string email = Email_TXT.text;
        StartCoroutine(Restor_Password(email));
    }

    public void Set_New_Score(int score)
    {
        if(score > currentUser.user_Score)
        {
            database.Child("users").Child(currentUser.userID).Child("score").SetValueAsync(score);
            UI_Manager.Instance.New_Record(score);
        }
    }

    public List<User> Set_Leader_Board()
    {
        List<User> users = All_Users.OrderByDescending(x=>x.user_Score).ToList();
        return users;
    }

    public void GetUserScore()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users/" + currentUser.userID + "/score")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    string _score = "" + snapshot.Value;
                    currentUser.user_Score = int.Parse(_score);
                }
            });
    }

    public void GetUser_Username()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users/" + currentUser.userID + "/username")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    currentUser.user_Username = (string) task.Result.Value;
                    UI_Manager.Instance.Set_User_Info(currentUser);
                }
            });
    }

    

    private IEnumerator Restor_Password(string email)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var resetTask = auth.SendPasswordResetEmailAsync(email);

        yield return new WaitUntil(() => resetTask.IsCompleted);

        if (resetTask.IsCanceled)
        {
            Debug.LogError($"SendPasswordResetEmailAsync is canceled");
        }
        else if (resetTask.IsFaulted)
        {
            Debug.LogError($"SendPasswordResetEmailAsync encountered error" + resetTask.Exception);
        }
        else
        {
            Debug.Log("Password reset email sent successfully to: " + email);
        }
    }

    private IEnumerator RegisterUser(string email, string password)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.IsCanceled)
        {
            Debug.LogError($"Create User With Email And Passwor dAsync is canceled");
        }
        else if (registerTask.IsFaulted)
        {
            Debug.LogError($"Create User With Email And Password Async encountered error" + registerTask.Exception);
        }
        else
        {
            AuthResult result = registerTask.Result;
            string name = Username_TXT.text;
            database.Child("users").Child(result.User.UserId).Child("username").SetValueAsync(name);
            database.Child("users").Child(result.User.UserId).Child("score").SetValueAsync(0);
        }
    }
    public void GetAllUsers()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("users").OrderByChild("score").LimitToLast(5)
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
                    {
                        var userObject = (Dictionary<string, object>)userDoc.Value;
                        string username = (string)userObject["username"];
                        if(All_Users_Usernames.Contains(username))continue;
                        All_Users_Usernames.Add(username);
                        int score = Convert.ToInt32(userObject["score"]);
                        User _user = new User
                        {
                            user_Username = username,
                            user_Score = score
                        };
                        
                        All_Users.Add(_user);
                        
                        
                    }

                    All_Users = Set_Leader_Board();
                    UI_Manager.Instance.Change_Leader_Board(All_Users);
                }

            });
    }

    private void OnDestroy()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= Check_Login;
    }
}

[System.Serializable]
public class User
{
    public string userID;
    public string user_Username;
    public int user_Score;
}
