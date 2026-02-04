using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] AudioClip sfx; 
    [SerializeField] Transform pala; 
    [SerializeField] GameObject pelota; 
    [SerializeField] float duration; 

    void Update()
    {
        if(Input.anyKeyDown){
            StartCoroutine("StartNextLevel");
        }
    }

    IEnumerator StartNextLevel(){
        Vector3 scaleStart = pala.localScale; 
        Vector3 scaleEnd = new Vector3(0, scaleStart.y, scaleStart.z);


        if (pelota != null)
        {
            pelota.SetActive(false);
        }

        float t = 0; 
        while(t< duration){
            t += Time.deltaTime; 
            pala.localScale = Vector3.Lerp(scaleStart, scaleEnd, t/duration);
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
}