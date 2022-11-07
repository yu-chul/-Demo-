using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSpUI : MonoBehaviour
{
    public GameObject it;

    // Update is called once per frame
    public void UpdateShootSpUI()
    {
        if(GameObject.Find("shp")!=null)
        {
            int count = GameObject.Find("shp").transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(GameObject.Find("shp").transform.GetChild(i).gameObject);
            }
            for(int i=0;i<Player.instance.shp;i++)
            {
                Instantiate(it, GameObject.Find("shp").transform);
            }
        }
    }
}
