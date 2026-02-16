using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PelotaController : MonoBehaviour
{
    Rigidbody2D rb;

    int contadorGolpes = 0;

    int brickCount;

    [SerializeField] float fuerzaIncrementada;
    [SerializeField] float delay;
    [SerializeField] float force;
    [SerializeField] AudioClip sfxPaddel;  
    [SerializeField] AudioClip sfxBrick;   
    [SerializeField] AudioClip sfxWall;    
    [SerializeField] AudioClip sfxFail;    
    [SerializeField] GameObject pala;
    bool halved = false;

    AudioSource sfx;  

    int sceneId;

    Dictionary<string, int> ladrillos = new Dictionary<string, int>(){
    {"Ladrillo-Amarillo", 10},
    {"Ladrillo-Verde", 15},
    {"Ladrillo-Naranja", 20},
    {"Ladrillo-Rojo", 25},
    {"Ladrillo-Atravesable", 25},
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sfx = GetComponent<AudioSource>();
        sceneId = SceneManager.GetActiveScene().buildIndex;

        Invoke("LanzarPelota", delay);
    }

    void Update()
    {

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
        if (other.tag == "Ladrillo-Atravesable")
        {
            GameManager.UpdateScore(ladrillos[other.tag]);

            sfx.clip = sfxBrick;
            sfx.Play();

            other.enabled = false;
        }
        if (other.tag == "ParedInferior")
        {

            if (halved)
            {
                HalvePaddle(false);

            }
            GameManager.UpdateLives();

            sfx.clip = sfxFail;
            sfx.Play();

            if (GameManager.Lives <= 0)
            {
                rb.linearVelocity = Vector2.zero;
                gameObject.SetActive(false);
                return;
            }

            Invoke("LanzarPelota", delay);
        }
    }


    void DestroyBrick(GameObject obj)
    {
        sfx.clip = sfxBrick;
        sfx.Play();

        GameManager.UpdateScore(ladrillos[obj.tag]);

        Destroy(obj);
        ++brickCount;

        if (brickCount == GameManager.totalBricks[sceneId])
        {
            sfx.clip = sfxBrick;
            sfx.Play();

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
    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;
        if (!halved && tag == "ParedSuperior")
        {
            HalvePaddle(true);
        }


        if (ladrillos.ContainsKey(tag))
        {
            GameManager.UpdateScore(ladrillos[tag]);
            if (ladrillos.ContainsKey(tag) && tag != "Ladrillo-Atravesable")
            {
                DestroyBrick(other.gameObject);
            }
        }

        if (tag == "Pala")
        {
            Vector3 pala = other.gameObject.transform.position;
            Vector2 contact = other.GetContact(0).point;

            contadorGolpes++;

            if (contadorGolpes % 4 == 0)
            {
                rb.AddForce(rb.linearVelocity * fuerzaIncrementada, ForceMode2D.Impulse);
            }

            if (rb.linearVelocity.x < 0 && contact.x > pala.x ||
                    rb.linearVelocity.x > 0 && contact.x < pala.x)
            {
                rb.linearVelocity = new Vector2(-rb.linearVelocityX, rb.linearVelocityY);
            }
        }


        if (tag == "Pala")
        {
            sfx.clip = sfxPaddel;
            sfx.Play();
        }
        else if (ladrillos.ContainsKey(tag))
        {
            sfx.clip = sfxBrick;
            sfx.Play();
        }
        else if (tag == "ParedDerecha" || tag == "ParedIzquierda" || tag == "ParedSuperior" || tag == "Ladrillo-Indestructible")
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
}