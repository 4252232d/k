using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Bullet : MonoBehaviour
{
    // private Transform _tr;

   
   private void OnCollisionEnter(Collision other) 
   {
  Debug.Log("Саня бибик");
   }
}
