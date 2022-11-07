using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BombUI : MonoBehaviour
{
    // Start is called before the first frame update
    Text text;

    // Update is called once per frame
    void Update()
    {
        //Coin.GetComponent<Text>() = Player.instance.gold.ToString();
        text=gameObject.GetComponent<Text>();
        text.text=Player.instance.bombCount.ToString();
    }
}
