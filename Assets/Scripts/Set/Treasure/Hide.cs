using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Treasure.instance.InitInRand(new Vector2(transform.position.x,transform.position.y+2f));
    }

}
