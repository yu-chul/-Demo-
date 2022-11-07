using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTreasure : MonoBehaviour
{
    public GameObject log;
    public GameObject thing;
    private bool canTake;
    public int type;
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
                switch (type)
                {
                    case 4:
                        EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.GOLDS_GET,null));
                        Player.instance.item.Add(type);
                    break;
                    case 5:
                        EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.HOLEGOLDS_GET,null));
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
