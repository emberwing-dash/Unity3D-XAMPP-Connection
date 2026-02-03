using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebTest : MonoBehaviour
{
    [Header("Server Settings")]
    public string serverIP = "10.47.89.94";   // CHANGE WHEN NETWORK CHANGES
    public int serverPort = 8888;
    public string serverPath = "sqlconnect";

    private IEnumerator Start()
    {
        string url = $"http://{serverIP}:{serverPort}/{serverPath}/webtest.php";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        //Debug.Log(request.downloadHandler.text);

        webResult(request);
    }

    void webResult(UnityWebRequest request)
    {
        string[] webResults = request.downloadHandler.text.Split('\t');
        Debug.Log(webResults[0]);   
        int webNumber = int.Parse(webResults[1]);
        webNumber *= 2;
        Debug.Log(webNumber);   
    }
}
