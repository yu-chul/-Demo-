using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
public class Damger : MonoBehaviour
{
    public float damage;
    public Vector2 offset;
    public Vector2 size;
    public LayerMask damageableLayer;

    ContactFilter2D m_DamageableFilter;
    Collider2D[] m_DamgeableReslts = new Collider2D[8];

    SpriteRenderer m_SpriteRenderer;

    bool m_EnableDamge;
    bool m_TriggerDamgeOnce;
    //子弹专用
    public bool isBurning=false;

    void Start()
    {
        m_DamageableFilter.layerMask=damageableLayer;
        m_DamageableFilter.useLayerMask=true;
        m_DamageableFilter.useTriggers=false;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        //offset.x*=h;
        
        if(!m_EnableDamge)  return;
        if(m_TriggerDamgeOnce)
        {
            m_EnableDamge=false;
            m_TriggerDamgeOnce=false;
        }
        //Vector2 tmpOffset = offset;
        if(gameObject.transform.localScale.x<=0) offset.x =-1 * Math.Abs(offset.x);
        else if(gameObject.transform.localScale.x>0)  offset.x =Math.Abs(offset.x);
        Vector2 center = (Vector2)transform.position + offset;
        Vector2 halfSize=size * 0.5f;
        Vector2 pointA=center-halfSize;
        Vector2 pointB=center+halfSize;

        int count =Physics2D.OverlapArea(pointA,pointB,m_DamageableFilter,m_DamgeableReslts);
        
        if(count>0)
        {
            if(gameObject.tag=="bomb")
            {
                for(int i=0;i<count;i++)
                {
                    Collider2D collider = m_DamgeableReslts[0];
                    Damageable damageable=collider.gameObject.GetComponent<Damageable>();
                    if(damageable!=null)
                    {
                        //攻击
                        damageable.TakeDamage(this);
                    }
                    isBurning=true;
                }
            }
            else
            {
                Collider2D collider = m_DamgeableReslts[0];
                Damageable damageable=collider.gameObject.GetComponent<Damageable>();
                if(damageable!=null)
                {
                    //攻击
                    damageable.TakeDamage(this);
                }
                isBurning=true;
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color=Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position+offset,size);
    }
    public void EnableFamge()
    {
        m_EnableDamge=true;
    }

    public void DisableDamage()
    {
        m_EnableDamge=false;
    }
    public void TriggerDamageOnce()
    {
        m_EnableDamge=true;
        m_TriggerDamgeOnce=true;
    }
}
