using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemydeath : MonoBehaviour
{


    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.name == "Rabbit")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
