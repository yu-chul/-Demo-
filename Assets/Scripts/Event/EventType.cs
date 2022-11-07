using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    GAME_START,     //开始游戏
    SHOOT_MISS,     //未击中
    GAME_TEST,      //测试事件
    MONSTER_INIT,    
    MONSTER_DES, 
    BULLET_INIT, 
    BULLET_DES, 
    ROOM_ENTER,     //玩家进入房间
    MONSTER_DIE,    //怪物死亡
    GOLD_GET,       //获取金币
    HALFSOUL_GET,       //获取半颗魂心
    SOUL_GET,       //获取一颗魂心
    HALFHEALTH_GET,       //获取半颗心
    HEALTH_GET,       //获取一颗心
    HOLEHEART_GET,     //获取血量上限
    GOLDS_GET,       //获取25金币\
    HOLEGOLDS_GET,       //获取金币
    BOMB_GET,       //获取炸弹
    PLAYER_DIED,   //玩家死亡
    BOSS_DIED,      //boss死亡
    BOSS_ENTER,     //进入boss房
}
