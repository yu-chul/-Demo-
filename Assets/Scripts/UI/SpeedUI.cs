using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUI : MonoBehaviour
{
    public GameObject it;

    // Update is called once per frame
    public void UpdateSpeedUI()
    {
        if(GameObject.Find("sp")!=null)
        {
            int count = GameObject.Find("sp").transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(GameObject.Find("sp").transform.GetChild(i).gameObject);
            }
            for(int i=0;i<Player.instance.sp;i++)
            {
                Instantiate(it, GameObject.Find("sp").transform);
            }
        }
    }
}
