using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class BossChargeAttack : Action
{
   [SerializeField] private float chargeMoveSpeed;
   [SerializeField] private int damage;
   [SerializeField] private float attackSize;
   [SerializeField] private LayerMask playerMask;
   [SerializeField] private Transform attackPoint;
   [SerializeField] private Transform target;

   private bool isHit;
   private Vector3 direction;

   public override void OnStart()
   {
      if(target == null) target = PlayerControl.Instance.transform;
      direction = (target.position - transform.position).normalized;
      isHit = false;
   }

   public override TaskStatus OnUpdate()
   {
      transform.Translate(chargeMoveSpeed * Time.deltaTime * direction);
      if(!isHit ) {
         var collider = Physics2D.OverlapCircle(attackPoint.position, attackSize, playerMask);
         if (collider != null) {
            if (collider.gameObject.TryGetComponent(out Health health)) {
               health.TakeDamage(damage);
               isHit = true;
            }
         }
      }
      return TaskStatus.Running;
   }

   public override void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(attackPoint.position, attackSize);
   }
}
