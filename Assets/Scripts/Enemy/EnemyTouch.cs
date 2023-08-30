using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTouch : MonoBehaviour
{
   [SerializeField] private LayerMask playerMask;
   [SerializeField] private int damage;

   private void OnTriggerStay2D(Collider2D collision)
   {
      if (((1 << collision.gameObject.layer) & playerMask) != 0) {
         if (collision.TryGetComponent<Health>(out Health health)) {
            health.TakeDamage(damage);
         }
      }
   }
}
