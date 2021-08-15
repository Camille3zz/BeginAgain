using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{

    public void Delete()
    {
        GameObject player = GameObject.Find("Player");
        MoveByCommand t1 = player.GetComponent<MoveByCommand>();
        MoveRain t2 = player.GetComponent<MoveRain>();

        if (t1 != null)
        {
            t1.fullCommand = t1.fullCommand.Remove(t1.fullCommand.Length - 1);
            t1.text.text = t1.text.text.Remove(t1.text.text.Length - 1);
        }

        if (t2 != null)
        {
            t2.fullCommand = t2.fullCommand.Remove(t2.fullCommand.Length - 1);
            t2.text.text = t2.text.text.Remove(t2.text.text.Length - 1);
        }

    }
}
