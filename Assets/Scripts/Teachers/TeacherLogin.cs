using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using AzureServicesForUnity;
using AzureServicesForUnity.Helpers;
using System;

public class TeacherLogin : MonoBehaviour
{
    public InputField loginName;
    public InputField loginPassword;
    public Button login;
    public Text loginMessage;

    public void CheckTeacherLogin ()
    {
        string name = string.Format("\"{0}\"", loginName.text).ToLower();
        string password = string.Format("\"{0}\"", loginPassword.text);
        string jsonData = "{\"loginName\":" + name + " , \"loginPassword\":" + password + "}";

        EasyAPIs.Instance.CallAPI<CustomAPIReturnObject>("CheckTeacherLogin", HttpMethod.Post, jsonData, response =>
        {
            if (response.Status == CallBackResult.Success)
            {
                loginMessage.text = EasyAPIs.Instance.apiMessage;

                int idNumber;
                bool result = int.TryParse(EasyAPIs.Instance.apiMessage, out idNumber);
                if (result)
                {
                    loginMessage.text = "Success!";
                    int teacherID = int.Parse(EasyAPIs.Instance.apiMessage);
                    GameControl.CurrentTeacherId = teacherID;
                    PlayerPrefsManager.SetTeacherID(teacherID);
                    GameControl.TeacherLoginName = loginName.text.ToLower();
                    GameControl.TeacherPassword = loginPassword.text;
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

    public IEnumerator Login ()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Teacher Dashboard");
    }
}