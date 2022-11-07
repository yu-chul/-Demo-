using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPool : MonoBehaviour
{
    public static BombPool instance;  //单例创建
    public GameObject bomb;
    public int bombCount;
    public Queue<GameObject> bombs = new Queue<GameObject>();      
    void Start()
    {
        instance = this;

        FillPool();
    }

    public void FillPool()
    {
        for (int i = 0; i < bombCount; i++)
        {
            var newBomb = Instantiate(bomb);
            //取消启用,返回对象池
            newBomb.transform.SetParent(transform);
            ReturnPool(newBomb);
        }
    }
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        
        bombs.Enqueue(gameObject);
    }

    public GameObject GetFormPool(Vector2 pos)
    {
        if (bombs.Count == 0)
        {
            FillPool();
        }
        var outBomb = bombs.Dequeue();

        outBomb.transform.position=pos;

        //outGold.GetComponent<Rigidbody2D>().velocity=new Vector2(15f,15f);

        outBomb.SetActive(true);

        return outBomb;
    }
}
