using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownButton : MonoBehaviour
{

    public void GoDown()
    {
        GameObject player = GameObject.Find("Player");
        MoveByCommand t1 = player.GetComponent<MoveByCommand>();
        MoveRain t2 = player.GetComponent<MoveRain>();

        if (t1 != null)
        {
            if (t1.isInputting == true)
            {
                t1.fullCommand += "s";
                t1.text.text += "↓";
                Time.timeScale = 1.0f;
            }
        }

        if (t2 != null)
        {
            if (t2.isInputting == true)
            {
                t2.fullCommand += "ss";
                t2.text.text += "↓";
                Time.timeScale = 1.0f;
            }
        }

    }
}
