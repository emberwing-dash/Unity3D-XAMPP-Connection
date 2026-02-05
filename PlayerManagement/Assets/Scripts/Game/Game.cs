using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public Text playerDisplay;
    public Text scoreDisplay;

    private void Awake()
    {
        if(DBManager.username==null)
        {
            SceneManager.LoadScene(0); //load menu scene if not logged in
        }
        playerDisplay.text = "Player: " + DBManager.username;
        scoreDisplay.text = "Score: " + DBManager.score;
    }

    public void CallSaveData()
    {
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
        SceneManager.LoadScene(0);

    }

    public void InscreaseScore()
    {
        DBManager.score++;
        scoreDisplay.text = "Score: "+DBManager.score;  
    }
}
