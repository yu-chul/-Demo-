using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEnemy : MonoBehaviour
{
    public float speed;
    public float trackSpeed;    //追踪玩家时速度
    public float searchRadius;  //搜索半径
    public float loseRadius;    //丢失玩家时半径
    public float attackRadius;  //攻击时半径
    public LayerMask searchLayer;   //检索图层
    public Damger attackDamage;     //伤害计算组件
    public Damageable attackDamageable;    //可受伤组件
    public int type=0;              //怪物类型
    public int att=0;

    bool m_die;
    Transform player;    //玩家位置

    Animator m_Animator;
    Rigidbody2D m_Rigidbody2D;
    SpriteRenderer m_SpriteRenderer;
    Vector2 m_VectorMove;
    float m_PatrolDuration;
    
    float m_lastPos;
    bool isDie=true;
    bool isKilled=true;
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
        originalColor=m_SpriteRenderer.color;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(m_die && isDie)
        {
            isDie=false;
            Invoke("returnPool",0.5f);
        }
        if(player)
        {
            Track();
        }
        else
        {
            Search();
        }
        UpdateFacing();
        m_VectorMove.y=m_Rigidbody2D.velocity.y;
        m_Rigidbody2D.velocity=m_VectorMove;
        m_VectorMove.x=0f;
        m_lastPos=transform.position.x;
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
            Burning();
        }
        else if(sqrDistance<=loseRadius * loseRadius)
        {
            m_VectorMove.x=(player.position-transform.position).normalized.x * trackSpeed;
        }
        else
        {
            player=null;
        }
    }
    //攻击函数
    void Burning()
    {
       if(att<=0)
       {
            m_Animator.SetTrigger("burning");
            isKilled=false;
            attackDamage.TriggerDamageOnce();
            m_die=true;
            att++;
       }
    }
    //改变朝向
    void UpdateFacing()
    {
        m_Animator.SetFloat("running", Mathf.Abs(m_Rigidbody2D.velocity.x));

        if(transform.position.x-m_lastPos>0)
        {
            transform.localScale = new Vector3(1f , 1f, 1);
            //attackDamage.h=1;
        }
        else if(transform.position.x-m_lastPos<0)
        {
            transform.localScale = new Vector3(-1f , 1f, 1);
            //attackDamage.h=-1;
        }
        else if(player!=null && player.position.x>transform.position.x)
            transform.localScale = new Vector3(1f , 1f, 1);
        else
            transform.localScale = new Vector3(-1f , 1f, 1);
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
        stateSet();     //重置
        //掉落
        int randKey=Random.Range(0,2);
        if(randKey!=0 && isKilled) 
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
            //Debug.Log("死亡");
            Destroy(gameObject);
        }   
    }
    //重置生命值等
    void stateSet()
    {
        att=0;
        attackDamageable.currentHealth=attackDamageable.maxHealth;
        m_die=false;
        isDie=true;
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