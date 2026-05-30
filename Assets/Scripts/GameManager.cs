using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public static int Score { get; private set; } = 0; 
    public static int Lives { get; private set; } = 10;

    public static void UpdateScore(int points) { Score += points; }

    public static void UpdateLives() { Lives--; }

    public static int[] totalBricks = new int[] {0, 49, 32};

    public static void ResetGame()
    {
        Score = 0;
        Lives = 10;
        SceneManager.LoadScene(0);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}