using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{   
    public static Player instance;  //单例创建

    [Header("组件")]
    [SerializeField] private Rigidbody2D rb;     //刚体
    [SerializeField] private Collider2D coll;    //碰撞体
    [SerializeField] private Animator anim;       //动画
    public Camera camera;
    Color originalColor;
    private Vector3 caPos;

    [Header("移动")]
    public float speed, jumpForce;  //移动速度和跳跃力量
    private float horizontalMove;   //转向
    public Transform groundCheck;   //地面监测点的位置
    public LayerMask ground;        //图层layer
    public bool isGround, isJump;   //两个是否在地面和跳跃的状态
    bool jumpPressed;   //是否按下跳跃键的状态
    int jumpCount;      //跳跃的次数
    //private float pos;
    private float lastpos;
    [Header("近战攻击")]
    Damger m_Damager;
    public float attackInterval;    //攻击间隔时间
    float attackTimer;              //攻击时间
    bool m_die;
    bool isAttack=false;
    bool isAttackToGo=false;

    [Header("远程攻击")]
    //float shootTimer; 
    public float shootRange;        //射距
    public float shootingSpeed=3f;    //射速
    public float shootingSpeedLite=1f;    //射速系数

    public float shootDamgeLite;    //射击伤害系数
    //public float shootInterval;   //射击时间间隔
    public float shootTimeLite=2f;
    public int bombCount;           //炸弹数量

    [Header("UI显示")]
    public Damageable attackDamageable;    //可受伤组件
    public Damger attackDamger;    //可受伤组件
    public float Health=6f;               //血量上限
    public float SoulHealth=1f;           //额外血量
    public float MaxHealth;           //最大血量
    public int gold;                //钱
    UIManager UI;

    [Header("数值计数")]
    public int da=2;    //伤害
    public int ra=2;    //射距
    public int sp=2;    //移速
    public int shp=2;    //射速

    public List<int> item=new List<int>();         //道具列表

    void Awake() 
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    void Start()
    {
        EventSystem.GetInstance().AddEventListener(EventType.GOLD_GET, goldGet);
        EventSystem.GetInstance().AddEventListener(EventType.HALFSOUL_GET, soulGet);
        EventSystem.GetInstance().AddEventListener(EventType.SOUL_GET, soulsGet);
        EventSystem.GetInstance().AddEventListener(EventType.HALFHEALTH_GET, healGet);
        EventSystem.GetInstance().AddEventListener(EventType.HEALTH_GET, healsGet);
        EventSystem.GetInstance().AddEventListener(EventType.HOLEHEART_GET, holeHealsGet);
        EventSystem.GetInstance().AddEventListener(EventType.GOLDS_GET, goldsGet);
        EventSystem.GetInstance().AddEventListener(EventType.HOLEGOLDS_GET, holegoldGet);
        EventSystem.GetInstance().AddEventListener(EventType.BOMB_GET, bombGet);

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        m_Damager=GetComponent<Damger>();

        Health=attackDamageable.currentHealth;
        MaxHealth=attackDamageable.maxHealth;
        SoulHealth=attackDamageable.exHealth;

        UI = UIManager.Instance;
        UI.PlayerUIInitialize();

        originalColor=gameObject.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)  //判断是否按下跳跃键和可跳跃的次数
        {
            jumpPressed = true;
        }
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Treasure.instance.gunTreasureInit(gameObject.transform.position);
        }
        if(Input.GetKeyDown(KeyCode.E) && bombCount>0)
        {
            bombCount--;
            var outBomb = BombPool.instance.GetFormPool(gameObject.transform.position);
            outBomb.GetComponent<Rigidbody2D>().velocity=transform.up * 5f + transform.right * 4f * transform.localScale.x;
            outBomb.GetComponent<Animator>().SetTrigger("bom");
        }
    }

    private void FixedUpdate()
    {
        if(m_die)
            die(); 
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground); //判断是否在地面上

        GroundMovement();

        Jump();

        Attack();

        ShootIng();

        SwitchAnim();

        attackTimer+=Time.deltaTime;
        //shootTimer+=Time.deltaTime;
        caPos=camera.ScreenToWorldPoint(Input.mousePosition);

        Health=attackDamageable.currentHealth;
        SoulHealth=attackDamageable.exHealth;

        UI.PlayerUIInitialize();
    }

    void GroundMovement()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal");      //获取移动指令，GetAxisRaw返回的是-1、0、1
        //anim.SetFloat("runing", Mathf.Abs(horizontalMove));
        if(Input.GetKey ("a") && !isAttackToGo)
            rb.velocity = new Vector2(-1f * speed, rb.velocity.y);   //实现人物的横向移动
        else if(Input.GetKey ("d") && !isAttackToGo)
            rb.velocity = new Vector2(1f * speed, rb.velocity.y);   //实现人物的横向移动
        else 
            rb.velocity = new Vector2(0f * speed, rb.velocity.y);
        //实现人物的翻转
        if (horizontalMove != 0)
        {
            transform.localScale = new Vector3(1f * horizontalMove, 1f, 1);
            //m_SpriteRenderer.flipX=true;
        }
    }
    //动画帧函数
    void isAttackSet()
    {
        isAttackToGo=false;
    }

    void Jump()
    {
        //Debug.Log(isGround);
        if (isGround)   //在地面，刷新跳跃的次数和状态
        {
            jumpCount = 2;
            isJump = false;
        }
        if (jumpPressed && isGround)  //按下了跳跃键且在地面
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;    //确保命令执行完毕
        }						//可以无需isJump改为!isGround
        else if (jumpPressed && jumpCount > 0 && isJump)   //多段跳的实现
        {
            anim.SetBool("jumping", true);
            anim.SetBool("falling", false);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
            jumpPressed = false;
        }
    }
    //攻击函数
    void Attack()
    {
        if((Input.GetKey(KeyCode.X) || Input.GetMouseButton(1)) && attackTimer>=attackInterval)
        {
            isAttackToGo=true;
            attackTimer=0f;
            isAttack=true;
            //Debug.Log("攻击");
            anim.SetTrigger("attack");
            m_Damager.TriggerDamageOnce();
            rb.velocity = new Vector2(0f,0f);
        }
    }
    //射击
    void ShootIng()
    {
        Vector2 pos=new Vector2(0f,0f);
        if(Input.GetKey ("up"))
        {
            pos.y+=shootingSpeed * shootDamgeLite;
        }
        else if(Input.GetKey ("down"))
        {
            pos.y-=shootingSpeed * shootDamgeLite;
        }
        if(Input.GetKey ("left"))
        {
            pos.x-=shootingSpeed * shootDamgeLite;
        }
        else if(Input.GetKey ("right"))
        {
            pos.x+=shootingSpeed * shootDamgeLite;
        }
        if(Input.GetMouseButton(0) && attackTimer>=attackInterval*shootTimeLite)
        {
            attackTimer=0f;
            float a=caPos.y - transform.position.y;

            if(caPos.x - transform.position.x>0)
                BulletsPool.instance.GetFormPool(new Vector2(shootingSpeed * shootDamgeLite,a),transform);
            else if(caPos.x - transform.position.x<0)
                BulletsPool.instance.GetFormPool(new Vector2(-1*shootingSpeed * shootDamgeLite,a),transform);
            else
                BulletsPool.instance.GetFormPool(new Vector2(0f,a),transform);
        }
        else if(attackTimer>=attackInterval*shootTimeLite && (Input.GetKey ("up") || Input.GetKey ("down") || Input.GetKey ("left") || Input.GetKey ("right")))
        {
            attackTimer=0f;
            BulletsPool.instance.GetFormPool(pos,transform);
        }
        
    }
    void SwitchAnim()//动画切换
    {
        anim.SetFloat("running", Mathf.Abs(rb.velocity.x));
        

        if(isAttack && attackTimer>=attackInterval)
        {
            isAttack=false;
            anim.ResetTrigger("attack");
            anim.SetTrigger("idle");
        }

        if (isGround)
        {
            anim.SetBool("falling", false);
        }
        else if (!isGround && rb.velocity.y > 0)
        {
            anim.SetBool("jumping", true);
        }
        else if (rb.velocity.y < 0)
        {
            anim.SetBool("jumping", false);
            anim.SetBool("falling", true);
        }
    }
    public void OnDie()
    {
        m_die=true;
        //死亡动画
    }

    public void OnTakeDamage()
    {
        //Debug.Log("攻击1");
        //受击动画
        FlashColor(0.1f);
        
    } 
    //死亡函数
    void die()
    {
        Died.instance.diedUIShow();
    }
    //受伤闪烁
    void FlashColor(float time)
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Invoke("ResetColor", time);
    }

    void ResetColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = originalColor;
    }

    public void switchBullet(int type)
    {
        BulletsPool.instance.switchBullet(type);
        switch (type)
            {
                case 1:
                    shootTimeLite=1f;
                break;
                case 2:
                    shootTimeLite=2.5f;
                break;
                case 3:
                    shootTimeLite=0.4f;
                break;
                case 4:
                    shootTimeLite=0.9f;
                break;
                case 5:
                    shootTimeLite=0.7f;
                break;
                case 6:
                    shootTimeLite=3f;
                break;
                case 7:
                    shootTimeLite=0.7f;
                break;
                case 8:
                    shootTimeLite=5f;
                break;
            }
    }

    ///////////////////////////////////接收信息函数
    void goldGet(BaseEvent evt)
    {
        gold++;
    }
    void goldsGet(BaseEvent evt)
    {
        gold+=25;
    }
    void holegoldGet(BaseEvent evt)
    {
        gold+=99;
    }
    void soulGet(BaseEvent evt)
    {
        SoulHealth++;
        attackDamageable.exHealth=SoulHealth;
    }
    void soulsGet(BaseEvent evt)
    {
        SoulHealth+=2;
        attackDamageable.exHealth=SoulHealth;
    }
    void healGet(BaseEvent evt)
    {
        if(Health<MaxHealth)
        {
            Health++;
            attackDamageable.currentHealth=Health;
        }
    }
    void healsGet(BaseEvent evt)
    {
        if(MaxHealth-Health>=2)
        {
            Health+=2;
            attackDamageable.currentHealth=Health;
        }
    }
    void holeHealsGet(BaseEvent evt)
    {
        MaxHealth+=2;
        Health+=2;
        attackDamageable.maxHealth=MaxHealth;
        attackDamageable.currentHealth=Health;
    }
    void bombGet(BaseEvent evt)
    {
        bombCount++;
    }
}



