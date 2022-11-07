using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : Singleton<UIManager>
{
    //[HideInInspector]
    //public Level level;
    [HideInInspector]
    public Player player;

    //public MiniMap miniMap;

    //public Slider bossHp;

    public HP hp;
    //public Staff staff;

    //public Attributes attributes;

    //public PausePanel pausePanel;

    void Start()
    {
        //level = GameManager.Instance.level;
        player = Player.instance;
    }

    public void PlayerUIInitialize()
    {
        hp.UpdateHP();
        //staff.UpdateStaff();
        //attributes.UpDateAttributes();
    }
}