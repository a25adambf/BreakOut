using System.Security.Cryptography;
using UnityEngine;

public class PaController : MonoBehaviour
{
    const float MAX_X = 3.1f;
    const float MIN_X = -3.1f;
    [SerializeField] float speed;


    void Update()
    {
        float x = transform.position.x; // Obtener la posición actual de x de la pala
        if (x > MIN_X && Input.GetKey(KeyCode.LeftArrow))
        {
            // Desplazamiento hacia la izquierda con un valor negativo
            // Utilizamos deltaTime para obtener una referencia de la velocidad independiente del hardware
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (x < MAX_X && Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0); // Desplazamiento hacia la derecha
        }
    }
}
