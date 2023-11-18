using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UtilityScript : MonoBehaviour
{
    public RabbitController util_carrot;
    public Image[] Util;
   
    void Update()
    {
        if (util_carrot.redBuff <= 0)
        {
           Util[0].enabled = false;
        }else
        {
            Util[0].enabled = true;
        }
        if (util_carrot.blueBuff <= 0)
        {
            Util[1].enabled = false;
        }
        else
        {
            Util[1].enabled = true;
        }


    }
}
