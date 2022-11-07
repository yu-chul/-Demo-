using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPool : MonoBehaviour
{
    public static GoldPool instance;  //单例创建
    public GameObject gold;
    public int goldCount;
    public Queue<GameObject> golds = new Queue<GameObject>();      
    void Start()
    {
        instance = this;

        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < goldCount; i++)
        {
            var newGold = Instantiate(gold);
            //取消启用,返回对象池
            newGold.transform.SetParent(transform);
            ReturnPool(newGold);
        }
    }
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        
        golds.Enqueue(gameObject);
    }

    public GameObject GetFormPool(Vector2 pos)
    {
        if (golds.Count == 0)
        {
            FillPool();
        }
        var outGold = golds.Dequeue();

        outGold.transform.position=pos;

        //outGold.GetComponent<Rigidbody2D>().velocity=new Vector2(15f,15f);

        outGold.SetActive(true);

        return outGold;
    }
}
