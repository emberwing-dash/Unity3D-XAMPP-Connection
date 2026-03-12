using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public Transform contentParent;
    public GameObject rowTemplate;

    private void Start()
    {
        StartCoroutine(LoadLeaderboard());
    }

    IEnumerator LoadLeaderboard()
    {
        string baseURL = $"http://{ServerConfig.GetIP()}/sqlconnect/";

        // Update leaderboard on server
        yield return UnityWebRequest.Get(baseURL + "updateLeaderboard.php")
            .SendWebRequest();

        // Get leaderboard data
        UnityWebRequest req =
            UnityWebRequest.Get(baseURL + "getLeaderboard.php");
        yield return req.SendWebRequest();

        LeaderboardEntry[] entries =
            JsonHelper.FromJson<LeaderboardEntry>(req.downloadHandler.text);

        foreach (LeaderboardEntry e in entries)
        {
            GameObject row = Instantiate(rowTemplate, contentParent);
            row.SetActive(true);

            TextMeshProUGUI[] texts =
                row.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = e.rank.ToString();
            texts[1].text = e.name;
            texts[2].text = e.score.ToString();
            texts[3].text = e.kills.ToString();
        }
    }
}
