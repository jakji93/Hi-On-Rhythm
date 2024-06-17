using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayProjectile : Projectile
{
   [SerializeField] private float delay = 0;

   override protected void Update()
   {
      if (delay < 0) {
         base.Update();
      } else {
         delay -= Time.deltaTime;
      }
   }
}
