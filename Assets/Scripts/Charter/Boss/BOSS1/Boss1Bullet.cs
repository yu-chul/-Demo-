using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Bullet : MonoBehaviour
{
    public Damger b_attackDamage;     //伤害计算组件
    public Vector2 b_Check;   //发射点位置
    public int type;
    bool isHappen=true;
    bool isAttack=true;

    Rigidbody2D m_Rigidbody2D;
    Animator m_Animator;

    void Start()
    {
        m_Animator=GetComponent<Animator>();
        m_Rigidbody2D=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!b_attackDamage.isBurning)
            b_attackDamage.TriggerDamageOnce();
    }
    void FixedUpdate()
    {
        if(b_attackDamage.isBurning && isAttack)
        {
            isAttack=false;
            m_Rigidbody2D.velocity=new Vector2(0f,0f);
            m_Animator.SetTrigger("dis");
        }
        if(System.Math.Abs(transform.position.x-b_Check.x)>6f && isHappen)
        {
            isHappen=false;
            m_Rigidbody2D.velocity=new Vector2(0f,0f);
            m_Animator.SetTrigger("dis");
        }
    }

    void returnPool()
    {
        isHappen=true;
        isAttack=true;
        b_attackDamage.isBurning=false;
        gameObject.transform.position=GameObject.Find(gameObject.name).transform.position;
        bossBulletPool.instance.ReturnPool(gameObject,type);
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
}
