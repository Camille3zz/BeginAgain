using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpButton : MonoBehaviour
{

    public void GoUp()
    {
        GameObject player = GameObject.Find("Player");
        MoveByCommand t1 = player.GetComponent<MoveByCommand>();
        MoveRain t2 = player.GetComponent<MoveRain>();

        if (t1 != null)
        {
            if (t1.isInputting == true)
            {
                t1.fullCommand += "w";
                t1.text.text += "↑";
                Time.timeScale = 1.0f;
            }
        }

        if (t2 != null)
        {
            if (t2.isInputting == true)
            {
                t2.fullCommand += "ww";
                t2.text.text += "↑";
                Time.timeScale = 1.0f;
            }
        }

    }
}
