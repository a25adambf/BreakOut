using UnityEngine;
using TMPro;

public class GUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtScore;
    [SerializeField] TextMeshProUGUI txtLives;

    private void OnGUI()
    {
        txtScore.text = string.Format("{0,3:D3}", GameManager.Score); 

        txtLives.text = GameManager.Lives.ToString();
    }
}