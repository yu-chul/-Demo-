using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class LayerGenerator : MonoBehaviour
{
    public static LayerGenerator instance;  //单例创建
    // Start is called before the first frame update
    public int layer=1;
    void Awake() 
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    // Update is called once per frame
    void Update()
    {
        cnimaReSet();
    }
    //相机角色重置
    void cnimaReSet()
    {   
        GameObject a=GameObject.Find("CM vcam1");
        if(a.GetComponent<CinemachineVirtualCamera>().m_Follow==null)
            a.GetComponent<CinemachineVirtualCamera>().m_Follow=GameObject.FindGameObjectWithTag("Player").transform;
    }
}
