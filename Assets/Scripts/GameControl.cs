using UnityEngine;
using AzureServicesForUnity;
using AzureServicesForUnity.Helpers;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameControl : MonoBehaviour
{
    private static GameControl _thisObject;

    public static int CurrentTeacherId { get; set; }
    public static int CurrentStudentId { get; set; }
    public static int StudentLevelProgress { get; set; }
    public static int Game1Progress { get; set; }
    public static int Game2Progress { get; set; }
    public static int Game3Progress { get; set; }
    public static string TeacherLoginName { get; set; }
    public static string TeacherPassword { get; set; }
    public static string StudentLoginName { get; set; }
    public static string StudentPassword { get; set; }

    void Awake()
    {
        if (_thisObject != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        _thisObject = this;
        DontDestroyOnLoad(transform.root.gameObject);
    }

    void Start ()
    {
        Globals.DebugFlag = true;

        if (Globals.DebugFlag)
        {
            Debug.Log("instantiated Azure Services for Unity version " + Constants.LibraryVersion);
        }
        EasyAPIs.Instance.AuthenticationToken = "";
        //EasyTables.Instance.AuthenticationToken = "";

        if (PlayerPrefsManager.GetTeacherID() > 0)
        {
            CurrentTeacherId = PlayerPrefsManager.GetTeacherID();
        }
        else
        {
            CurrentTeacherId = 0;
        }
    }

    public void LoadLevel (string level)
    {
        SceneManager.LoadScene(level);
    }

    //TODO Finish Logout Method.
    public void LogOut()
    {
        GameEngine.LogOut();
    }

    public static void UploadStudentInformation (double timePlayed, int levelProgress = 0)
    {
        var name = string.Format("\"{0}\"", StudentLoginName);
        var password = string.Format("\"{0}\"", StudentPassword);
        var teacherId = string.Format("\"{0}\"", CurrentTeacherId);
        var jsonData = "{\"loginName\":" + name + " , \"loginPassword\":" + password + " , \"teacherID\":" + teacherId + " , ";
        var currentGame = GameEngine.CurrentGame;

        if (levelProgress == 0)
        {
            //only updating time played
            jsonData += "}"; 
        }
        else
        {
            //update it all
            jsonData += "}";
        }

        //Stage is cleared...record the timestamp.
        if (1==2)
        {
            jsonData += "}";
        }
        else
        {
            jsonData += "1/1/2000 12:00:00 AM";
        }

        EasyAPIs.Instance.CallAPI<CustomAPIReturnObject>("UdateInformation", HttpMethod.Post, jsonData, response =>
        {
            if (response.Status != CallBackResult.Success)
            {
                Debug.Log(response.Exception.Message);
            }
        });
    }
}