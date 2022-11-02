using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject Bullet;
    public float Power;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if (Input.GetMouseButtonDown(0)){
            GameObject b = Instantiate(Bullet, transform.position, transform.rotation);
            b.GetComponent<Rigidbody>().AddForce(Vector3.forward * Power, ForceMode.Impulse);
        }
    } 
    
}
