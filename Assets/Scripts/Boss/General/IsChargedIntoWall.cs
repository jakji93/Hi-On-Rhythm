using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsChargedIntoWall : Conditional
{
   [SerializeField] private float attackSize;
   [SerializeField] private LayerMask wallMask;
   [SerializeField] private Transform attackPoint;

   public override TaskStatus OnUpdate()
   {
      var collider = Physics2D.OverlapCircle(attackPoint.position, attackSize, wallMask);
      return collider != null? TaskStatus.Success : TaskStatus.Failure;
   }

   public override void OnDrawGizmos()
   {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(attackPoint.position, attackSize);
   }
}
