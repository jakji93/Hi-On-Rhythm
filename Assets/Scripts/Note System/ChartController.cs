using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartController : MonoBehaviour
{
   public Transform notehitzone;
   public Transform hitcircle;

   private void Awake()
   {
      Debug.Log(transform.childCount);
   }

   private void Update()
   {
      var distance = transform.position.x - notehitzone.position.x;
      if(distance < -200 || distance > 1200) hitcircle.gameObject.SetActive(false);
      else hitcircle.gameObject.SetActive(true);
      var scale = 1 + distance/480;
      hitcircle.localScale = new Vector3(scale, scale, 0);
   }
}
