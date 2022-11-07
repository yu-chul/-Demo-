using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour
{

    // Update is called once per frame
    public void UpdateStaff()
    {
        if(GameObject.Find("item")!=null)
        {
            int count = GameObject.Find("item").transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(GameObject.Find("item").transform.GetChild(i).gameObject);
            }
            var treasure=(GameObject)null;
            for(int i=0;i<Player.instance.item.Count;i++)
            {
                treasure = (GameObject)Resources.Load((Player.instance.item[i]+100).ToString());
                treasure = Instantiate(treasure,GameObject.Find("item").transform);
            }
        }
    }
}
