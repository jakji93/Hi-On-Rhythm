using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttackRange : EnemyAttackRange
{
   [SerializeField] private Vector2 attackSize;
   [SerializeField] private LayerMask playerMask;
   public override bool IsInAttackRange()
   {
      return Physics2D.OverlapBox(attackPoint.position, attackSize, 0, playerMask);
   }

   private void OnDrawGizmos()
   {
      if(attackPoint == null) return;
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube(attackPoint.position, attackSize);
   }
}
