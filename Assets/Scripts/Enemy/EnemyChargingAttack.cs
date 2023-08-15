using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyChargingAttack : EnemyAttack
{
   [SerializeField] private int damage;
   [SerializeField] private Vector2 attackSize;
   [SerializeField] private LayerMask playerMask;
   [SerializeField] private float chargeMoveSpeed;

   private bool isCharging = false;
   private Vector3 chargeDirection;

   public override void Attack()
   {
      var player = PlayerControl.Instance.transform;
      chargeDirection = (player.position - transform.position).normalized;
      isCharging = true;
   }

   public override void EndAttacking()
   {
      base.EndAttacking();
      isCharging = false;
   }

   protected override void Update()
   {
      base.Update();
      if (isCharging) {
         var collider = Physics2D.OverlapBox(attackPoint.position, attackSize, 0, playerMask);
         if (collider != null) {
            //Player take damage
            if (collider.gameObject.TryGetComponent(out Health health)) {
               health.TakeDamage(damage);
            }
         }
         transform.Translate(chargeMoveSpeed * Time.deltaTime * chargeDirection);
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireCube(attackPoint.position, attackSize);
   }
}
