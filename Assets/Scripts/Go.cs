using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Go : MonoBehaviour
{

    public void GoGo()
    {
        GameObject player = GameObject.Find("Player");
        MoveByCommand t1 = player.GetComponent<MoveByCommand>();
        MoveRain t2 = player.GetComponent<MoveRain>();
        GameObject Enemy = GameObject.Find("Enemy");
        EnemyMove t3;
        if (Enemy != null)
        {
            t3 = Enemy.GetComponent<EnemyMove>();
        }
        else
            t3 = null;


        if (t1 != null)
        {
            if (t1.isMoving == false)
            {
                t1.isMoving = true;
                t1.isInputting = false;
                Time.timeScale = 1.0f;
            }
        
        }

        if (t2 != null)
        {
            if (t2.isMoving == false)
            {
                t2.isMoving = true;
                t2.isInputting = false;
                Time.timeScale = 1.0f;
            }
        }

        if(t3 != null)
        {
            if (t3.isMoving == false)
            {
                t3.isMoving = true;
            }
        }

        
    }
}
