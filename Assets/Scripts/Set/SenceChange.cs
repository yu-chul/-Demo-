using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SenceChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene ("#2-singlegame");//引号内为场景
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
