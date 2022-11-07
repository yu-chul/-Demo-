using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BooldTreasure : MonoBehaviour
{
    public GameObject log;
    public GameObject thing;
    private bool canTake;
    public int type;
    void Start()
    {
        canTake = false;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(canTake)
            {
                switch (type)
                {
                    case 1:
                        EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.HOLEHEART_GET,null));
                        Player.instance.item.Add(type);
                    break;
                    case 2:
                        EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.HOLEHEART_GET,null));
                        EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.SOUL_GET,null));
                        Player.instance.item.Add(type);
                    break;
                    case 3:
                        EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.HOLEHEART_GET,null));
                        EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.HOLEHEART_GET,null));
                        Player.instance.item.Add(type);
                    break;
                }
                Destroy(thing);
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
