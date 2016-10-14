using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using AzureServicesForUnity;
using AzureServicesForUnity.Helpers;
using System.Collections.Generic;

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
        var name = string.Format("\"{0}\"", loginName.text);
        var password = string.Format("\"{0}\"", loginPassword.text);
        var teacherId = string.Format("\"{0}\"", GameControl.CurrentTeacherId);
        var jsonData = "{\"loginName\":" + name + " , \"loginPassword\":" + password + " , \"teacherID\":" + teacherId + "}";

        EasyAPIs.Instance.CallAPI<CustomAPIReturnObject>("CheckStudentLogin", HttpMethod.Post, jsonData, response =>
        {
            if (response.Status == CallBackResult.Success)
            {
                loginMessage.text = EasyAPIs.Instance.apiMessage;

                string[] message = EasyAPIs.Instance.apiMessage.Split(',');
                List<string> messageList = new List<string>(message.Length);
                messageList.AddRange(message);

                int idNumber;
                var result = int.TryParse(messageList[0], out idNumber);
                if (result)
                {
                    loginMessage.text = "Success!";
                    var studentId = int.Parse(messageList[0]);
                    GameControl.CurrentStudentId = studentId;
                    GameControl.StudentLevelProgress = int.Parse(messageList[1]);
                    GameControl.Game1Progress = int.Parse(messageList[2]);
                    GameControl.Game2Progress = int.Parse(messageList[3]);
                    GameControl.Game3Progress = int.Parse(messageList[4]);
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
        var name = string.Format("\"{0}\"", teacherEmail.text).ToLower();
        var jsonData = "{\"teacherEmail\":" + name + "}";

        EasyAPIs.Instance.CallAPI<CustomAPIReturnObject>("CheckTeacherEmail", HttpMethod.Post, jsonData, response =>
        {
            if (response.Status == CallBackResult.Success)
            {
                loginMessage.text = EasyAPIs.Instance.apiMessage;

                int idNumber;
                var result = int.TryParse(EasyAPIs.Instance.apiMessage, out idNumber);
                if (result)
                {
                    var teacherId = int.Parse(EasyAPIs.Instance.apiMessage);
                    classroomNumber.text = teacherId.ToString();
                    GameControl.CurrentTeacherId = teacherId;
                    PlayerPrefsManager.SetTeacherID(teacherId);
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