using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealScript : MonoBehaviour
{
    public RabbitController playerScriptReference;


    void OnTriggerEnter2D(Collider2D triger)
    {
        if (triger.gameObject.CompareTag("Player"))
        {
            playerScriptReference.health++;
            Destroy(this.gameObject);
        }
    }
}
