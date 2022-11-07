using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public static Treasure instance;
    int type;           //宝物类型0为枪，1位buff类,2加血，3加钱，4加数值
    
    void Start()
    {
        instance = this;
    }
    public GameObject InitInRand(Vector2 pos)
    {
        int randKey=Random.Range(0,10);
        if(randKey==5)
            return gunTreasureInit(pos);
        else
        {   
            return buffTreasureInit(pos);
        }
    }

    // Update is called once per frame
    public GameObject gunTreasureInit(Vector2 pos)
    {
        var treasure=(GameObject)null;
        treasure = (GameObject)Resources.Load("gun");
        treasure = Instantiate(treasure, pos, Quaternion.identity, GameObject.Find("UIManager").transform);
        int randKey=Random.Range(1,10);
        treasure.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("g"+randKey.ToString());

        return treasure;
    }
    public GameObject buffTreasureInit(Vector2 pos)
    {
        int key=Random.Range(0,10);
        switch (key)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return bloodTreasureInit(pos);
            break;
            case 4:
            case 5:
                return moneyTreasureInit(pos);
            break;
            case 6:
            case 7:
            case 8:
            case 9:
                return valueTreasureInit(pos);
            break;
            default:
            return null;
        }
    }
    //加血
    public GameObject bloodTreasureInit(Vector2 pos)
    {
        int randKey=Random.Range(0,10);
        var treasure=(GameObject)null;
        switch (randKey)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                treasure = (GameObject)Resources.Load("1");
                treasure = Instantiate(treasure, pos, Quaternion.identity, GameObject.Find("UIManager").transform);
            break;
            case 5:
            case 6:
            case 7:
                treasure = (GameObject)Resources.Load("2");
                treasure = Instantiate(treasure, pos, Quaternion.identity, GameObject.Find("UIManager").transform);
            break;
            case 8:
            case 9:
                treasure = (GameObject)Resources.Load("3");
                treasure = Instantiate(treasure, pos, Quaternion.identity, GameObject.Find("UIManager").transform);
            break;
        }
        return treasure;
    }
    //加钱
    public GameObject moneyTreasureInit(Vector2 pos)
    {
        int randKey=Random.Range(0,10);
        var treasure=(GameObject)null;
        if(randKey==8)
        {
            treasure = (GameObject)Resources.Load("5");
            treasure = Instantiate(treasure, pos, Quaternion.identity, GameObject.Find("UIManager").transform);
        }
        else
        {
            treasure = (GameObject)Resources.Load("4");
            treasure = Instantiate(treasure, pos, Quaternion.identity, GameObject.Find("UIManager").transform);
        }
        return treasure;
    }
    //加数值
    public GameObject valueTreasureInit(Vector2 pos)
    {
        var treasure=(GameObject)null;
        treasure = (GameObject)Resources.Load("BuffTreasure");
        treasure = Instantiate(treasure, pos, Quaternion.identity, GameObject.Find("UIManager").transform);
        int randKey=Random.Range(6,16);
        treasure.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(randKey.ToString());
        
        Player.instance.item.Add(randKey);

        return treasure;
    }
}
