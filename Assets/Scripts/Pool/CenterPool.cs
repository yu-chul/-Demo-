using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPool : MonoBehaviour
{
    public static CenterPool instance;  //单例创建
    [Header("怪物列表")]
    public GameObject shadowPrefab;
    public GameObject bat;
    public GameObject octopus;
    public GameObject ghost;
    public GameObject skullfly;
    public GameObject crab;
    public GameObject frog;
    public GameObject slime;
    public GameObject thing;
    public GameObject burning;
    [SerializeField] private Queue<GameObject> availableflyMonsters1 = new Queue<GameObject>();      //空闲飞行怪物队列1
    [SerializeField] private Queue<GameObject> availableflyMonsters2 = new Queue<GameObject>();      //空闲飞行怪物队列2
    [SerializeField] private Queue<GameObject> availableMonsters1 = new Queue<GameObject>();      //空闲地上怪物队列1
    [SerializeField] private Queue<GameObject> availableMonsters2 = new Queue<GameObject>();      //空闲地上怪物队列2
    [SerializeField] private Queue<GameObject> availableMonsters3 = new Queue<GameObject>();      //空闲地上怪物队列3


    [Header("基础信息")]
    public int Count;       //怪物数量
    public int shadowCount; //残影数量

    private int bullettype;  
   public List<int> types=new List<int>();      //地上怪物类型
    void Awake()
    {
        instance = this;

        List<int> list = new List<int>(){1,2,3,4}; 
        int n=list.Count;
        int s;
        for(int i=0;i<5;i++)
        {   
            if(i==2)
            {
                list=new List<int>(){5,6,7,8,9}; 
                n=list.Count;
            }
            s=list[Random.Range(0,n--)];
            types.Add(s);
            FillMonsterPool(s,i+1);
            list.Remove(s);
        }
    }

    public void FillMonsterPool(int n,int m)
    {
        var flyMonster=(GameObject)null;
        for (int i = 0; i < Count; i++)
        {
            switch (n)
            {
                case 1:
                    flyMonster=Instantiate(bat);
                break;
                case 2:
                    flyMonster=Instantiate(octopus);
                break;
                case 3:
                    flyMonster=Instantiate(ghost);
                break;
                case 4:
                    flyMonster=Instantiate(skullfly);
                break;
                case 5:
                    flyMonster=Instantiate(crab);
                break;
                case 6:
                    flyMonster=Instantiate(frog);
                break;
                case 7:
                    flyMonster=Instantiate(slime);
                break;
                case 8:
                    flyMonster=Instantiate(thing);
                break;
                case 9:
                    flyMonster=Instantiate(burning);
                break;
            }
            flyMonster.transform.SetParent(transform);

            //取消启用,返回对象池
            ReturnPool(flyMonster,m);
        }
    }

    public void ReturnPool(GameObject gameObject,int m)
    {
        gameObject.SetActive(false);

        switch (m)
        {
            case 1:
                availableflyMonsters1.Enqueue(gameObject);
            break;
            case 2:
                availableflyMonsters2.Enqueue(gameObject);
            break;
            case 3:
                availableMonsters1.Enqueue(gameObject);
            break;
            case 4:
                availableMonsters2.Enqueue(gameObject);
            break;
            case 5:
                availableMonsters3.Enqueue(gameObject);
            break;
        }
    }
    
    public GameObject GetFormPool(int n,int m,Vector3 pos)
    {   //n为怪物类型，m为几号队列
        var outMonster=(GameObject)null;
        switch (m)
        {
            case 1:
                if (availableflyMonsters1.Count == 0)
                    FillMonsterPool(n,m);
                outMonster=availableflyMonsters1.Dequeue();
            break;
            case 2:
                if (availableflyMonsters2.Count == 0)
                    FillMonsterPool(n,m);
                outMonster=availableflyMonsters2.Dequeue();
            break;
            case 3:
                if (availableMonsters1.Count == 0)
                    FillMonsterPool(n,m);
                outMonster=availableMonsters1.Dequeue();
            break;
            case 4:
                if (availableMonsters2.Count == 0)
                    FillMonsterPool(n,m);
                outMonster=availableMonsters2.Dequeue();
            break;
            case 5:
                if (availableMonsters3.Count == 0)
                    FillMonsterPool(n,m);
                outMonster=availableMonsters3.Dequeue();
            break;
            default:
                Debug.Log("m is wrong");
                break;
        }
        outMonster.transform.position = pos;
        outMonster.SetActive(true);
    
        return outMonster;
    }
    
}