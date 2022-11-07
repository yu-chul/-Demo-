using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Damger b_attackDamage;     //伤害计算组件
    public Vector2 b_Check;   //发射点位置
    bool isHappen=true;
    bool isAttack=true;
    bool isShooting=true;
    float m_lastPos;
    Rigidbody2D m_Rigidbody2D;
    Animator m_Animator;
    
    void Start()
    {
        m_Animator=GetComponent<Animator>();
        m_Rigidbody2D=GetComponent<Rigidbody2D>();
        b_Check=GameObject.FindGameObjectWithTag("flymonster").transform.position;
        gameObject.transform.position=b_Check;
    }

    // Update is called once per frame
    void Update()
    {
        if(isShooting)
        {
            isShooting=false;
            Vector2 ro=(GameObject.FindGameObjectWithTag("Player").transform.position-transform.position).normalized;
            float ang=Mathf.Atan2(ro.y, ro.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, ang);
        }
        b_attackDamage.damage*=Player.instance.shootDamgeLite;
        if(!b_attackDamage.isBurning)
            b_attackDamage.TriggerDamageOnce();
    }
    void FixedUpdate()
    {
        UpdateFacing();
        if(b_attackDamage.isBurning && isAttack)
        {
            isAttack=false;
            m_Rigidbody2D.velocity=new Vector2(0f,0f);
            m_Animator.SetTrigger("dis");
        }
        if(System.Math.Abs(transform.position.x-b_Check.x)>Player.instance.shootRange && isHappen)
        {
            isHappen=false;
            m_Rigidbody2D.velocity=new Vector2(0f,0f);
            m_Animator.SetTrigger("dis");
        }
        m_lastPos=transform.position.x;
    }

    void returnPool()
    {
        //Debug.Log("UE");
        isHappen=true;
        isAttack=true;
        isShooting=true;
        b_attackDamage.isBurning=false;
        if(GameObject.FindGameObjectWithTag("flymonster"))
            gameObject.transform.position=GameObject.FindGameObjectWithTag("flymonster").transform.position;
        else
            Destroy(gameObject);
        BulletsPool.instance.ReturnEnPool(gameObject);
        m_Animator.ResetTrigger("dis");
        m_Animator.SetTrigger("fly");
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Wall")
        {
            m_Rigidbody2D.velocity=new Vector2(0f,0f);
            m_Animator.SetTrigger("dis");
        }
    }

    void UpdateFacing()
    {
        if(transform.position.x-m_lastPos>0)
        {
            transform.localScale = new Vector3(1f , 1f, 1);
            //attackDamage.h=1;
        }
        else
        {
            transform.localScale = new Vector3(-1f , 1f, 1);
            //attackDamage.h=-1;
        }
    }
}
