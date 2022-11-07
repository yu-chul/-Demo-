using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss1 : MonoBehaviour
{
    public bool isTakeAction=false;       //当前是否在进行动作
    public float speed, jumpForce;  //移动速度和跳跃力量
    public Transform groundCheck;   //地面监测点的位置
    public LayerMask ground;        //图层layer
    public bool isGround;
    public bool isJumped;
    public bool isMoved;
    public float attackInterval;    //攻击间隔时间1
    public float jumpInterval;    //跳跃间隔时间
    public int shootRange1;    //射距1
    public int shootRange2;    //射距2

    Vector2 target;
    Transform player;
    Animator m_Animator;
    Rigidbody2D m_Rigidbody2D;
    SpriteRenderer m_SpriteRenderer;

    bool isAlreadyDie=true;
    bool isDie=false;
    float m_AttackTimer;    //攻击计时1
    float m_JumpTimer;    //跳跃计时
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        m_Animator=GetComponent<Animator>();
        m_Rigidbody2D=GetComponent<Rigidbody2D>();
        m_SpriteRenderer=GetComponent<SpriteRenderer>();
        
        gameObject.GetComponent<Damageable>().currentHealth=gameObject.GetComponent<Damageable>().maxHealth;
    }

    void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground); //判断是否在地面上
    }
    void FixedUpdate()
    {
        if(isDie && isAlreadyDie)
        {
            isAlreadyDie=false;
            m_Animator.SetTrigger("dis");
        }

        if(m_Rigidbody2D.velocity.x==0f)
            isMoved=false;

        if(isGround && !isTakeAction && !isMoved)
        {
            m_Animator.SetBool("fall", false);
            m_Rigidbody2D.velocity = new Vector2(0f,0f);
            isJumped=false;
        }

        if(!isTakeAction)
            jump();

        if(!isJumped && !isTakeAction)
            move();

        if(!isTakeAction && !isJumped && !isMoved)
        {
            attack();
        }

        if(Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log(m_Rigidbody2D.velocity.x);
        }

        m_AttackTimer+=Time.deltaTime;
        m_JumpTimer+=Time.deltaTime;
    }
    //进行跳跃
    void jump()
    {
        if(m_JumpTimer<jumpInterval)
            return;
        if(Mathf.Abs(transform.position.y-player.position.y)<2.5f)
            return;
        if(!isGround)
            return;
        m_JumpTimer=0f;
        isJumped=true;
        if(player.position.x-transform.position.x>0)
        {
            m_Rigidbody2D.velocity = new Vector2(speed, jumpForce);
            transform.localScale = new Vector3(1f, 1f, 1);
        }
        else
        {
            m_Rigidbody2D.velocity = new Vector2(speed * -1f, jumpForce);
            transform.localScale = new Vector3(-1f, 1f, 1);
        }
        m_Animator.SetTrigger("jump");
    }
    //进行移动
    void move()
    {
        m_Animator.SetFloat("running", Mathf.Abs(m_Rigidbody2D.velocity.x));
        if(Mathf.Abs(transform.position.x-player.position.x) <= shootRange1 && Mathf.Abs(transform.position.x-player.position.x) <= shootRange2 && Mathf.Abs(transform.position.x-player.position.x) >= 2f)
            return;
        isMoved=true;
        if(Mathf.Abs(transform.position.x-player.position.x)<2f)
        {
            if(player.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1);
                m_Rigidbody2D.velocity = new Vector2(-1f * speed, m_Rigidbody2D.velocity.y);
                m_Animator.SetFloat("running", Mathf.Abs(m_Rigidbody2D.velocity.x));
                Invoke("stopMovIng",0.75f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1);
                m_Rigidbody2D.velocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
                m_Animator.SetFloat("running", Mathf.Abs(m_Rigidbody2D.velocity.x));
                Invoke("stopMovIng",0.75f);
            }
            RaycastHit2D hit = Physics2D.Raycast(transform.position,new Vector2(transform.localScale.x * 0.5f,0f),0.5f,ground);
            if (hit.collider!=null && hit.collider.tag=="Wall")
            {
                isJumped=true;
                m_JumpTimer=0f;
                transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1);
                m_Rigidbody2D.velocity = new Vector2((speed + 2f) * transform.localScale.x, jumpForce);
                m_Animator.SetTrigger("jump");
                Invoke("takeAontherSide",1f);
            }
        }
        else if(Mathf.Abs(transform.position.x-player.position.x)>=shootRange1 || Mathf.Abs(transform.position.x-player.position.x)>=shootRange2)
        {
            if(player.position.x > transform.position.x)
            {
                transform.localScale = new Vector3(1f, 1f, 1);
                m_Rigidbody2D.velocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
                m_Animator.SetFloat("running", Mathf.Abs(m_Rigidbody2D.velocity.x));
                Invoke("stopMovIng",0.75f);
            }
             else
            {
                transform.localScale = new Vector3(-1f, 1f, 1);
                m_Rigidbody2D.velocity = new Vector2(-1f * speed, m_Rigidbody2D.velocity.y);
                m_Animator.SetFloat("running", Mathf.Abs(m_Rigidbody2D.velocity.x));
                Invoke("stopMovIng",0.75f);
            }
        }
    }
    //进行攻击
    void attack()
    {
        if(player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1f, 1f, 1);
        }
        else if(player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1);
        }
        int key=Random.Range(1,4);
        switch(key)
        {
            case 1:
                upShoot();
            break;
            case 2:
                duck();
            break;
            case 3:
                shoot();
            break;
        }
    }
    void duck()
    {
        if(m_AttackTimer>=attackInterval)
        {
            isTakeAction=true;
            m_Animator.SetTrigger("duck");
            m_AttackTimer=0f;
            for(int i=0;i<3;i++)
            {
                Invoke("duckShoot",0.1f+(float)i);
                Invoke("duckShoot",0.2f+(float)i);
                Invoke("duckShoot",0.3f+(float)i);
            }
            Invoke("actionFinshed",3f);
        }
        else
        {
            return;
        }
    }
    void duckShoot()
    {
        bossBulletPool.instance.GetFormPool(new Vector2(3.5f * transform.localScale.x,0f),new Vector2(transform.position.x,transform.position.y-0.4f),2);
    }
    void shoot()
    {
        if(m_AttackTimer>=attackInterval)
        {
            isTakeAction=true;
            m_Animator.SetTrigger("shoot");
            m_AttackTimer=0f;
            for(int i=0;i<7;i++)
            {
                Invoke("nomalShoot",((float)i)/2);
            }
            Invoke("actionFinshed",3.6f);
        }
        else
        {
            return;
        }
    }
    void nomalShoot()
    {
        bossBulletPool.instance.GetFormPool(new Vector2(3.5f * transform.localScale.x,player.position.y - transform.position.y),gameObject.transform.position,2);
    }
    void upShoot()
    {
        if(m_AttackTimer>=attackInterval)
        {
            isTakeAction=true;
            m_Animator.SetTrigger("upshoot");
            m_AttackTimer=0f;
            for(int i=0;i<5;i++)
            {
                Invoke("upShooting",((float)i)/2);
            }
            Invoke("actionFinshed",3f);
        }
        else
        {
            return;
        }
    }
    void upShooting()
    {
        int key=Random.Range(1,9);
        switch(key)
        {
            case 1:
                bossBulletPool.instance.GetFormPool(new Vector2(5f,4.5f),new Vector2(transform.position.x,transform.position.y+0.65f),1);
            break;
            case 2:
                bossBulletPool.instance.GetFormPool(new Vector2(4f,3.5f),new Vector2(transform.position.x,transform.position.y+0.65f),1);
            break;
            case 3:
                bossBulletPool.instance.GetFormPool(new Vector2(3f,2.5f),new Vector2(transform.position.x,transform.position.y+0.65f),1);
            break;
            case 4:
                bossBulletPool.instance.GetFormPool(new Vector2(2f,1.5f),new Vector2(transform.position.x,transform.position.y+0.65f),1);
            break;
            case 5:
                bossBulletPool.instance.GetFormPool(new Vector2(-2f,1.5f),new Vector2(transform.position.x,transform.position.y+0.65f),1);
            break;
            case 6:
                bossBulletPool.instance.GetFormPool(new Vector2(-3f,2.5f),new Vector2(transform.position.x,transform.position.y+0.65f),1);
            break;
            case 7:
                bossBulletPool.instance.GetFormPool(new Vector2(-4f,3.5f),new Vector2(transform.position.x,transform.position.y+0.65f),1);
            break;
            case 8:
                bossBulletPool.instance.GetFormPool(new Vector2(-5f,4.5f),new Vector2(transform.position.x,transform.position.y+0.65f),1);
            break;
        }
    }
    void stopMovIng()
    {
        m_Rigidbody2D.velocity = new Vector2(0f,0f);
    }
    void takeAontherSide()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1);
    }
    public void OnTakeDamage()
    {
        //受伤动画
        m_Animator.SetTrigger("hurt");
    } 
    public void onDie()
    {
        isDie=true;
    }
    //动画事件
    void bossDie()
    {
        EventSystem.GetInstance().SendEvent(new BaseEvent(EventType.BOSS_DIED,null));
        Destroy(gameObject);
    }

    void bossAppear()
    {
        m_Animator.SetTrigger("idle");
        isTakeAction=false;
    }
    void takeClint()
    {
        m_Animator.SetTrigger("clint");
        m_Animator.SetBool("fall", true);
    }
    void clintShoot()
    {
        //Debug.Log("1111");
    }
    void actionFinshed()
    {
        m_Animator.SetTrigger("idle");
        isTakeAction=false;
    }
}
