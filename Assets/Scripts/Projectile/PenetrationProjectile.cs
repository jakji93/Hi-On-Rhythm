using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PenetrationProjectile : Projectile
{

   private Collider2D[] hitHistory;

   override protected void Update()
   {
      Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
      Vector2 newPosition = currentPosition + (Vector2)transform.up * speed * Time.deltaTime;
      var hitInfos = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, targetMask);
      foreach (var hitInfo in hitInfos) {
         if(hitHistory.Contains(hitInfo)) {
            continue;
         }
         if (hitInfo.TryGetComponent(out Health health)) {
            health.TakeDamage(damage);
            hitHistory.Append(hitInfo);
         }
      }
      transform.position = newPosition;
   }
}
