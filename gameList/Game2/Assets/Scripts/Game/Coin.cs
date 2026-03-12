using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    public int value = 1; // how much score this coin gives

    private void Awake()
    {
        if (DBManager.username == null)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }


        
    }


    private void OnTriggerEnter(Collider other)
    {
        // Check if player touched the coin
        if (other.CompareTag("Player"))
        {
            // Increase score
            DBManager.score += value;

            // Optional: update UI if Game exists in scene
            Game game = FindObjectOfType<Game>();
            if (game != null)
            {
                game.scoreDisplay.text = "Score: " + DBManager.score;
            }

            // Optional: sound / VFX here

            // Destroy coin
            Destroy(gameObject);
        }
    }
}
