using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoldUI : MonoBehaviour
{
    //public GameObject Coin;
    Text text;


    // Update is called once per frame
    void Update()
    {
        //Coin.GetComponent<Text>() = Player.instance.gold.ToString();
        text=gameObject.GetComponent<Text>();
        text.text=Player.instance.gold.ToString();
    }
}
