using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject log;
    public GameObject thing;
    public int type;
    bool canTake;
    void Start()
    {
        type=Random.Range(1,9);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(canTake)
            {
                int count = GameObject.Find("Gun").transform.childCount;
                if(count!=0)
                {
                    GameObject grandChild=GameObject.Find("Gun").transform.GetChild(0).gameObject;
                    grandChild.transform.SetParent(GameObject.Find("UIManager").transform);
                    grandChild.transform.localScale = new Vector3(1f, 1f, 1);
                    //添加刚体
                    Rigidbody2D shoot = grandChild.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
                    shoot.constraints = RigidbodyConstraints2D.FreezeRotation;      //锁定旋转Z
                    //添加碰撞箱
                    BoxCollider2D col = grandChild.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                    col.size=new Vector2(0.5f,0.25f);
                    

                    GameObject schild=grandChild.transform.GetChild(1).gameObject;
                    schild.SetActive(true);

                    thing.transform.SetParent(GameObject.Find("Gun").transform);
                    thing.transform.position=new Vector2(GameObject.Find("Player").transform.position.x+0.05f,GameObject.Find("Player").transform.position.y-0.3f);
                    thing.transform.localScale = new Vector3(1f, 1f, 1);
                    log.SetActive(false);
                    Destroy(thing.GetComponent<Rigidbody2D>());   
                    Destroy(thing.GetComponent<Collider2D>());   
                    gameObject.SetActive(false);

                    Player.instance.switchBullet(type);
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag=="Player" )
        {
            canTake = true;
            log.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag=="Player")
        {
            canTake = false;
            log.SetActive(false);
        }
    }
}
