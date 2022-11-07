
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Damger b_attackDamage;     //伤害计算组件
    private Rigidbody2D rb2d;
    private Animator anim;
    public GameObject thing;
    public GameObject log;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    void Explode()
    {
        b_attackDamage.TriggerDamageOnce();
    }

    void logSet()
    {
        thing.SetActive(false);
        log.SetActive(false);
    }
    void returnPool()
    {
        BombPool.instance.ReturnPool(gameObject);
    }
    
}
