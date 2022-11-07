using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Point
{   
    public float x;
    public float y;
    public Point(){}
    public Point(float x1,float y1){ x=x1;y=y1; }
}
//传送数据包
public class Packger
{
    public int type;
    public Transform pos;
    public Packger(){}
    public Packger(int t,Transform p){ type=t;pos=p; }
}

public class OtherRoomGenerator : MonoBehaviour
{
    private int[] type1={0,3,6,9,12};
    private int[] type2={3,9};
    private int[] type3={0,3,6,9,12};
   
    [Header("房间信息")]
    public GameObject roomPerfab0;      //基础房间
    public GameObject roomPerfab1;      //基础房间1
    public GameObject roomPerfab2;      //基础房间2
    public GameObject roomPerfab3;      //基础房间3
    public GameObject shop;             //商店 房间型号3
    public GameObject hide;             //宝藏房 房间型号3
    public GameObject boss;             //boss房 房间型号2

    public GameObject standPlace;      //基础立足点
    public GameObject standPlace1;      //基础立足点1
    public GameObject standPlace2;      //基础立足点2
    public GameObject standPlace3;      //基础立足点3
    public GameObject standPlace4;      //基础立足点4
    public GameObject sWall;           //房间关闭点1

    private int snum;                   //立足点数量
    public GameObject loadPerfabs;      //基础道路
    public int roomNumber;              //房间数
    public Color startColor, endColor;  //始末房间颜色
    private GameObject endRoom;         
    private bool isleft=true;                //点是否在左边，是就左，否就右 
    private float type=2f;                 //当前房间类型
    private int high;                     //当前房间高度
    private float roomType=0f;                 //当前房间类型

    [Header("位置信息")]
    public Transform generatorPoint;    //房间点坐标
    public Transform loadPoint;         //通路点
    [SerializeField] private Transform player;    //玩家位置
    //public LayerMask RoomLayer;         //房间层级

    [Header("墙壁信息")]
    public Tilemap wallMap;
    public TileBase wallTile;
    //public Transform wallPoint;         //通路点

    public List<GameObject> rooms=new List<GameObject>();   //房间
    public List<GameObject> loads=new List<GameObject>();   //道路
    public List<Point> points=new List<Point>();            //房间中心点
    public List<int> types=new List<int>();                 //房间类型
    private List<GameObject> stands=new List<GameObject>();           //立足点
    public List<Point> loadpoints=new List<Point>();
    private List<Vector3Int> walls=new List<Vector3Int>();

    [Header("管理信息")]
    public int aliveMonster=0;                              //存活怪物数
    int playerRoom=0;                               //玩家所在房间
    List<int> r=new List<int>();                    //房间状态表
    bool isLocked=false;                            //是否被锁入房间
    bool isInRoom=false;                            //是否在房间内
    int roadEnter=0;                                //记录玩家进入那条道
    List<GameObject> lockWall=new List<GameObject>();



    //获取玩家位置
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        EventSystem.GetInstance().AddEventListener(EventType.MONSTER_DIE, monsterDie);

        roomMake();
        draWall();
        standPlaceInit();
    }

    //管理事件触发
      void Update()
    {
        playerlook();

        if(isLocked && !isInRoom)
            Invoke("roomLocked",0.2f);

        if(isLocked && aliveMonster<=0)
        {
            isLocked=false;
            lockWall[0].GetComponent<Animator>().SetTrigger("dis");
            lockWall[1].GetComponent<Animator>().SetTrigger("dis");
            Invoke("roomUnLocked",1.5f);
        }
    }
    void roomMake()
    {
        
        lockWall.Add(Instantiate(sWall, generatorPoint.position, Quaternion.identity));
        lockWall.Add(Instantiate(sWall, generatorPoint.position, Quaternion.identity));
        lockWall[0].transform.parent = gameObject.transform;
        lockWall[1].transform.parent = gameObject.transform;
        lockWall[0].SetActive(false);
        lockWall[1].SetActive(false);

        rooms.Add(Instantiate(roomPerfab0, generatorPoint.position, Quaternion.identity));
        //rooms[0].transform.localScale = new Vector3(1,3,1);
        rooms[0].transform.parent = gameObject.transform;
        points.Add(new Point(generatorPoint.position.x,generatorPoint.position.y));
        types.Add(0);
        r.Add(-1);
        wallPointAdd(generatorPoint.position.x,generatorPoint.position.y,0);
        Iniit();
    }
    
    void Iniit()
    {
        int hideRoom=Random.Range(2,4);
        int shopRoom=Random.Range(6,8);
        for (int i = 1; i <= roomNumber; i++)
        {
            if(i==hideRoom)
            {
                ChangPointPos(3,i);
                loads.Add(Instantiate(loadPerfabs, loadPoint.position, Quaternion.identity));
                r.Add(1);
                loads[i-1].transform.parent = gameObject.transform;

                rooms.Add(Instantiate(hide, generatorPoint.position, Quaternion.identity));
                //rooms[i].transform.localScale = new Vector3(1f,1f,1f);
                rooms[i].transform.parent = gameObject.transform;
                points.Add(new Point(generatorPoint.position.x,generatorPoint.position.y));
                loadpoints.Add(new Point(loadPoint.position.x,loadPoint.position.y));

                types.Add(3);
                //添加立足点
                wallPointAdd(generatorPoint.position.x,generatorPoint.position.y,3);//添加墙壁点
                loadWallPointAdd(loadPoint.position.x,loadPoint.position.y,isleft);//添加道路点
            }
            else if(i==shopRoom)
            {
                ChangPointPos(3,i);
                loads.Add(Instantiate(loadPerfabs, loadPoint.position, Quaternion.identity));
                r.Add(1);
                loads[i-1].transform.parent = gameObject.transform;

                rooms.Add(Instantiate(shop, generatorPoint.position, Quaternion.identity));
                //rooms[i].transform.localScale = new Vector3(1f,1f,1f);
                rooms[i].transform.parent = gameObject.transform;
                points.Add(new Point(generatorPoint.position.x,generatorPoint.position.y));
                loadpoints.Add(new Point(loadPoint.position.x,loadPoint.position.y));

                types.Add(3);
                //添加立足点
                wallPointAdd(generatorPoint.position.x,generatorPoint.position.y,3);//添加墙壁点
                loadWallPointAdd(loadPoint.position.x,loadPoint.position.y,isleft);//添加道路点
            }
            else if(i==roomNumber)
            {
                ChangPointPos(2,i);
                loads.Add(Instantiate(loadPerfabs, loadPoint.position, Quaternion.identity));
                r.Add(0);
                loads[i-1].transform.parent = gameObject.transform;

                rooms.Add(Instantiate(boss, generatorPoint.position, Quaternion.identity));
                //rooms[i].transform.localScale = new Vector3(1f,1f,1f);
                rooms[i].transform.parent = gameObject.transform;
                points.Add(new Point(generatorPoint.position.x,generatorPoint.position.y));
                loadpoints.Add(new Point(loadPoint.position.x,loadPoint.position.y));

                types.Add(2);
                //添加立足点
                wallPointAdd(generatorPoint.position.x,generatorPoint.position.y,2);//添加墙壁点
                loadWallPointAdd(loadPoint.position.x,loadPoint.position.y,isleft);//添加道路点
            }
            else
            {
                int randKey=Random.Range(1,4);
                ChangPointPos(randKey,i);
                loads.Add(Instantiate(loadPerfabs, loadPoint.position, Quaternion.identity));
                r.Add(0);
                loads[i-1].transform.parent = gameObject.transform;
                switch (randKey)
                {
                    //规格1:(0.5,2)  2:(1,2) 3:(1,1)
                    case 1:
                    rooms.Add(Instantiate(roomPerfab1, generatorPoint.position, Quaternion.identity));
                    //rooms[i].transform.localScale = new Vector3(0.5f,2f,1f);
                    rooms[i].transform.parent = gameObject.transform;
                    points.Add(new Point(generatorPoint.position.x,generatorPoint.position.y));
                    loadpoints.Add(new Point(loadPoint.position.x,loadPoint.position.y));
                    break;
                    case 2:
                    rooms.Add(Instantiate(roomPerfab2, generatorPoint.position, Quaternion.identity));
                    //rooms[i].transform.localScale = new Vector3(1f,2f,1f);
                    rooms[i].transform.parent = gameObject.transform;
                    points.Add(new Point(generatorPoint.position.x,generatorPoint.position.y));
                    loadpoints.Add(new Point(loadPoint.position.x,loadPoint.position.y));
                    break;
                    case 3:
                    rooms.Add(Instantiate(roomPerfab3, generatorPoint.position, Quaternion.identity));
                    //rooms[i].transform.localScale = new Vector3(1f,1f,1f);
                    rooms[i].transform.parent = gameObject.transform;
                    points.Add(new Point(generatorPoint.position.x,generatorPoint.position.y));
                    loadpoints.Add(new Point(loadPoint.position.x,loadPoint.position.y));
                    break;
                }
                types.Add(randKey);
                //添加立足点
                wallPointAdd(generatorPoint.position.x,generatorPoint.position.y,randKey);//添加墙壁点
                loadWallPointAdd(loadPoint.position.x,loadPoint.position.y,isleft);//添加道路点
            }
        }
    }

    void ChangPointPos(int key,int num)
    {
        if(num==roomNumber/2)
        {
            generatorPoint.position=new Vector3(0,6,0);
            loadPoint.position=new Vector3(-8.5f,6,0);
            isleft=false;
            type=2f;
            roomType=0f;
        }
        if(isleft)
        {
            if(key==1)
            {   
                int gen=hideJudge(roomType,key);
                if(type==1)
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(-10f,gen,0);
                    loadPoint.position += new Vector3(-10f,0,0);
                }
                else
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(-13.5f,gen,0);
                    loadPoint.position += new Vector3(-17f,0,0);  
                }
                type=1;roomType=1;
            }
            else if(key==2)
            {
                int gen=hideJudge(roomType,key);
                if(type==1)
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(-13.5f,gen,0);
                    loadPoint.position += new Vector3(-10f,0,0);
                }
                else
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(-17f,gen,0);
                    loadPoint.position += new Vector3(-17f,0,0);  
                }
                type=2;roomType=2;
            }
            else
            {
                int gen=hideJudge(roomType,key);
                if(type==1)
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(-13.5f,gen,0);
                    loadPoint.position += new Vector3(-10f,0,0);  
                }
                else
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(-17f,gen,0);
                    loadPoint.position += new Vector3(-17f,0,0);  
                }
                type=2;roomType=3;
            }
        }
        else
        {
            if(key==1)
            {   
                int gen=hideJudge(roomType,key);
                if(type==1)
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(10f,gen,0);
                    loadPoint.position += new Vector3(10f,0,0);  
                }
                else
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(13.5f,gen,0);
                    loadPoint.position += new Vector3(17f,0,0);  
                }
                type=1;roomType=1;
            }
            else if(key==2)
            {
                int gen=hideJudge(roomType,key);
                if(type==1)
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(13.5f,gen,0);
                    loadPoint.position += new Vector3(10f,0,0);  
                }
                else
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(17f,gen,0);
                    loadPoint.position += new Vector3(17f,0,0);  
                }
                type=2;roomType=2;
            }
            else
            {
                int gen=hideJudge(roomType,key);
                if(type==1)
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(13.5f,gen,0);
                    loadPoint.position += new Vector3(10f,0,0);  
                }
                else
                {
                    generatorPoint.position=new Vector3(generatorPoint.position.x,0,0);
                    generatorPoint.position += new Vector3(17f,gen,0);
                    loadPoint.position += new Vector3(17f,0,0);  
                }
                type=2;roomType=3;
            }
        }


    }

    int hideJudge(float lastroomtype,float nowroomtype)
    {
        int h=0;
        int top=0,low=0;
        if(lastroomtype==0)
        {
            switch (nowroomtype)
            {   
                case 1:
                h=type1[Random.Range(0,type1.Length)];
                if((h-3)>-3)
                    loadPoint.position=new Vector3(loadPoint.position.x,(h-3),0);
                else
                    loadPoint.position=new Vector3(loadPoint.position.x,-2f,0);
                high=h;
                return h;
                break;
                case 2:
                h=type2[Random.Range(0,type2.Length)];
                loadPoint.position=new Vector3(loadPoint.position.x,(h-3),0);
                high=h;
                return h;
                break;
                case 3:
                h=type3[Random.Range(0,type3.Length)];
                loadPoint.position=new Vector3(loadPoint.position.x,(h-2),0);
                high=h;
                return h;
                break;
            }
        }

        switch (lastroomtype)
        {
            case 1:
            case 2:
            top=high+6;
            low=high-6;
            break;
            case 3:
            top=high+3;
            low=high-3;
            break;
        }
        int dec=type3[Random.Range(0,type3.Length)];
        do
        {
            switch (nowroomtype)
            {
            case 1:
            dec=type1[Random.Range(0,type1.Length)];
            break;
            case 2:
            dec=type2[Random.Range(0,type2.Length)];
            break;
            case 3:
            dec=type3[Random.Range(0,type3.Length)];
            break;
            }
        } while (jud(top,low,dec,nowroomtype));
        high=dec;
        return dec;


    }

    bool jud(int top,int low,int h,float roomtype)
    {
        int t1=0,l1=0;
        switch (roomtype)
        {
            case 1:
            case 2:
            t1=h+6;
            l1=h-6;
            break;
            case 3:
            t1=h+3;
            l1=h-3;
            break;
        }
        if(low==l1)
        {
            loadPoint.position=new Vector3(loadPoint.position.x,(low+3),0);
        }
        else if(low>l1)
        {
            loadPoint.position=new Vector3(loadPoint.position.x,(low+1),0);
        }
        else if(low<l1)
        {
            loadPoint.position=new Vector3(loadPoint.position.x,(l1+1),0);
        }
        if(((l1<0)&&((t1-low)>=2))||((low<0)&&((top-l1)>=2)))
        {
            return false;
        }
        if( (((t1-low)>=2)&&(low>=0)&&(top>l1))||(((top-l1)>=2)&&(l1>=0)&&(t1>low)) )
        {
            return false;
        }
        return true;
    }

    void draWall()
    {   
        foreach (var i in walls)
        {
            wallMap.SetTile(i,wallTile);
        }
    }

    void wallPointAdd(float x,float y,int type)
    {
        float l=0;
        float s=0;
        switch (type)
        {
            case 0:
            l=16;
            s=18;
            break;
            case 1:
            l=9;
            s=12;
            break;
            case 2:
            l=16;
            s=12;
            break;
            case 3:
            l=16;
            s=6;
            break;
        }   
        int x1=0,y1=0;
        x1=(int)(x-l/2);
        y1=(int)(y+s/2);
        int num=x1+(int)l-1;
        for(;x1<=num;x1++)                      //上面从左往右
            walls.Add(new Vector3Int(x1,y1,0));
        num=y1-(int)s-1;x1--;
        for(y1-=1;y1>=num;y1--)                 //右边从上往下
            walls.Add(new Vector3Int(x1,y1,0));
        num=x1-(int)l+1;y1++;
        for(x1-=1;x1>=num;x1--)                 //下边从左往右
            walls.Add(new Vector3Int(x1,y1,0)); 
        num=y1+(int)s;x1++;
        for(y1+=1;y1<=num;y1++)                 //左边从下往上
            walls.Add(new Vector3Int(x1,y1,0)); 
    }

    void loadWallPointAdd(float x,float y,bool t)
    {
        float a=0,b=0,c=0;
        if(t)
        {
            a=-1.5f;
            b=-0.5f;
            c=0.5f;
        }
        else
        {
            a=1.5f;
            b=0.5f;
            c=-0.5f;
            x-=1;
        }
        judPoint(walls,(int)(x+a),(int)y);
        judPoint(walls,(int)(x+c),(int)y);
        judPoint(walls,(int)(x+a),(int)(y-1));
        judPoint(walls,(int)(x+c),(int)(y-1));
        judPoint(walls,(int)(x+b),(int)(y+1));
        judPoint(walls,(int)(x+b),(int)(y-2));
    }
    //判断墙壁点是否重合
    void judPoint(List<Vector3Int> walls,int x,int y)
    {
        for(int i=0;i<walls.Count;i++)
        {
            if((walls[i].x==x)&&(walls[i].y==y))
            {
                walls.RemoveAt(i);
                return;
            }
        }
        walls.Add(new Vector3Int((int)x,(int)y,0));
    }
    //创建立足点
    void standPlaceInit()
    {
        for(int i=0;i<roomNumber;i++)
        {
            if(r[i]<=0)
                standPlaceMake(types[i],i);
        }
    }
    void standPlaceMake(int key,int n)
    {
        switch (key)
        {
            case 0:
                stands.Add(Instantiate(standPlace, new Vector3(3f, 1f, 0f), Quaternion.identity));
                stands.Add(Instantiate(standPlace, new Vector3(3f, 9f, 0f), Quaternion.identity));
                stands.Add(Instantiate(standPlace, new Vector3(-3f, 1f, 0f), Quaternion.identity));
                stands.Add(Instantiate(standPlace, new Vector3(-3f, 9f, 0f), Quaternion.identity));
                stands.Add(Instantiate(standPlace, new Vector3(0f, 5f, 0f), Quaternion.identity));
                stands[0].transform.parent = gameObject.transform;
                stands[1].transform.parent = gameObject.transform;
                stands[2].transform.parent = gameObject.transform;
                stands[3].transform.parent = gameObject.transform;
                stands[4].transform.parent = gameObject.transform;
                snum+=5;
            break;
            case 1:
                stands.Add(Instantiate(standPlace1, new Vector3((points[n].x+2f), points[n].y-0.075f, 0f), Quaternion.identity));
                stands.Add(Instantiate(standPlace1, new Vector3((points[n].x-2f), points[n].y+2.925f, 0f), Quaternion.identity));
                stands.Add(Instantiate(standPlace1, new Vector3(points[n].x, (points[n].y-4f), 0f), Quaternion.identity));
                stands[snum++].transform.parent = gameObject.transform;
                stands[snum++].transform.parent = gameObject.transform;
                stands[snum++].transform.parent = gameObject.transform;
            break;
            case 2:
                stands.Add(Instantiate(standPlace3, new Vector3((points[n].x+5.5f), points[n].y+2.075f, 0f), Quaternion.identity));
                stands.Add(Instantiate(standPlace3, new Vector3((points[n].x-5.5f), points[n].y+2.075f, 0f), Quaternion.identity));
                stands.Add(Instantiate(standPlace4, new Vector3(points[n].x, (points[n].y-3f), 0f), Quaternion.identity));
                stands[snum++].transform.parent = gameObject.transform;
                stands[snum++].transform.parent = gameObject.transform;
                stands[snum++].transform.parent = gameObject.transform;
            break;
            case 3:
                stands.Add(Instantiate(standPlace2, new Vector3(points[n].x, points[n].y-1f, 0f), Quaternion.identity));
                stands[snum++].transform.parent = gameObject.transform;
            break;
        }
    }

    //玩家位置函数
    void playerlook()
    {
        for(int i=0;i<roomNumber;i++)
        {   
            if(player!=null && System.Math.Abs(player.position.x)>System.Math.Abs(loadpoints[i].x) && r[i+1]!=1 && ((player.position.x<0 && loadpoints[i].x<0) || (player.position.x>0 && loadpoints[i].x>0)))
            {
                if(i+1!=10)
                {
                    aliveMonster=0;
                    monsterMake(points[i+1],types[i+1]);
                    chestMake(points[i+1],types[i+1]);
                    isLocked=true;
                    roadEnter=i;
                }
                r[i+1]=1;
                playerRoom=i+1;
                //boss生成
                if(i+1==10)
                {
                    isLocked=true;
                    BossManger.instance.bossInit(generatorPoint.transform);
                    aliveMonster+=1;
                }
            }
        }
    }
    //生成怪物
    void monsterMake(Point point,int type)
    {
        switch (type)
        {   
            case 1:
                type1MonsterMake1(point);
            break;
            case 2:
                type1MonsterMake2(point);
            break;
            case 3:
                type1MonsterMake3(point);
            break;
        }
    }
    //生成宝箱
    void chestMake(Point point,int type)
    {
        switch (type)
        {   
            case 1:
                chestMake1(point);
            break;
            case 2:
                chestMake2(point);
            break;
            case 3:
                chestMake3(point);
            break;
        }
    }
    //一型房间生成怪物
    void type1MonsterMake1(Point point)
    {
        int m1=Random.Range(0,2);
        int flytype=CenterPool.instance.types[m1++];
        int m2=Random.Range(2,5);
        int groundtype=CenterPool.instance.types[m2++];
        //地面怪生成
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x,point.y-3f,0));
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x+2,point.y+1f,0));
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x-2,point.y+4f,0));
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x-2,point.y-5f,0));
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x+2,point.y-5f,0));
        //空中怪生成
        CenterPool.instance.GetFormPool(flytype,m1,new Vector3(point.x-2,point.y-0.5f,0));
        CenterPool.instance.GetFormPool(flytype,m1,new Vector3(point.x+2,point.y-1.5f,0));
        CenterPool.instance.GetFormPool(flytype,m1,new Vector3(point.x+1.5f,point.y+4f,0));
        aliveMonster+=8;
    }
    //二型房间生成怪物
    void type1MonsterMake2(Point point)
    {
        int m1=Random.Range(0,2);
        int flytype=CenterPool.instance.types[m1++];
        int m2=Random.Range(2,5);
        int groundtype=CenterPool.instance.types[m2++];
        //地面怪生成
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x-5.5f,point.y+3f,0));
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x+5.5f,point.y+3f,0));
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x+4.5f,point.y-5f,0));
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x-4.5f,point.y-5f,0));
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x,point.y-2f,0));
        //空中怪生成
        CenterPool.instance.GetFormPool(flytype,m1,new Vector3(point.x-4.5f,point.y-1.5f,0));
        CenterPool.instance.GetFormPool(flytype,m1,new Vector3(point.x+4.5f,point.y-1.5f,0));
        aliveMonster+=7;

    }
    //三型房间生成怪物
    void type1MonsterMake3(Point point)
    {
        int m1=Random.Range(0,2);
        int flytype=CenterPool.instance.types[m1++];
        int m2=Random.Range(2,5);
        int groundtype=CenterPool.instance.types[m2++];
        //地面怪生成
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x-4.5f,point.y-2f,0));
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x+4.5f,point.y-2f,0));
        CenterPool.instance.GetFormPool(groundtype,m2,new Vector3(point.x,point.y,0));
        //空中怪生成
        CenterPool.instance.GetFormPool(flytype,m1,new Vector3(point.x-5f,point.y+1f,0));
        CenterPool.instance.GetFormPool(flytype,m1,new Vector3(point.x+5f,point.y+1f,0));
        aliveMonster+=5;
    }
    //一型房间宝箱生成
    void chestMake1(Point point)
    {
        int key=Random.Range(1,4);
        switch (key)
        {
            case 1:
                DropManager.instance.chestInit(new Vector2(point.x-1.5f,point.y+3.2f));
            break;
            case 2:
                DropManager.instance.chestInit(new Vector2(point.x+2f,point.y+0.2f));
            break;
            case 3:
                DropManager.instance.chestInit(new Vector2(point.x-3f,point.y-5.8f));
            break;
        }
    }
    //二型房间宝箱生成
    void chestMake2(Point point)
    {
        int key=Random.Range(1,6);
        switch (key)
        {
            case 1:
                DropManager.instance.chestInit(new Vector2(point.x-5.5f,point.y+2.5f));
            break;
            case 2:
                DropManager.instance.chestInit(new Vector2(point.x+5.5f,point.y+2.5f));
            break;
            case 3:
                DropManager.instance.chestInit(new Vector2(point.x,point.y-2.5f));
            break;
            case 4:
                DropManager.instance.chestInit(new Vector2(point.x-6.5f,point.y-5.5f));
            break;
            case 5:
                DropManager.instance.chestInit(new Vector2(point.x+6.5f,point.y-5.5f));
            break;
        }
    }
    //三型房间宝箱生成
    void chestMake3(Point point)
    {
        int key=Random.Range(1,4);
        switch (key)
        {
            case 1:
                DropManager.instance.chestInit(new Vector2(point.x,point.y-0.5f));
            break;
            case 2:
                DropManager.instance.chestInit(new Vector2(point.x+3.5f,point.y-2.5f));
            break;
            case 3:
                DropManager.instance.chestInit(new Vector2(point.x-3.5f,point.y-2.5f));
            break;
        }
    }
    void monsterDie(BaseEvent evt)
    {
        aliveMonster--;
    }
    //房间上锁
    void roomLocked()
    {
        isInRoom=true;
        lockWall[0].transform.position=new Vector2(loads[roadEnter].transform.position.x,loads[roadEnter].transform.position.y);
        lockWall[1].transform.position=new Vector2(loads[roadEnter+1].transform.position.x,loads[roadEnter+1].transform.position.y);
        lockWall[0].SetActive(true);
        lockWall[1].SetActive(true);
        lockWall[0].GetComponent<Animator>().SetTrigger("apear");
        lockWall[1].GetComponent<Animator>().SetTrigger("apear");
        Invoke("WallAnimPlay",1.5f);
    }
    void WallAnimPlay()
    {
        lockWall[0].GetComponent<Animator>().SetTrigger("stay");
        lockWall[1].GetComponent<Animator>().SetTrigger("stay");
    }
    //房间解锁
    void roomUnLocked()
    {
        isInRoom=false;
        lockWall[0].SetActive(false);
        lockWall[1].SetActive(false);
    }
}
