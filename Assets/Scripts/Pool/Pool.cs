using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;
public class Pool : MonoBehaviour
{
   [Header("基础信息")]
    private Transform player;    //玩家位置
    private int num = 0;      //对象数量
    private int busyNum = 0;      //非空闲对象数量
    Dictionary<int, GameObject> objectPool = new Dictionary<int, GameObject>();         //对象表及其索引

    [Header("怪物列表")]
    public GameObject monsterfly1;      //基础房间
    public GameObject roomPerfab1;      //基础房间1
    public GameObject roomPerfab2;      //基础房间2
    public GameObject roomPerfab3;      //基础房间3
    //获取脚本对象
    //OtherRoomGenerator sc=GameObject.FindWithTag("OtherRoomGenerator").GetComponent<OtherRoomGenerator>();
    //添加监听
    void Start()
    {
        EventSystem.GetInstance().AddEventListener(EventType.ROOM_ENTER, ask);
    }
    //获取玩家位置
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //实例添加
    void init()
    {
        num++;
        string name = "精灵" + num.ToString();
        GameObject ob = new GameObject();
        ob.name = name;
        ob.transform.parent = gameObject.transform;
    }

    //怪物初始化
    void monsterInit(GameObject ob)
    {
        // 获取SpriteRenderer对象
        SpriteRenderer spr = ob.AddComponent<SpriteRenderer>();
        // 添加图片
        ob.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("snake");

        //定位到发射处
        ob.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
        ob.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        //获取刚体
        Rigidbody2D shoot = ob.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        shoot.constraints = RigidbodyConstraints2D.FreezeRotation;      //锁定旋转Z
        shoot.gravityScale = 0;                                         //重力为0

        //添加碰撞体
        CircleCollider2D col = ob.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
        col.isTrigger = true;                                           //配置成触发器

        //添加脚本
        //ob.AddComponent<Bullet>();
        //加入表中
        objectPool[num]=ob;
    }

    //怪物解除
    void monsterRes(GameObject ob)
    {
        List<Component> comList = new List<Component>();
        foreach (var component in gameObject.GetComponents<Component>())
        {
            comList.Add(component);
            print(component.GetType());
        }
        foreach (Component item in comList)
        {
            Destroy(item);
        }
    }

    //子弹初始化
    void bulletInit(GameObject ob)
    {
        // 获取SpriteRenderer对象
        SpriteRenderer spr = ob.AddComponent<SpriteRenderer>();
        // 添加图片
        ob.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("snake");


        //定位到发射处
        ob.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
        ob.transform.localScale = new Vector3(0.2f, 0.2f, 1f);

        //获取刚体
        Rigidbody2D shoot = ob.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        shoot.constraints = RigidbodyConstraints2D.FreezeRotation;      //锁定旋转Z
        shoot.gravityScale = 0;                                         //重力为0

        //添加碰撞体
        CircleCollider2D col = ob.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
        col.isTrigger = true;                                           //配置成触发器

        //添加脚本
        //ob.AddComponent<Bullet>();
        //加入表中
        objectPool[num]=ob;
    }

    //子弹解除
    void bulletRes(GameObject ob)
    {

    }

    public void ask(BaseEvent evt)
    {
        Packger p=(Packger)evt.Sender;
        GameObject p1 = GameObject.FindGameObjectWithTag("Player");
        p1=(GameObject)Instantiate(monsterfly1, player.position, Quaternion.identity);
        Debug.Log(p.type);
    }
}
