using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsPool : MonoBehaviour
{
    public static BulletsPool instance;

    public GameObject enBullet;
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject bullet3;
    public GameObject bullet4;
    public GameObject bullet5;
    public GameObject bullet6;
    public GameObject bullet7;
    public GameObject bullet8;
    public int bulletsCount;
    public int b_type=1;
    

    public Queue<GameObject> bullets = new Queue<GameObject>();      //空闲飞行怪物队列1
    public Queue<GameObject> enBullets = new Queue<GameObject>();      //空闲飞行怪物队列1
    void Awake()
    {
        instance = this;

        //初始化对象池
        FillPool(1);
        enemyBulletFillPool();
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
                case 3:
                    newBullet=Instantiate(bullet3);
                break;
                case 4:
                    newBullet=Instantiate(bullet4);
                break;
                case 5:
                    newBullet=Instantiate(bullet5);
                break;
                case 6:
                    newBullet=Instantiate(bullet6);
                break;
                case 7:
                    newBullet=Instantiate(bullet7);
                break;
                case 8:
                    newBullet=Instantiate(bullet8);
                break;
            }
            //取消启用,返回对象池
            //newBullet.transform.SetParent(GameObject.FindGameObjectWithTag("pool").transform);
            ReturnPool(newBullet);
        }
    }
    public void ReturnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        //Debug.Log(bullets.Count);
        bullets.Enqueue(gameObject);
    }

    public GameObject GetFormPool(Vector2 pos,Transform trans)
    {
        if (bullets.Count == 0)
        {
            FillPool(b_type);
        }
        GameObject outBullet = bullets.Dequeue();
        outBullet.SetActive(true);

        outBullet.GetComponent<Bullet>().b_Check=trans.position;
        outBullet.transform.localScale = new Vector3(trans.localScale.x, 1f, 1);
        outBullet.transform.position=trans.transform.position;
        outBullet.GetComponent<Rigidbody2D>().velocity=pos;
        return outBullet;
    }
    public void switchBullet(int type)
    {
        //查找bullet-tag精灵删除
        
        bullets.Clear();
        b_type=type;
        FillPool(type);
    }
    ///////////////////////////////////////////////////////////怪物子弹操作////////////////////////////////////////////////////////
    public void enemyBulletFillPool()
    {
        for (int i = 0; i < bulletsCount; i++)
        {
            var newBullet=Instantiate(enBullet);
            ReturnEnPool(newBullet);
        }
    }
    public void ReturnEnPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        
        enBullets.Enqueue(gameObject);
    }
    public GameObject GetEnBulletFormPool(Vector2 pos,Transform trans)
    {
        if (enBullets.Count == 0)
        {
            enemyBulletFillPool();
        }
        GameObject outBullet = enBullets.Dequeue();
        outBullet.SetActive(true);

        outBullet.GetComponent<EnemyBullet>().b_Check=trans.position;

        outBullet.transform.position=trans.transform.position;
        outBullet.GetComponent<Rigidbody2D>().velocity=pos;
        return outBullet;
    }
}