using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MoveRain : MonoBehaviour       //MoveRain以MoveByCommand脚本为基础，只是修改指令添加方式
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
    public Button Go;               //开始按钮
    public Button Delete;           //删除按钮
    public Button Up;               //向上按钮
    public Button Down;             //向下按钮
    public Button Left;             //向左按钮
    public Button Right;            //向右按钮
    public Button Attack;           //攻击按钮

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        oneStepFinished = false;
        count = 0;
        layerMask = 1 << 10;
        layerMask = ~layerMask;
        fullCommand = "";
        isMoving = false;
        isInputting = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isInputting == true)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))   
            {
                fullCommand += "ww";    
                text.text += "↑ ";     
                Debug.Log("上");
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))   
            {
                fullCommand += "ss";
                text.text += "↓ ";
                Debug.Log("下");
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))    
            {
                fullCommand += "aa";
                text.text += "← ";
                Debug.Log("左");
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))    
            {
                fullCommand += "dd";
                text.text += "→ ";
                Debug.Log("右");
            }

            if (fullCommand.Length >= 8)                //玩家最大输入指令长度为4，对于雨天来说指令长度为8
                isInputting = false;
        }

        if (fullCommand.Length < 4)     //当用户撤销指令后，可以继续输入
            isInputting = true;


        if (Input.GetKeyDown(KeyCode.Return) && isMoving == false)
        {
            isMoving = true;
            isInputting = false;
            Debug.Log(fullCommand);
        }

        //移动代码
        if (isMoving == true)
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
            if (countDown >= 1.0f)   
            {
                if (count > fullCommand.Length - 1)     
                    count = 0;

                Move(fullCommand, count);   
                countDown = 0;
                oneStepFinished = true;     
                count++;
            }
            countDown += Time.deltaTime;
        }

    }


    void Move(string command, int count)
    {
        if (command[count] == 'w')
        {

            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 2.0f), Vector2.up, gridDistance, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Collider")
                    return;
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, moveDistance, 0), speed * Time.deltaTime);
                    this.GetComponent<SpriteRenderer>().sprite = RobotUp;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, moveDistance, 0), speed * Time.deltaTime);
                this.GetComponent<SpriteRenderer>().sprite = RobotUp;
            }
        }

        if (command[count] == 'a')  
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

        if (command[count] == 's') 
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

        if (command[count] == 'd') 
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
    }

}
