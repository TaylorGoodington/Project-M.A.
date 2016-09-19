using UnityEngine;
using AzureServicesForUnity;
using AzureServicesForUnity.Helpers;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    static GameControl thisObject;

    public static int currentTeacherID;
    public static int currentStudentID;
    public static int studentLevelProgress;
    public static int game1Progress;
    public static int game2Progress;
    public static int game3Progress;

    public static string teacherLoginName;
    public static string teacherPassword;
    public static string studentLoginName;
    public static string studentPassword;

    void Awake()
    {
        if (thisObject != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        thisObject = this;
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
            currentTeacherID = PlayerPrefsManager.GetTeacherID();
        }
        else
        {
            currentTeacherID = 0;
        }
    }

    public void LoadLevel (string level)
    {
        SceneManager.LoadScene(level);
    }
}