using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassRoad : MonoBehaviour
{
    public GameObject log;
    private bool canOpen;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(canOpen)
            {
                Destroy(GameObject.Find("Player"));

                Destroy(GameObject.Find("LayerGenerator"));
                SceneManager.LoadScene ("#2-singlegame");
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag=="Player" )
        {
            canOpen = true;
            log.SetActive(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag=="Player")
        {
            canOpen = false;
            log.SetActive(false);
        }
    }
}
