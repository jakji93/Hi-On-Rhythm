using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class IsGameOver : Conditional
{
   public override TaskStatus OnUpdate()
   {
      return GameplayManager.Instance.IsGameOver()? TaskStatus.Success : TaskStatus.Failure;
   }
}
