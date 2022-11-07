using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBulletPool : MonoBehaviour
{
    public static bossBulletPool instance;

    public GameObject bullet1;
    public GameObject bullet2;

    public int bulletsCount;

    public Queue<GameObject> bullets1 = new Queue<GameObject>();      //boss子弹对象池1
    public Queue<GameObject> bullets2 = new Queue<GameObject>();      //boss子弹对象池2
    void Start()
    {
        instance = this;

        FillPool(1);
        FillPool(2);
    }

    public void FillPool(int n)
    {
        var newBullet=(GameObject)null;
        for (int i = 0; i < bulletsCount; i++)
        {
            switch (n)
            {
                case 1:
                    newBullet=Instantiate(bullet1);
                break;
                case 2:
                    newBullet=Instantiate(bullet2);
                break;
            }
            ReturnPool(newBullet,n);
        }
    }
    public void ReturnPool(GameObject gameObject,int n)
    {
        gameObject.SetActive(false);
        
        switch (n)
        {
            case 1:
                bullets1.Enqueue(gameObject);
            break;
            case 2:
                bullets2.Enqueue(gameObject);
            break;
        }
    }

    public GameObject GetFormPool(Vector2 pos,Vector2 trans,int n)
    {
        if (bullets1.Count == 0 || bullets2.Count == 0)
        {
            FillPool(n);
        }
        GameObject outBullet=(GameObject)null;

        switch (n)
        {
            case 1:
                outBullet = bullets1.Dequeue();
            break;
            case 2:
                outBullet = bullets2.Dequeue();
            break;
        }

        outBullet.SetActive(true);
        //定位
        outBullet.GetComponent<Boss1Bullet>().b_Check=trans;
        //位置变换
        outBullet.transform.position=trans;
        //加速
        outBullet.GetComponent<Rigidbody2D>().velocity=pos;

        return outBullet;
    }
}
