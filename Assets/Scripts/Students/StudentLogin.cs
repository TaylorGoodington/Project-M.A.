using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using AzureServicesForUnity;
using AzureServicesForUnity.Helpers;
using System.Collections.Generic;
using System.Linq;

public class StudentLogin : MonoBehaviour
{
    public Text loginName;
    public InputField loginPassword;
    public Button login;
    public Text loginMessage;
    public Text classroomNumber;
    public GameObject enterTeacherEmailWindow;
    public InputField teacherEmail;

    void Start ()
    {
        if (GameControl.CurrentTeacherId == 0)
        {
            enterTeacherEmailWindow.SetActive(true);
        }
        else
        {
            classroomNumber.text = GameControl.CurrentTeacherId.ToString();
        }
    }

    public void CheckStudentLogin()
    {
        string name = string.Format("\"{0}\"", loginName.text);
        string password = string.Format("\"{0}\"", loginPassword.text);
        string teacherID = string.Format("\"{0}\"", GameControl.CurrentTeacherId);
        string jsonData = "{\"loginName\":" + name + " , \"loginPassword\":" + password + " , \"teacherID\":" + teacherID + "}";

        EasyAPIs.Instance.CallAPI<CustomAPIReturnObject>("CheckStudentLogin", HttpMethod.Post, jsonData, response =>
        {
            if (response.Status == CallBackResult.Success)
            {
                loginMessage.text = EasyAPIs.Instance.apiMessage;

                string[] message = EasyAPIs.Instance.apiMessage.Split(',');
                List<string> messageList = new List<string>(message.Length);
                messageList.AddRange(message);

                int idNumber;
                bool result = int.TryParse(messageList[0], out idNumber);
                if (result)
                {
                    loginMessage.text = "Success!";
                    int studentID = int.Parse(messageList[0]);
                    GameControl.CurrentStudentId = studentID;
                    GameControl.StudentLevelProgress = int.Parse(messageList[1]);
                    GameControl.Game1Progress = int.Parse(messageList[2]);
                    GameControl.Game2Progress = int.Parse(messageList[3]);
                    GameControl.Game3Progress = int.Parse(messageList[4]);
                    Debug.Log(studentID);
                    GameControl.StudentLoginName = loginName.text;
                    GameControl.StudentPassword = loginPassword.text;
                    StartCoroutine("Login");
                }
            }
            else
            {
                Debug.Log(response.Exception.Message);
            }
        });
        loginMessage.text = "Loading...";
    }

    public void EnterTeacherEmail()
    {
        string name = string.Format("\"{0}\"", teacherEmail.text).ToLower();
        string jsonData = "{\"teacherEmail\":" + name + "}";

        EasyAPIs.Instance.CallAPI<CustomAPIReturnObject>("CheckTeacherEmail", HttpMethod.Post, jsonData, response =>
        {
            if (response.Status == CallBackResult.Success)
            {
                loginMessage.text = EasyAPIs.Instance.apiMessage;

                int idNumber;
                bool result = int.TryParse(EasyAPIs.Instance.apiMessage, out idNumber);
                if (result)
                {
                    int teacherID = int.Parse(EasyAPIs.Instance.apiMessage);
                    classroomNumber.text = teacherID.ToString();
                    GameControl.CurrentTeacherId = teacherID;
                    PlayerPrefsManager.SetTeacherID(teacherID);
                    loginMessage.text = "";
                    enterTeacherEmailWindow.SetActive(false);
                }
            }
            else
            {
                Debug.Log(response.Exception.Message);
            }
        });
        loginMessage.text = "Loading...";
    }

    public IEnumerator Login()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level" + GameControl.StudentLevelProgress);
    }
}