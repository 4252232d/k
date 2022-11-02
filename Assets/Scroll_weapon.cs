using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll_weapon : MonoBehaviour
{
    public GameObject w1;
    public GameObject w2;
    public GameObject w3;
    
    int s;
    public int weapon = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Scroll(){
        if(s == 0)
        {
            w1.SetActive(true);
            w2.SetActive(false);
            w3.SetActive(false);
        }
        if(s == 1)
        {
            w1.SetActive(false);
            w2.SetActive(true);
            w3.SetActive(false);
        }
        if(s == 2)
        {
            w1.SetActive(false);
            w2.SetActive(false);
            w3.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(s <= 0)
        {
            s = 0;
        Scroll();

        }
        if(s >= weapon)
        {
            s = weapon;
            Scroll();
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)  //Mouse ScrollWheel - прокрутка колессика мыши
        {
            s -= 1;
            Scroll();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            s += 1;
            Scroll();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            s += 1;
                        Scroll();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            s -= 1;
                        Scroll();

        }

    } 
            
}
