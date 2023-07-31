using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class PenetrationProjectile : Projectile
{
   override protected void Update()
   {
      Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
      Vector2 newPosition = currentPosition + (Vector2)transform.right * speed * Time.deltaTime;
      transform.position = newPosition;
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.CompareTag("Player")) return;
      if (collision.gameObject.TryGetComponent(out Health health)) {
         health.TakeDamage(damage * ComboManager.Instance.GetMultiplier());
      }
   }
}
