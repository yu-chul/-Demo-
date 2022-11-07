using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullflyEnemy : MonoBehaviour
{
    public float speed;
    public float patrolRadius;  //巡逻半径
    public float searchRadius;  //搜索半径
    public float loseRadius;    //丢失玩家时半径
    public float attackRadius;  //攻击时半径
    public LayerMask searchLayer;   //检索图层
    public float attackInterval;    //攻击间隔时间
    public Damger attackDamage;     //伤害计算组件
    public Damageable attackDamageable;    //可受伤组件
    public int type=0;              //怪物类型

    bool isAttack;
    bool m_die;
    Transform player;    //玩家位置
    float m_AttackTimer;

    Animator m_Animator;
    Rigidbody2D m_Rigidbody2D;
    SpriteRenderer m_SpriteRenderer;
    Vector2 m_VectorMove;
    Vector2 m_PatrolStart;
    Vector2 m_PatrolEnd;
    float m_PatrolDuration;
    
    float shootTimer; 

    public float shootRange;        //射距
    public float shootDamgeLite;    //射击伤害系数
    public float shootInterval;        //射击时间间隔
    Color originalColor;
     private void OnEnable()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        stateSet();
        m_Animator=GetComponent<Animator>();
        m_Rigidbody2D=GetComponent<Rigidbody2D>();
        m_SpriteRenderer=GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();

        m_PatrolStart=transform.position+Vector3.left * patrolRadius;
        m_PatrolEnd=transform.position+Vector3.right * patrolRadius;
        m_PatrolDuration=(m_PatrolEnd-m_PatrolStart).magnitude/speed;

        originalColor=m_SpriteRenderer.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_die)
        {
            m_Animator.ResetTrigger("attack");
            m_Animator.ResetTrigger("fly");
            m_Animator.SetTrigger("die");
        }
        if(player)
        {
            Track();
        }
        else
        {
            Patrol();
            Search();
        }
        UpdateFacing();
        m_VectorMove.y=m_Rigidbody2D.velocity.y;
        m_Rigidbody2D.velocity=m_VectorMove;
        m_VectorMove.x=0f;
        m_AttackTimer+=Time.deltaTime;
        shootTimer+=Time.deltaTime;
    }
    //巡逻函数
    void Patrol()
    {
        float t=Mathf.InverseLerp(0,m_PatrolDuration,Mathf.PingPong(Time.time,m_PatrolDuration));
        Vector2 position=Vector2.Lerp(m_PatrolStart,m_PatrolEnd,t);
        m_VectorMove.x=(position-(Vector2)transform.position).normalized.x*speed;
        m_Animator.SetFloat("running", Mathf.Abs(m_Rigidbody2D.velocity.x));
    }
    //搜索函数
    void Search()
    {
        Collider2D collider=Physics2D.OverlapCircle(transform.position,searchRadius,searchLayer);
        if(collider)
            player=collider.transform;
    }
    //检查函数
    void Track()
    {
        float sqrDistance=(transform.position-player.position).sqrMagnitude;
        
        if(sqrDistance<=attackRadius * attackRadius)
        {
            isAttack=true;
            Attack();
        }
        else if(sqrDistance<=loseRadius * loseRadius)
        {
            isAttack=true;
            
            if(shootTimer>=shootInterval)
            {
                shootTimer=0f;
                m_Animator.SetTrigger("attack");
                if(player.position.x - transform.position.x>0)
                    BulletsPool.instance.GetEnBulletFormPool(new Vector2(3f,player.position.y - transform.position.y),gameObject.transform);
                else 
                    BulletsPool.instance.GetEnBulletFormPool(new Vector2(-3f,player.position.y - transform.position.y),gameObject.transform);    
            }
        }
        else
        {
            m_Animator.ResetTrigger("attack");
            m_Animator.SetTrigger("fly");
            player=null;
            isAttack=false;
        }
    }
    //攻击函数
    void Attack()
    {
        if(m_AttackTimer>=attackInterval)
        {
            m_AttackTimer=0f;
            attackDamage.TriggerDamageOnce();
        }
    }
    //改变朝向
    void UpdateFacing()
    {
        if(isAttack && player.transform.position.x-transform.localScale.x>0 )
        {
            transform.localScale = new Vector3(1f , 1f, 1);
        }
        else if(isAttack && player.transform.position.x-transform.localScale.x<0&& System.Math.Abs(player.transform.position.x-transform.localScale.x)>1f)
        {
            transform.localScale = new Vector3(-1f , 1f, 1);
        }
        else if(m_VectorMove.x > 0.01 && isAttack==false)
        {
            transform.localScale = new Vector3(1f , 1f, 1);
            //attackDamage.h=1;
        }
        else if(m_VectorMove.x <= 0.01 && isAttack==false)
        {
            transform.localScale = new Vector3(-1f , 1f, 1);
            //attackDamage.h=-1;
        }
    }
    //范围绘制
    void OnDrawGizmos()
    {
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRadius);
        Gizmos.color=Color.yellow;
        Gizmos.DrawWireSphere(transform.position,searchRadius);
        Gizmos.color=Color.green;
        Gizmos.DrawWireSphere(transform.position,loseRadius);
    }
    public void OnDie()
    {
        m_die=true;
        //死亡动画
    }

    public void OnTakeDamage()
    {
        //受伤动画
        FlashColor(0.1f);
    } 

    void returnPool()
    {
        stateSet();
        //掉落
        int randKey=Random.Range(0,2);
        if(randKey!=0)
        {
            GoldPool.instance.GetFormPool(transform.position);
        }
        
        EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.MONSTER_DIE,null));
        bool j=true;
        for(int i=0;i<CenterPool.instance.types.Count;i++)
        {
            if(type==CenterPool.instance.types[i])
            {
                j=false;
                CenterPool.instance.ReturnPool(gameObject,i+1);
                break;
            }
        }
        if(j)
        {
            Debug.Log("死亡");
            Destroy(gameObject);
        }   
    }
    //重置生命值等
    void stateSet()
    {
        attackDamageable.currentHealth=attackDamageable.maxHealth;
        m_die=false;
    }
    void FlashColor(float time)
    {
        m_SpriteRenderer.color = Color.red;
        Invoke("ResetColor", time);
    }

    void ResetColor()
    {
        m_SpriteRenderer.color = originalColor;
    }
}