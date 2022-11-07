using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComOne : MonoBehaviour
{
    public GameObject thing;
    public GameObject log;
    public int type;
    int seal;
    GameObject check;
    GameObject com;
    bool canTake;
    Animator m_Animator;
    void Start()
    {
        m_Animator=thing.GetComponent<Animator>();
        switch (type)
        {
            case 1:
                seal=15;
                com=Treasure.instance.gunTreasureInit(new Vector2(thing.transform.position.x,thing.transform.position.y+1f));
                check=com.transform.GetChild(1).gameObject;
                check.SetActive(false);
            break;
            case 2:
                seal=10;
                com=Treasure.instance.buffTreasureInit(new Vector2(thing.transform.position.x,thing.transform.position.y+1f));
                check=com.transform.GetChild(1).gameObject;
                check.SetActive(false);
            break;
            case 3:
                seal=5;
                com=HeartPool.instance.GetFormPool(new Vector2(thing.transform.position.x,thing.transform.position.y+0.7f),1);
                com.GetComponent<Collider2D>().isTrigger = true;    
                com.GetComponent<Rigidbody2D>().gravityScale=0f;  
            break;
            case 4:
                seal=3;
                com=BombPool.instance.GetFormPool(new Vector2(thing.transform.position.x,thing.transform.position.y+1f));
                check=com.transform.GetChild(1).gameObject;
                check.SetActive(false);
            break;
            case 5:
                seal=3;
                com=HeartPool.instance.GetFormPool(new Vector2(thing.transform.position.x,thing.transform.position.y+0.7f),3);
                com.GetComponent<Collider2D>().isTrigger = true;
                com.GetComponent<Rigidbody2D>().gravityScale=0f;
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(canTake)
            {
                if(Player.instance.gold>=seal)
                {
                    Player.instance.gold-=seal;
                    if(type!=3 && type!=5)
                        check.SetActive(true);
                    else
                    {
                        com.GetComponent<Collider2D>().isTrigger = false;
                        com.GetComponent<Rigidbody2D>().gravityScale=1f;
                    }
                    m_Animator.SetTrigger("dis");
                    Invoke("re",0.5f);
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
    void re()
    {
         Destroy(thing);
    }
}
