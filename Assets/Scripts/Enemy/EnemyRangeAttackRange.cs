using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttackRange : EnemyAttackRange
{
   [SerializeField] private float range;
   [SerializeField] private Transform target;

   public override bool IsInAttackRange()
   {
      return Vector2.Distance(transform.position, target.position) < range;
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(attackPoint.position, range);
   }
}
