using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    // Start is called before the first frame update
    Animator m_Animator;
    bool isGet=true;

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
            EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.GOLD_GET,null));
            m_Animator.SetTrigger("dis");
            //Invoke("returnPool",0.5f);
        }
    }

    void returnPool()
    {
         gameObject.GetComponent<Rigidbody2D>().gravityScale=1f;

        gameObject.GetComponent<Collider2D>().isTrigger=false;

        isGet=true;
        
        GoldPool.instance.ReturnPool(gameObject);
    }
}
