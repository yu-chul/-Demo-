using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPool : MonoBehaviour
{
    public static HeartPool instance;  //单例创建
    public GameObject heartFull;
    public GameObject heartHalf;
    public GameObject soulFull;
    public GameObject soulHalf;
    public int heartsCount;
    public Queue<GameObject> h_Full = new Queue<GameObject>(); 
    public Queue<GameObject> h_Half = new Queue<GameObject>(); 
    public Queue<GameObject> s_Full = new Queue<GameObject>(); 
    public Queue<GameObject> s_Half = new Queue<GameObject>(); 

    void Start()
    {
        instance = this;

        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < heartsCount; i++)
        {
            var newHeart = Instantiate(heartFull);
            //取消启用,返回对象池
            newHeart.transform.SetParent(transform);
            ReturnPool(newHeart,1);
        }
        for (int i = 0; i < heartsCount; i++)
        {
            var newHeart = Instantiate(heartHalf);
            //取消启用,返回对象池
            newHeart.transform.SetParent(transform);
            ReturnPool(newHeart,2);
        }
        for (int i = 0; i < heartsCount; i++)
        {
            var newHeart = Instantiate(soulFull);
            //取消启用,返回对象池
            newHeart.transform.SetParent(transform);
            ReturnPool(newHeart,3);
        }
        for (int i = 0; i < heartsCount; i++)
        {
            var newHeart = Instantiate(soulHalf);
            //取消启用,返回对象池
            newHeart.transform.SetParent(transform);
            ReturnPool(newHeart,4);
        }
    }
    public void ReturnPool(GameObject gameObject,int i)
    {
        switch (i)
        {
            case 1:
                gameObject.SetActive(false);
                
                h_Full.Enqueue(gameObject);
            break;
            case 2:
                gameObject.SetActive(false);
                
                h_Half.Enqueue(gameObject);
            break;
            case 3:
                gameObject.SetActive(false);
                
                s_Full.Enqueue(gameObject);
            break;
            case 4:
                gameObject.SetActive(false);
                
                s_Half.Enqueue(gameObject);
            break;
        }
    }

    public GameObject GetFormPool(Vector2 pos,int i)
    {
        var outHeart=(GameObject)null;
        switch (i)
        {
            case 1:
                if (h_Full.Count == 0)
                    FillPool();
                outHeart=h_Full.Dequeue();
            break;
            case 2:
                if (h_Half.Count == 0)
                    FillPool();
                outHeart=h_Half.Dequeue();
            break;
            case 3:
                if (s_Full.Count == 0)
                    FillPool();
                outHeart=s_Full.Dequeue();
            break;
            case 4:
                if (s_Half.Count == 0)
                    FillPool();
                outHeart=s_Half.Dequeue();
            break;
            default:
                Debug.Log("i is wrong");
            break;
        }

        outHeart.transform.position=pos;

        //outGold.GetComponent<Rigidbody2D>().velocity=new Vector2(15f,15f);

        outHeart.SetActive(true);

        return outHeart;
    }
}
