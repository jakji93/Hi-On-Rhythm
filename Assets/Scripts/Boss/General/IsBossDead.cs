using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsBossDead : Conditional
{
   [SerializeField] private Health health;

   public override TaskStatus OnUpdate()
   {
      if(health == null) return TaskStatus.Failure;
      return health.GetCurHealth() <= 0 ? TaskStatus.Success : TaskStatus.Failure;
   }
}
