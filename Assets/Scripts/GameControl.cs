using UnityEngine;
using AzureServicesForUnity;
using AzureServicesForUnity.Helpers;
using UnityEngine.SceneManagement;

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
}