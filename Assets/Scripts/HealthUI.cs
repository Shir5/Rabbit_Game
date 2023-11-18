using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public RabbitController hp_carrot;
    public int numOfHp;
    public Image[] Hp;
    public Sprite fullHp;
    public Sprite emptyHp;

    void Update()
    {
        if (hp_carrot.health > numOfHp)
        {
            hp_carrot.health = numOfHp;
        }
        for (int i = 0; i < Hp.Length; i++)
        {
            if (i < numOfHp)
            {
                Hp[i].enabled = true;
            }
            else
            {
                Hp[i].enabled = false;
            }
            if (i < hp_carrot.health)
            {
                Hp[i].sprite = fullHp;
            }
            else
            {
                Hp[i].sprite = emptyHp;
            }      
        }
    }
    
}
