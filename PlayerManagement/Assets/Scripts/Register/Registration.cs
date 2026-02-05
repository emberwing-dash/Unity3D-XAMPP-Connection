using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Registration : MonoBehaviour
{

    [Header("UI")]
    public InputField nameField;
    public InputField passwordField;
    public Button submitButton;

    public int minLength = 8;

    private void Start()
    {
        submitButton.interactable = false;

        nameField.onValueChanged.AddListener(_ => VerifyInputs());
        passwordField.onValueChanged.AddListener(_ => VerifyInputs());
    }

    public void VerifyInputs()
    {
        submitButton.interactable =
            nameField.text.Length >= minLength/4 &&
            passwordField.text.Length >= minLength;
    }

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    private IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", nameField.text);
        form.AddField("password", passwordField.text);

        string url = $"http://{ServerConfig.GetIP()}/sqlconnect/register.php";




        Debug.Log("Sending request to: " + url);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        www.timeout = 10; // seconds

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Network error: " + www.error);
            yield break;
        }

        string response = www.downloadHandler.text.Trim();
        Debug.Log("Server response: " + response);

        if (response == "0")
        {
            Debug.Log("User created successfully");
            SceneManager.LoadScene("MainMenu"); // FIXED
        }

        else
        {
            Debug.LogWarning("Registration failed: " + response);
        }
    }
}
