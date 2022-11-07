using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject log;
    public GameObject thing;
    private bool canOpen;
    private bool isOpened;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = thing.GetComponent<Animator>();
        isOpened = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(canOpen && !isOpened)
            {
                anim.SetTrigger("open");
                isOpened = true;
                Invoke("GenCoin",0.3f);
                Invoke("Dis",2f);
            }
        }
    }

    void GenCoin()
    {
        int randKey=Random.Range(1,5);
        for(int i=0;i<randKey;i++)
        {
            GoldPool.instance.GetFormPool(transform.position).GetComponent<Rigidbody2D>().velocity = new Vector2(-2f,(float)(i+1));
        }
        randKey=Random.Range(0,3);
        if(randKey==1)
        {
            randKey=Random.Range(0,4);
            if(randKey==0)
                HeartPool.instance.GetFormPool(transform.position,1).GetComponent<Rigidbody2D>().velocity = new Vector2(-2f,3f);
            else
                HeartPool.instance.GetFormPool(transform.position,2).GetComponent<Rigidbody2D>().velocity = new Vector2(-2f,3f);
        }
        randKey=Random.Range(0,3);
        if(randKey==1)
        {
            randKey=Random.Range(0,4);
            if(randKey==0)
                HeartPool.instance.GetFormPool(transform.position,3).GetComponent<Rigidbody2D>().velocity = new Vector2(-2f,3f);
            else
                HeartPool.instance.GetFormPool(transform.position,4).GetComponent<Rigidbody2D>().velocity = new Vector2(-2f,3f);
        }
        randKey=Random.Range(0,15);
        if(randKey==7 || randKey==10)
        {
            //生成宝具
            Treasure.instance.InitInRand(gameObject.transform.position).GetComponent<Rigidbody2D>().velocity = new Vector2(-2f,2f);
        }        
        randKey=Random.Range(0,10);
        if(randKey<=4)
        {
            //生成炸弹
            BombPool.instance.GetFormPool(gameObject.transform.position).GetComponent<Rigidbody2D>().velocity = new Vector2(-2f,2f);
        }
        else if(randKey>=8)
        {
            BombPool.instance.GetFormPool(gameObject.transform.position).GetComponent<Rigidbody2D>().velocity = new Vector2(-2f,2f);
            BombPool.instance.GetFormPool(gameObject.transform.position).GetComponent<Rigidbody2D>().velocity = new Vector2(-2f,2f);
        }
    }

    void Dis()
    {
        anim.SetTrigger("dis");
        Invoke("des",0.5f);
    }
    void des()
    {
        Destroy(thing);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag=="Player" )
        {
            canOpen = true;
            log.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag=="Player")
        {
            canOpen = false;
            log.SetActive(false);
        }
    }
}