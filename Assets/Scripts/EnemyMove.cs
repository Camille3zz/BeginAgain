using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour      //敌人的移动脚本
{

    public float speed;             //敌人移动的速度
    public Transform transform;     //敌人的transform组件
    private float countDown;        //敌人移动的时间间隔
    public string fullCommand;      //玩家输入的指令
    public float moveDistance;      //敌人移动距离
    public int count;               //步数记录
    public bool isMoving;           //敌人是否在进行移动

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
        count = 0;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return) && isMoving == false)
        {
            isMoving = true;
        }

        //移动代码
        if (isMoving == true)
        {
            if (countDown >= 1.0f)      //一秒一步
            {
                if (count > fullCommand.Length - 1)     //判断count是否超出索引，超出则重新循环
                    count = 0;

                Move(fullCommand, count);   //调用移动函数
                countDown = 0;
                count++;
            }
            countDown += Time.deltaTime;
        }
    }

    void Move(string command, int count)
    {
        if (command[count] == 'w')  //向上
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, moveDistance, 0), speed * Time.deltaTime);
        }

        if (command[count] == 'a')  //向左
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(-moveDistance, 0, 0), speed * Time.deltaTime);
        }

        if (command[count] == 's')  //向下
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, -moveDistance, 0), speed * Time.deltaTime);
        }

        if (command[count] == 'd')  //向右
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(moveDistance, 0, 0), speed * Time.deltaTime);
        }

    }

}
