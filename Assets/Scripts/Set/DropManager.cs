using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    public static DropManager instance;  //单例创建

    public GameObject chest;            //宝箱
    //public GameObject treasure;         //宝具
    void Start()
    {
        instance = this;
    }

    public void chestInit(Vector2 pos)
    {
        Instantiate(chest, pos, Quaternion.identity);
    }
    public void treasureInit(Vector2 pos)
    {

    }

}
