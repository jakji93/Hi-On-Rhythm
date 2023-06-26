using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsInAttackRange : Conditional
{
   [SerializeField] private EnemyAttackRange attackRange;

   public override TaskStatus OnUpdate()
   {
      return attackRange.IsInAttackRange() ? TaskStatus.Success : TaskStatus.Failure;
   }
}
