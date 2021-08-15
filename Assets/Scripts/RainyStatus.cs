using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RainyStatus : MonoBehaviour    //雨天中玩家触发机关会有不同的效果，以PlayerStatus为基础修改
{

    private int sceneIndex;        //场景序号
    public int maxHealth;          //角色最大生命值
    public int currentHealth;      //角色当前生命值
    private MoveRain t;       //角色移动脚本
    private float timer;           //计时器
    public Transform portalOut;    //传送门出口
    public GameObject mainCamera;  //摄像机
    public Transform moveCamera;   //摄像机移动位置，仅用于多地图传送门关卡
    public Image HP3;               //满血图片
    public Image HP2;               //半血图片
    public Image HP1;               //残血图片
    public Image NextLevel;         //下一关菜单

    // Start is called before the first frame update
    void Start()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        maxHealth = 3;
        currentHealth = maxHealth;
        t = this.gameObject.GetComponent<MoveRain>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHealth == 2)
        {
            HP3.enabled = false;
        }

        if (currentHealth == 1)
        {
            HP2.enabled = false;
        }

        if (currentHealth == 0)     //生命值降为0则游戏结束
        {
            SceneManager.LoadScene(sceneIndex);
            Time.timeScale = 0.0f;
        }
        Debug.Log(t.count);
    }

    private void OnTriggerEnter2D(Collider2D collision)     //场景中各种机关触发
    {

        if (collision.gameObject.tag == "Final")        //到达终点则跳转到下一个场景
        {
            NextLevel.gameObject.SetActive(true);
            Time.timeScale = 0.0f;
        }

        if (collision.gameObject.tag == "Trap")         //掉到坑里则关卡重新开始
        {
            SceneManager.LoadScene(sceneIndex);
            Time.timeScale = 0.0f;
        }

        if (collision.gameObject.tag == "Nuclear")       //角色在核废料周围3*3区域内每次行动扣除一点生命值
        {
            currentHealth--;
        }

        if (collision.gameObject.tag == "EMP")           //在雨天中，干扰区会屏蔽玩家指令但不会影响玩家滑行
        {
            if(t.count % 2 == 0)                         //表示玩家通过指令移动到干扰区，则干扰区屏蔽下一个指令，否则不作处理
            {
                t.count += 2;
            }
            if (t.count > t.fullCommand.Length)          //假如忽略的是第一个指令则要做特殊处理
                t.count = 2;
        }

        if (collision.gameObject.tag == "BeltUp")        //传送带的机制是忽略下一个指令并进行强制位移
        {
            t.count++;                                   //处理方法同EMP
            if (t.count > t.fullCommand.Length)
                t.count = 1;
        }

        if (collision.gameObject.tag == "BeltDown")
        {
            t.count++;
            if (t.count > t.fullCommand.Length)
                t.count = 1;
        }

        if (collision.gameObject.tag == "BeltLeft")
        {
            t.count++;
            if (t.count > t.fullCommand.Length)
                t.count = 1;
        }

        if (collision.gameObject.tag == "BeltRight")
        {
            t.count++;
            if (t.count > t.fullCommand.Length)
                t.count = 1;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Nuclear")       //角色在核废料周围3*3区域内每次行动扣除一点生命值
        {
            if (t.oneStepFinished == true)
                currentHealth--;
        }

        if (collision.gameObject.tag == "BeltUp")        //传送带强制向上位移
        {
            t.isMoving = false;     //暂时停止执行移动指令
            if (timer >= 1.0f)      //一秒后强制位移并恢复执行指令
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position +
                  new Vector3(0, t.moveDistance, 0), t.speed * Time.deltaTime);
                t.isMoving = true;
                timer = 0;
            }
            timer += Time.deltaTime;
        }

        if (collision.gameObject.tag == "BeltDown")        //传送带强制向下位移
        {
            t.isMoving = false;     //暂时停止执行移动指令
            if (timer >= 1.0f)      //一秒后强制位移并恢复执行指令
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position +
                  new Vector3(0, -t.moveDistance, 0), t.speed * Time.deltaTime);
                t.isMoving = true;
                timer = 0;
            }
            timer += Time.deltaTime;
        }

        if (collision.gameObject.tag == "BeltLeft")        //传送带强制向左位移
        {
            t.isMoving = false;     //暂时停止执行移动指令
            if (timer >= 1.0f)      //一秒后强制位移并恢复执行指令
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position +
                  new Vector3(-t.moveDistance, 0, 0), t.speed * Time.deltaTime);
                t.isMoving = true;
                timer = 0;
            }
            timer += Time.deltaTime;
        }

        if (collision.gameObject.tag == "BeltRight")        //传送带强制向右位移
        {
            t.isMoving = false;     //暂时停止执行移动指令
            if (timer >= 1.0f)      //一秒后强制位移并恢复执行指令
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.position +
                  new Vector3(t.moveDistance, 0, 0), t.speed * Time.deltaTime);
                t.isMoving = true;
                timer = 0;
            }
            timer += Time.deltaTime;
        }

        if (collision.gameObject.tag == "PortalIn")        //进入一个传送门则会被传送到另一个传送门
        {
            t.isMoving = false;     //暂时停止执行移动指令
            if (timer >= 1.0f)      //一秒后强制位移并恢复执行指令
            {
                Debug.Log("IN");
                mainCamera.transform.position = moveCamera.position;    //单地图传送门则moveCamera位于原位置，多地图传送门则移动摄像机   
                transform.position = portalOut.position;
                t.isMoving = true;
                timer = 0;
            }
            timer += Time.deltaTime;
        }

    }
}
