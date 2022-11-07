using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeBomb : MonoBehaviour
{
    public GameObject log;
    public GameObject thing;
    private bool canOpen;
    private Animator anim;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(canOpen)
            {
                canOpen=false;
                EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.BOMB_GET,null));
                BombPool.instance.ReturnPool(thing);
            }
        }
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
