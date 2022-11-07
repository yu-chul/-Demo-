using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUI : MonoBehaviour
{
    public GameObject it;

    // Update is called once per frame
    public void UpdateRange()
    {
        if(GameObject.Find("ra")!=null)
        {
            int count = GameObject.Find("ra").transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Destroy(GameObject.Find("ra").transform.GetChild(i).gameObject);
            }
            for(int i=0;i<Player.instance.ra;i++)
            {
                Instantiate(it, GameObject.Find("ra").transform);
            }
        }
    }
}
