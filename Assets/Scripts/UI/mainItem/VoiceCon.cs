using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceCon : MonoBehaviour
{
    public static VoiceCon instance;  //单例创建
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


}
