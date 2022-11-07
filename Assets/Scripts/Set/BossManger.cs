using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossManger : MonoBehaviour
{
    public static BossManger instance;  //单例创建
    public GameObject boss1;
    public GameObject blode;
    public GameObject bossRoom;
    private Image health;
    GameObject boss;
    float maxHealth;
    bool isShow=false;
    void Start()
    {
        instance = this;

        EventSystem.GetInstance().AddEventListener(EventType.BOSS_DIED, bossDie);

        health = blode.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isShow)
            health.fillAmount = (float)(boss.GetComponent<Damageable>().currentHealth) / (float)maxHealth;
    }
    public void bossInit(Transform pos)
    {
        blode.SetActive(true);
        isShow=true;
        boss = Instantiate(boss1);
        boss.transform.position = new Vector2(pos.position.x,pos.position.y+6f);
        maxHealth=boss.GetComponent<Damageable>().maxHealth;
    }

    void bossDie(BaseEvent evt)
    {
        isShow=false;
        bossRoom = GameObject.FindWithTag("bossRoom");
        blode.SetActive(false);
        GameObject com = bossRoom.transform.GetChild(1).gameObject;
        com.SetActive(true);
        com = bossRoom.transform.GetChild(2).gameObject;
        com.SetActive(true);
    }
}
