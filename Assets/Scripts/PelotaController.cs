using UnityEngine;
using System.Collections.Generic;

public class PelotaController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float delay;
    [SerializeField] float force;
    [SerializeField] GameManager gameManager;

    [SerializeField] AudioClip sfxPaddel;  // Sonido al chocar con la pala
    [SerializeField] AudioClip sfxBrick;   // Sonido al chocar con un ladrillo
    [SerializeField] AudioClip sfxWall;    // Sonido al chocar con una pared
    [SerializeField] AudioClip sfxFail;    // Sonido al salir por la pared inferior


    // Mantenemos un registro de los golpes con la pala.
    int contadorGolpes = 0;

    // Definimos la fuerza a aplicar para aumentar la velocidad.
    [SerializeField] float fuerzaIncrementada;

    AudioSource sfx;  // Componente AudioSource

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("LanzarPelota", delay);
        sfx = GetComponent<AudioSource>();

    }


    private void LanzarPelota()
    {
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        float dirX, dirY = -1;
        dirX = Random.Range(0, 2) == 0 ? -1 : 1;
        Vector2 dir = new Vector2(dirX, dirY);
        dir.Normalize();

        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos que el objeto que estamos atravesando es la pared inferior
        if (other.tag == "Pared inferior")
        {
            // Actualizamos el contador de vidas
            gameManager.Updatelives();

            // Volvemos a lanzar la pelota
            Invoke("LanzarPelota", delay);

            sfx.clip = sfxFail;
            sfx.Play();

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Almacenamos la etiqueta del objeto con el que estamos colisionando
        string tag = other.gameObject.tag;

        if (tag == "Pala")
        {   
            Vector3 pala = other.gameObject.transform.position;
            Vector2 contact = other.GetContact(0).point;

            if(rb.linearVelocity.x < 0 && contact.x > pala.x ||
                rb.linearVelocity.x > 0 && contact.x < pala.x){
                rb.linearVelocity = new Vector2(-rb.linearVelocityX, rb.linearVelocityY);
        }
            // Incrementamos el contador de golpes cada vez que la pelota golpea la pala.
            contadorGolpes++;

            // Si el contador de golpes es un múltiplo de 4, incrementamos la velocidad.
            if (contadorGolpes % 4 == 0)
            {
                // Aplicamos una fuerza adicional en la dirección actual de movimiento de la pelota.
                rb.AddForce(rb.linearVelocity * fuerzaIncrementada, ForceMode2D.Impulse);
            }
            sfx.clip = sfxPaddel;
            sfx.Play();
        }
        // Comprobamos si la etiqueta es un ladrillo 
        else if (ladrillos.ContainsKey(tag))
        {
            // Destruimos el objeto
            Destroy(other.gameObject);

            gameManager.UpdateScore(ladrillos[tag]);

            sfx.clip = sfxBrick;
            sfx.Play();
        }
        else if (tag == "ParedDerecha" || tag == "ParedIzquierda" || tag == "ParedSuperior")
        {
            sfx.clip = sfxWall;
            sfx.Play();
        }
    }




    // Estructura donde almacenaremos las etiquetas y la puntuación de cada ladrillo
    Dictionary<string, int> ladrillos = new Dictionary<string, int>(){
    {"LadrilloRojo", 10},
    {"LadrilloAmarillo", 15},
    {"LadrilloNaranja", 20},
    {"LadrilloVerde", 25},
    {"LadrilloCian", 30},
    {"LadrilloAzul", 35},
    {"LadrilloVioleta", 40},
};


}
