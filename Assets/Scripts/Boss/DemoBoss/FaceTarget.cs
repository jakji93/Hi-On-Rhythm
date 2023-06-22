using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime.Tasks;

public class FaceTarget : Action
{
   private float baseScaleX;

   [SerializeField] private Transform target;

   public override void OnAwake()
   {
      base.OnAwake();
      baseScaleX = transform.localScale.x;
   }

   public override TaskStatus OnUpdate()
   {
      var scale = transform.localScale;
      scale.x = transform.position.x > target.position.x ? -baseScaleX : baseScaleX;
      transform.localScale = scale;
      return TaskStatus.Success;
   }
}
