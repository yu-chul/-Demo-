using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public GameObject heartFull;
    public GameObject heartHalf;
    public GameObject heartVoid;
    public GameObject soulHeartFull;
    public GameObject soulHeartHalf;

    public void UpdateHP()
    {
        int count = GameObject.Find("HP").transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(GameObject.Find("HP").transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < (int)Player.instance.Health / 2; i++)
        {
            Instantiate(heartFull, GameObject.Find("HP").transform);
        }
        for (int i = 0; i < (int)Player.instance.Health % 2; i++)
        {
            Instantiate(heartHalf,GameObject.Find("HP").transform);
        }
        for (int i = 0; i < ((int)Player.instance.MaxHealth - (int)Player.instance.Health) / 2; i++)
        {
            Instantiate(heartVoid, GameObject.Find("HP").transform);
        }

        for (int i = 0; i < (int)Player.instance.SoulHealth / 2; i++)
        {
            Instantiate(soulHeartFull, GameObject.Find("HP").transform);
        }
        for (int i = 0; i < (int)Player.instance.SoulHealth % 2; i++)
        {
            Instantiate(soulHeartHalf, GameObject.Find("HP").transform);
        }
    }

}