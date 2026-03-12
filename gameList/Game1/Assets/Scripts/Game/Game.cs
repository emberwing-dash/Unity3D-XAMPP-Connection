using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public TextMeshProUGUI playerDisplay;
    public TextMeshProUGUI scoreDisplay;

    [Header("Exit Menu")]
    public GameObject exitPanel;

    private void Start()
    {
        DBManager.score = 0;

        // Make cursor visible BEFORE scene change
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        DBManager.score = 0;

        if (DBManager.username==null)
        {
            SceneManager.LoadScene("MainMenu"); //load menu scene if not logged in
        }
        playerDisplay.text = "Player: " + DBManager.username;
        scoreDisplay.text = "Score: " + DBManager.score;
    }

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleExitMenu();
        }
    }

    void ToggleExitMenu()
    {
        isPaused = !isPaused;

        exitPanel.SetActive(isPaused);

        // Pause / resume game
        Time.timeScale = isPaused ? 0f : 1f;

        // Cursor control
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }


    public void CallSaveData()
    {
        // Make cursor visible BEFORE scene change
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(SavePlayerData());
    }


    IEnumerator SavePlayerData()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        form.AddField("score", DBManager.score);

        string url = $"http://{ServerConfig.GetIP()}/sqlconnect/saveData.php";



        Debug.Log("Sending request to: " + url);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        www.timeout = 10; // seconds

        yield return www.SendWebRequest();


        string response = www.downloadHandler.text.Trim();
        Debug.Log("Server response: " + response);

        if (response == "0")
        {
            Debug.Log("Game saved");
        }
        else
        {
            Debug.Log("Save failed. Error #" + response);
        }

        DBManager.LogOut();
        SceneManager.LoadScene("MainMenu");

    }

    public void InscreaseScore()
    {
        DBManager.score++;
        scoreDisplay.text = "Score: "+DBManager.score;  
    }
}
