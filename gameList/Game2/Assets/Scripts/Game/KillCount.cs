using UnityEngine;
using UnityEngine.SceneManagement;

public class KillCount : MonoBehaviour
{
    private static Game game;

    private void Awake()
    {
        if (DBManager.username == null)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        // Cache Game reference once
        game = FindObjectOfType<Game>();

        DBManager.kills = 0;

        if (game != null && game.killDisplay != null)
        {
            game.killDisplay.text = "Kills: " + DBManager.kills;
        }
    }

    public static void AddKill(int value = 1)
    {
        DBManager.kills += value;

        if (game == null)
            game = FindObjectOfType<Game>();

        if (game != null && game.killDisplay != null)
        {
            game.killDisplay.text = "Kills: " + DBManager.kills;
        }
    }
}
