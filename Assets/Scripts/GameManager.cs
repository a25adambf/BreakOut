using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] int lives; 
    [SerializeField] TextMeshProUGUI txtlives; 
    // Variable para llevar el control de la puntuación
    int score = 0;
    
    // Referencia al texto para mostrar la puntuación en la interfaz
    [SerializeField] TextMeshProUGUI txtScore;
    private void OnGUI()
    {
        // Actualizamos el texto de la puntuación
        txtScore.text = string.Format("{0,3:D3}", score);  // Formateamos a 3 dígitos

        txtlives.text = lives.ToString();
    }
        // Método para actualizar la puntuación
    public void UpdateScore(int points)
    {
        score += points;

        
    }

        // Método para actualizar las vidas
    public void Updatelives()
    {
        lives--;
    }

    
}