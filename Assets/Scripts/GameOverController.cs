using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI GameOver;
    bool gameOver = false;

    void Update()
    {
        if (!gameOver && GameManager.Lives <= 0)
        {
            GameOver.gameObject.SetActive(true);
            gameOver = true;
        }
        
        if (gameOver && Input.anyKeyDown)
        {
            GameManager.ResetGame();
        }
    }
}