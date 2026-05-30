using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PelotaController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float delay;
    [SerializeField] float force;
    //[SerializeField] GameManager gameManager;

    [SerializeField] AudioClip sfxPaddel;  // Sonido al chocar con la pala
    [SerializeField] AudioClip sfxBrick;   // Sonido al chocar con un ladrillo
    [SerializeField] AudioClip sfxWall;    // Sonido al chocar con una pared
    [SerializeField] AudioClip sfxFail;    // Sonido al salir por la pared inferior
    [SerializeField] AudioClip sfxNextLevel;   


    GameObject pala;

    bool halved = false;

    // Mantenemos un registro de los golpes con la pala.
    int contadorGolpes = 0;

    // Definimos la fuerza a aplicar para aumentar la velocidad.
    [SerializeField] float fuerzaIncrementada;

    AudioSource sfx;  // Componente AudioSource


    // Acumulamos los ladrillos que destruimos 
    int brickCount;

    int sceneId;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pala = GameObject.FindWithTag("Pala");
        Invoke("LanzarPelota", delay);
        sfx = GetComponent<AudioSource>();

        sceneId = SceneManager.GetActiveScene().buildIndex;

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

        // Si atravesamos un ladrillo rojo atravesable 
        if (other.tag == "LadrilloAtravesable")
        {
            //Sumamos puntos
            GameManager.UpdateScore(ladrillos[other.tag]);
            //Sonido del ladrillo
            sfx.clip = sfxBrick;
            sfx.Play();
            //Se desactiva el collider para que la pelota no detecte el "Trigger" y no sumar puntos
            other.enabled = false;
        }
        // Comprobamos que el objeto que estamos atravesando es la pared inferior
        if (other.tag == "Pared inferior")
        {
            GameManager.UpdateLives();

            // Si ya no quedan vidas
            if (GameManager.Lives <= 0)
            {
                rb.linearVelocity = Vector2.zero;
                gameObject.SetActive(false);
                return;  // No relanzar la pelota
            }

            // Si aún quedan vidas se vuelve a lanzar la pelota
            if (halved)
            {
                HalvePaddle(false);
            }
            Invoke("LanzarPelota", delay);
            sfx.clip = sfxFail;
            sfx.Play();
        }


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Almacenamos la etiqueta del objeto con el que estamos colisionando
        string tag = other.gameObject.tag;

        pala = GameObject.FindWithTag("Pala");

        if (!halved && tag == "Pared superior")
        {
            HalvePaddle(true);
        }

        if (tag == "Pala")
        {
            Vector3 pala = other.gameObject.transform.position;
            Vector2 contact = other.GetContact(0).point;

            if (rb.linearVelocity.x < 0 && contact.x > pala.x ||
                rb.linearVelocity.x > 0 && contact.x < pala.x)
            {
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
            DestroyBrick(other.gameObject);
        }
        else if (tag == "Pared derecha" || tag == "Pared izquierda" || tag == "Pared superior" || tag == "Brikc-rock")
        {
            sfx.clip = sfxWall;
            sfx.Play();
        }


    }

    public void HalvePaddle(bool reducir)
    {
        halved = reducir;
        Vector3 escalaActual = pala.transform.localScale;
        pala.transform.localScale = reducir ?
            new Vector3(escalaActual.x * 0.5f, escalaActual.y, escalaActual.z) :
            new Vector3(escalaActual.x * 2f, escalaActual.y, escalaActual.z);
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
        {"LadrilloAtravesable", 10}
};


    public void DestroyBrick(GameObject obj)
    {
        sfx.clip = sfxBrick;
        sfx.Play();
        // Actualizamos la puntuación 
        GameManager.UpdateScore(ladrillos[obj.tag]);
        // Se destruye el objeto
        Destroy(obj, 0.05f);
        // Actualizamos el contador de ladrillos destruidos
        ++brickCount;
        // Comprobamos si hemos alcanzado el máximo de ladrillos. Necesitamos el índice de la escena en la que nos encontramos para saber cuántos ladrillos tenemos.
        if (brickCount == GameManager.totalBricks[sceneId])
        {

            sfx.clip = sfxNextLevel;
            sfx.Play();
            // Detenemos el movimiento de la pelota
            rb.linearVelocity = Vector2.zero;
            Invoke("NextScene", 3);
        }

    }


    void NextScene()
    {
        int nextId = sceneId + 1;
        if (nextId == SceneManager.sceneCountInBuildSettings)
        {
            nextId = 0;
        }
        SceneManager.LoadScene(nextId);
    }
}
