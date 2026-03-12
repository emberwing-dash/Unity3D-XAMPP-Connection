using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI playerDisplay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI killDisplay;

    [Header("Exit Menu")]
    public GameObject exitPanel;

    private bool isPaused = false;

    // -------------------- AWAKE --------------------
    private void Awake()
    {
        if (DBManager.username == null)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        // Reset values for new game
        DBManager.score = 0;
        DBManager.kills = 0;

        playerDisplay.text = "Player: " + DBManager.username;
        scoreDisplay.text = "Score: " + DBManager.score;
        killDisplay.text = "Kills: " + DBManager.kills;
    }

    // -------------------- START --------------------
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // -------------------- UPDATE --------------------
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleExitMenu();
        }
    }

    // -------------------- SCORE --------------------
    public void IncreaseScore(int value = 1)
    {
        DBManager.score += value;
        scoreDisplay.text = "Score: " + DBManager.score;
    }

    // -------------------- KILLS --------------------
    public void IncreaseKills(int value = 1)
    {
        DBManager.kills += value;
        killDisplay.text = "Kills: " + DBManager.kills;
    }

    // -------------------- PAUSE MENU --------------------
    void ToggleExitMenu()
    {
        isPaused = !isPaused;

        exitPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // -------------------- SAVE & EXIT --------------------
    public void CallSaveData()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        StartCoroutine(SavePlayerData());
    }

    IEnumerator SavePlayerData()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", DBManager.username);
        form.AddField("score", DBManager.score);
        form.AddField("kills", DBManager.kills);

        string url = $"http://{ServerConfig.GetIP()}/sqlconnect/saveData.php";
        Debug.Log("Sending request to: " + url);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        www.timeout = 10;

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Server response: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Save failed: " + www.error);
        }

        DBManager.LogOut();
        SceneManager.LoadScene("MainMenu");
    }
}
