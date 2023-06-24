using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class TurnAround : Action
{
   public override TaskStatus OnUpdate()
   {
      var scale = transform.localScale;
      scale.x = scale.x * -1;
      transform.localScale = scale;
      return TaskStatus.Success;
   }
}
