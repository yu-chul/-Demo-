using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTreasure : MonoBehaviour
{
    public GameObject log;
    public GameObject thing;
    private bool canTake;
    void Start()
    {
        canTake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(canTake)
            {
                TakeBuff();
                Destroy(thing);
            }
        }
    }
    void TakeBuff()
    {
        int randKey=Random.Range(0,12);
        switch (randKey)
        {
            case 0:
            case 1:
                Player.instance.shootDamgeLite+=0.2f;
                Player.instance.da++;
            break;
            case 2:
            case 3:
            case 4:
                Player.instance.shootingSpeed*=0.8f;
                Player.instance.shp++;
            break;
            case 5:
            case 6:
            case 7:
                Player.instance.shootRange+=1f;
                Player.instance.ra++;
            break;
            case 8:
            case 9:
            case 10:
                Player.instance.speed+=0.5f;
                Player.instance.sp++;
            break;
            case 11:
                Player.instance.attackDamger.damage+=0.3f;
                Player.instance.da++;
            break;
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
