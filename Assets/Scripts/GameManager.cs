using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static int Score { get; private set; } = 0;
    public static int Lives { get; private set; } = 3;
    public static List<int> totalBricks = new List<int> { 0, 28, 28 };

    public static void UpdateScore(int points) { Score += points; }

    public static void UpdateLives() { Lives--; }
    public static void ResetGame()
    {
        Score = 0;

        Lives = 3;

        SceneManager.LoadScene(0);
    }
}