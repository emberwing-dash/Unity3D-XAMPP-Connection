using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text playerDisplay;

    [Header("IP Input")]
    public InputField ipField;

    [Header("Buttons")]
    public Button registerButton;
    public Button loginButton;
    public Button playButton;

    private void Start()
    {
        if (DBManager.LoggedIn)
            playerDisplay.text = "Player: " + DBManager.username;

        ipField.text = ServerConfig.GetIP();

        ipField.onValueChanged.AddListener(_ => VerifyInputs());

        VerifyInputs();
    }

    // --------------------------------------------------
    // VALIDATION
    // --------------------------------------------------

    public void VerifyInputs()
    {
        bool validIP = IPAddress.TryParse(ipField.text, out _);

        registerButton.interactable = validIP;
        loginButton.interactable = validIP;
        playButton.interactable = validIP;
    }

    // --------------------------------------------------
    // SCENE LOADERS
    // --------------------------------------------------

    public void GoToRegister()
    {
        ServerConfig.SetIP(ipField.text);
        SceneManager.LoadScene("RegisterUser");
    }

    public void GoToLogin()
    {
        ServerConfig.SetIP(ipField.text);
        SceneManager.LoadScene("LoginUser");
    }

    public void GoToGame()
    {
        ServerConfig.SetIP(ipField.text);
        SceneManager.LoadScene("Game");
    }
}
