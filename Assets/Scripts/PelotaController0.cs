using UnityEngine;
using System.Collections.Generic;

public class PelotaController0 : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float delay;
    [SerializeField] float force;
    [SerializeField] AudioClip sfxPaddel; 
    [SerializeField] AudioClip sfxBrick;   
    [SerializeField] AudioClip sfxWall;    
    [SerializeField] AudioClip sfxFail;   
    AudioSource sfx;  

    Dictionary<string, int> ladrillos = new Dictionary<string, int>(){
    {"Ladrillo-Amarillo", 10},
    {"Ladrillo-Verde", 15},
    {"Ladrillo-Naranja", 20},
    {"Ladrillo-Rojo", 25},
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sfx = GetComponent<AudioSource>();
        Invoke("LanzarPelota", delay);
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






    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;




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
        else if (tag == "ParedDerecha" || tag == "ParedIzquierda" || tag == "ParedSuperior")
        {
            sfx.clip = sfxWall;
            sfx.Play();
        }
    }

}