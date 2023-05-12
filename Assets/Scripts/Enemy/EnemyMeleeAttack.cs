using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
   [SerializeField] private float damage;
   [SerializeField] private Vector2 attackSize;
   [SerializeField] private LayerMask playerMask;

   public override void Attack()
   {
      var collider = Physics2D.OverlapBox(attackPoint.position, attackSize, 0, playerMask);
      if (collider != null) {
         //Player take damage
         Debug.Log(collider.gameObject.name);
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(attackPoint.position, attackSize);
   }
}
