using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamgerUI : MonoBehaviour
{
    public GameObject it;

    // Update is called once per frame
    public void UpdateDamgerUI()
    {
        if(GameObject.Find("da")!=null)
        {
            int count = GameObject.Find("da").transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(GameObject.Find("da").transform.GetChild(i).gameObject);
            }
            for(int i=0;i<Player.instance.da;i++)
            {
                Instantiate(it, GameObject.Find("da").transform);
            }
        }
    }
}
