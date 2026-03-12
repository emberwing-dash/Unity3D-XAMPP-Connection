using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    

    public InputField nameField;
    public InputField passwordField;

    public Button submitButton;

    public void CallLogin()
    {
        StartCoroutine(LoginUser());
    }

    private IEnumerator LoginUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", nameField.text);
        form.AddField("password", passwordField.text);

        string url = $"http://{ServerConfig.GetIP()}/sqlconnect/login.php";



        Debug.Log("Sending request to: " + url);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        www.timeout = 10; // seconds

        yield return www.SendWebRequest();

        if (www.downloadHandler.text[0] == '0') //when successfully logged in
        {
            DBManager.username = nameField.text; //username
            DBManager.score = int.Parse(www.downloadHandler.text.Split('\t')[1]); //password

            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("User login failed. Error #"+www.downloadHandler.text);
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length>=4 && passwordField.text.Length>=8);
    }
}
