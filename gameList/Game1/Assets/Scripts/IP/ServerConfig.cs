using UnityEngine;

public static class ServerConfig
{
    const string KEY = "SERVER_IP";

    private static string serverIP;

    // called automatically first time used
    static ServerConfig()
    {
        serverIP = PlayerPrefs.GetString(KEY, "");
    }

    public static void SetIP(string ip)
    {
        serverIP = ip;
        PlayerPrefs.SetString(KEY, ip);
        PlayerPrefs.Save();
    }

    public static string GetIP()
    {
        return serverIP;
    }
}
