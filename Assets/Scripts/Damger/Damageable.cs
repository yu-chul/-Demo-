using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Damageable : MonoBehaviour
{
    [System.Serializable]
    public class DamageableEvent : UnityEvent<Damger,Damageable>{}
    public DamageableEvent OnTakeDamage;
    public DamageableEvent onDie;
    public float maxHealth=0f;
    public float currentHealth;
    public float exHealth; 
    void Start()
    {
        int n = 0;
        n = LayerGenerator.instance.layer;
        if(gameObject.name!="Player")
        {
            switch (n)
            {   
                case 1:
                    maxHealth=3f;
                break;
                case 2:
                    maxHealth=5f;
                break;           
                case 3:
                    maxHealth=6f;
                break;
                case 4:
                    maxHealth=8f;
                break;
                case 5:
                    maxHealth=10f;
                break;
                case 6:
                    maxHealth=12f;
                break;
                default:
                    maxHealth=6f;
                break;
            }
        }
        if(gameObject.tag=="boss")
        {
            switch (n)
            {   
                case 1:
                    maxHealth=20f;
                break;
                case 2:
                    maxHealth=25f;
                break;           
                case 3:
                    maxHealth=30f;
                break;
                case 4:
                    maxHealth=35f;
                break;
                case 5:
                    maxHealth=40f;
                break;
                case 6:
                    maxHealth=45f;
                break;
                default:
                    maxHealth=50f;
                break;
            }
        }
        if(gameObject.name=="Player" && n==1)
            currentHealth=maxHealth;
    }
    public void TakeDamage(Damger damger)
    {
        //Debug.Log(gameObject.name+currentHealth);
        float tempHealth;
        float dam=damger.damage;

        //若以后新增血量类型，只需再写多一层计算即可
        tempHealth = exHealth;
        exHealth -= dam;
        exHealth = exHealth > 0 ? exHealth : 0;
        dam = dam - tempHealth > 0 ? dam - tempHealth : 0;

        currentHealth-=dam;

        OnTakeDamage.Invoke(damger,this);
        if(currentHealth<=0)
        {
            //die
            onDie.Invoke(damger,this);
        }
    }
}
