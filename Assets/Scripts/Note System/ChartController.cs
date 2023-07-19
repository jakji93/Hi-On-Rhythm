using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartController : MonoBehaviour
{
   public Transform notehitzone;
   public Transform hitcircle;
   public Transform target;

   private Transform thisCircle;

   private void Awake()
   {
      Debug.Log(transform.childCount);
   }

   private void Update()
   {
      var distance = transform.position.x - notehitzone.position.x;

      if(distance < 1440 && thisCircle == null) {
         thisCircle = Instantiate(hitcircle, target);
      }
      if(thisCircle != null) {
         var scale = Mathf.Max(1 + distance / 480, 1);
         thisCircle.localScale = new Vector3(scale, scale, 0);
      }
   }
}
