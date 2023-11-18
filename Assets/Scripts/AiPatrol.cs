using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrol : MonoBehaviour
{
    public float speed;
    public bool facingRight;

    void Update()
    {
        if (facingRight == true)
        {
            transform.Translate(1 * Time.deltaTime * speed, 0, 0);
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.Translate(-1 * Time.deltaTime * speed, 0, 0);
            transform.localScale = new Vector2(-1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("AI"))
        {
            if (facingRight == true)
            {
                facingRight = false;
            }
            else
            {
                facingRight = true;
            }
        }
        {
            if (trig.gameObject.name == "Rabbit")
            {
                Destroy(this.gameObject);
            }

            
        }
    }
}
