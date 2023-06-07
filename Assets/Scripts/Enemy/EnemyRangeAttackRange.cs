using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttackRange : EnemyAttackRange
{
   [SerializeField] private float range;

   public override bool IsInAttackRange()
   {
      return Vector2.Distance(transform.position, PlayerControl.Instance.gameObject.transform.position) < range;
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(attackPoint.position, range);
   }
}
