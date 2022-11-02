using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physik : MonoBehaviour
{
    public Rigidbody rb;
    // Start is called before    the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
