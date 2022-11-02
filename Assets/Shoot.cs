using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject Bullet;
    public float Power;
  

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetMouseButtonDown(0)){
            GameObject b = Instantiate(Bullet, transform.position, transform.rotation);
            b.GetComponent<Rigidbody>().AddForce(Vector3.forward * Power, ForceMode.Impulse);
        }
        
    }
}
