using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    // Start is called before the first frame update
    Animator m_Animator;
    bool isGet=true;
    public int type;

    void Start()
    {
        m_Animator=GetComponent<Animator>();
    }
    void Update()
    {
        if(!isGet)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity=new Vector2(0f,0f);
            gameObject.GetComponent<Rigidbody2D>().gravityScale=0f;
            gameObject.GetComponent<Collider2D>().isTrigger=true;
        }
    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" && isGet)
        {
            isGet=false;
            switch (type)
            {
                case 1:
                    EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.HEALTH_GET,null));
                break;
                case 2:
                    EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.HALFHEALTH_GET,null));
                break;
                case 3:
                    EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.SOUL_GET,null));
                break;
                case 4:
                    EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.HALFSOUL_GET,null));
                break;
            }

            m_Animator.SetTrigger("dis");
            Invoke("returnPool",0.5f);
        }
    }

    void returnPool()
    {
        gameObject.GetComponent<Rigidbody2D>().gravityScale=1f;

        gameObject.GetComponent<Collider2D>().isTrigger=false;

        isGet=true;
        
        HeartPool.instance.ReturnPool(gameObject,type);
    }
}
