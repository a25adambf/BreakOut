using UnityEngine;

public class PalaController : MonoBehaviour
{
    const float MAX_X = 3.1f;
    const float MIN_X = -3.1f;
    [SerializeField] float speed;

    void Update()
    {
        float x = transform.position.x; 
        if (x > MIN_X && Input.GetKey("left"))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        else if (x < MAX_X && Input.GetKey("right"))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0); 
        }
    }
}
