using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{

    public void Attack()
    {
        GameObject player = GameObject.Find("Player");
        MoveByCommand t1 = player.GetComponent<MoveByCommand>();

        if (t1 != null)
        {
            if (t1.isInputting == true)
            {
                t1.fullCommand += "j";
                t1.text.text += "A";
                Time.timeScale = 1.0f;
            }
        }

    }
}
