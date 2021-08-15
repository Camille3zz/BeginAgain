using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MoveByCommand : MonoBehaviour
{

    public Text text;               //右下角文本内容
    public float speed;             //机器人移动的速度
    private float countDown;        //机器人移动的时间间隔
    public Transform transform;     //机器人的transform组件
    public string fullCommand;      //玩家输入的指令
    public float gridDistance;      //网格间的距离
    public float moveDistance;      //机器人移动距离
    public int count;               //步数记录
    public bool isMoving;           //机器人是否在进行移动
    public bool isInputting;       //是否允许玩家输入指令
    private int sceneIndex;         //场景序号
    public bool oneStepFinished;    //角色是否完成单步移动
    private int layerMask;          //设置射线检测忽略的层数
    public Sprite RobotUp;          //机器人向上移动图片
    public Sprite RobotDown;        //机器人向下移动图片
    public Sprite RobotLeft;        //机器人向左移动图片
    public Sprite RobotRight;       //机器人向右移动图片
    public Transform fog;           //雾
    public Button Go;               //开始按钮
    public Button Delete;           //删除按钮
    public Button Up;               //向上按钮
    public Button Down;             //向下按钮
    public Button Left;             //向左按钮
    public Button Right;            //向右按钮
    public Button Attack;           //攻击按钮
    public GameObject AttackLeft;   //向左攻击
    public GameObject AttackRight;  //向右攻击
    public GameObject AttackUp;     //向上攻击
    public GameObject AttackDown;   //向下攻击
    public Transform AttackLeftPos;
    public Transform AttackRightPos;
    public Transform AttackUpPos;
    public Transform AttackDownPos;

    // Start is called before the first frame update
    void Start()
    {
        Go.interactable = true;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        oneStepFinished = false;
        count = 0;
        layerMask = 1 << 10;
        layerMask = ~layerMask;
        fullCommand = "";
        isMoving = false;
        isInputting = true;
    }

    
    void FixedUpdate()
    {
        //当玩家按下上下左右时，系统会捕获这个指令并添加到fullCommand中，并且文本显示指令
        if (isInputting == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))    //按下上键
            {
                fullCommand += "w";     //对于指令来说，将上键转换为w方便处理
                text.text += "↑ ";     //文本框添加显示
                Debug.Log("上");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))    //按下下键
            {
                fullCommand += "s";
                text.text += "↓ ";
                Debug.Log("下");
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))    //按下左键
            {
                fullCommand += "a";
                text.text += "← ";
                Debug.Log("左");
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))    //按下右键
            {
                fullCommand += "d";
                text.text += "→ ";
                Debug.Log("右");
            }

            if (Input.GetKeyDown(KeyCode.J))             //按下攻击键
            {
                fullCommand += "j";
                text.text += "j";
            }

            if (fullCommand.Length >= 4)                //玩家最大输入指令长度为4
                isInputting = false;


        }

        if (fullCommand.Length < 4)     //当用户撤销指令后，可以继续输入
            isInputting = true;

        if (fullCommand == "j" || fullCommand == "jj" || fullCommand == "jjj" || fullCommand == "jjjj") //避免玩家只输入攻击导致游戏闪退
            Go.interactable = false;
        else
            Go.interactable = true;

        //当玩家按下回车键时，机器人开始执行命令
        if(Input.GetKeyDown(KeyCode.Return) && isMoving == false)
        {
            isMoving = true;
            isInputting = false;
            Debug.Log(fullCommand);
        }

        //移动代码
        if(isMoving == true)
        {
            //指令相关按钮失效
            Go.interactable = false;
            Up.interactable = false;
            Down.interactable = false;
            Left.interactable = false;
            Right.interactable = false;
            Delete.interactable = false;
            Attack.interactable = false;    

            oneStepFinished = false;
            if (countDown >= 1.0f)      //一秒一步
            {
                if (count > fullCommand.Length - 1)     //判断count是否超出索引，超出则重新循环
                    count = 0;

                Move(fullCommand, count);   //调用移动函数
                countDown = 0;
                oneStepFinished = true;     //标志当前单步指令执行完毕
                //Debug.Log(this.gameObject.GetComponent<PlayerStatus>().currentHealth);
                count++;
            }
            countDown += Time.deltaTime;
        }

    }

    //机器人执行玩家指令进行移动
    void Move(string command, int count)
    {
        if (command[count] == 'w')  //向上
        {
            //使用射线检测判断下一步移动位置是否有无法通过的墙体
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), Vector2.up, gridDistance, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Collider")
                    return;
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, moveDistance, 0), speed * Time.deltaTime);
                    this.GetComponent<SpriteRenderer>().sprite = RobotUp;   //移动同时替换机器人图片
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, moveDistance, 0), speed * Time.deltaTime);
                this.GetComponent<SpriteRenderer>().sprite = RobotUp;
            }
        }

        if (command[count] == 'a')  //向左
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 2.0f, transform.position.y), Vector2.left, gridDistance, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Collider")
                    return;
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(-moveDistance, 0, 0), speed * Time.deltaTime);
                    this.GetComponent<SpriteRenderer>().sprite = RobotLeft;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(-moveDistance, 0, 0), speed * Time.deltaTime);
                this.GetComponent<SpriteRenderer>().sprite = RobotLeft;
            }
        }

        if (command[count] == 's')  //向下
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 2.0f), Vector2.down, gridDistance, layerMask);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.gameObject.tag == "Collider")
                    return;
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, -moveDistance, 0), speed * Time.deltaTime);
                    this.GetComponent<SpriteRenderer>().sprite = RobotDown;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, -moveDistance, 0), speed * Time.deltaTime);
                this.GetComponent<SpriteRenderer>().sprite = RobotDown;
            }
        }

        if (command[count] == 'd')  //向右
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 2.0f, transform.position.y), Vector2.right, gridDistance, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Collider")
                    return;
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(moveDistance, 0, 0), speed * Time.deltaTime);
                    this.GetComponent<SpriteRenderer>().sprite = RobotRight;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(moveDistance, 0, 0), speed * Time.deltaTime);
                this.GetComponent<SpriteRenderer>().sprite = RobotRight;
            }
        }

        if (command[count] == 'j')  //攻击
        {
            int lastCountIndex = 0;
            if (count == 0)  //如果攻击是第一个指令，则攻击方向为最后一个指令
                lastCountIndex = command.Length - 1;
            else
                lastCountIndex = count - 1;     //否则攻击方向为上一个指令

            if (command[lastCountIndex] == 'a')  //向左攻击
            {
                GameObject attLeft = Instantiate(AttackLeft, AttackLeftPos);
                GameObject.Destroy(attLeft, 0.5f);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x - 2.0f, transform.position.y), Vector2.left, gridDistance, layerMask);
                //射线检测向左

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Enemy")    //检测到敌人则敌人被消灭
                        GameObject.Destroy(hit.collider.gameObject);
                }
                else
                    return;
            }

            else if (command[lastCountIndex] == 'w')  //向上攻击
            {
                GameObject attUp = Instantiate(AttackUp, AttackUpPos);
                GameObject.Destroy(attUp, 0.5f);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), Vector2.up, gridDistance, layerMask);
                //射线检测向上

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Enemy")    //检测到敌人则敌人被消灭
                        GameObject.Destroy(hit.collider.gameObject);
                }
                else
                    return;
            }

            else if (command[lastCountIndex] == 'd')  //向右攻击
            {
                GameObject attRight = Instantiate(AttackRight, AttackRightPos);
                GameObject.Destroy(attRight, 0.5f);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x + 2.0f, transform.position.y), Vector2.right, gridDistance, layerMask);
                //射线检测向右

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Enemy")    //检测到敌人则敌人被消灭
                        GameObject.Destroy(hit.collider.gameObject);
                }
                else
                    return;
            }

            else if (command[lastCountIndex] == 's')  //向下攻击
            {
                GameObject attDown = Instantiate(AttackDown, AttackDownPos);
                GameObject.Destroy(attDown, 0.5f);
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 2.0f), Vector2.down, gridDistance, layerMask);
                //射线检测向下

                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Enemy")    //检测到敌人则敌人被消灭
                        GameObject.Destroy(hit.collider.gameObject);
                }
                else
                    return;
            }

            else if (command[lastCountIndex] == 'j')    //重复攻击(最后一关需要用到)
            {
                Move(fullCommand, lastCountIndex);
                return;
            }

            else
                return;
        }
    }

}
