using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class BossMovement : Action
{
   [SerializeField] private Transform target;
   [SerializeField] private float moveSpeed;

   private float baseScaleX;

   public override void OnAwake()
   {
      baseScaleX = transform.localScale.x;
   }

   public override void OnStart()
   {
      if (target == null) target = PlayerControl.Instance.transform;
   }

   public override TaskStatus OnUpdate()
   {
      Move();
      FaceTarget();
      return TaskStatus.Running;
   }

   private void Move()
   {
      var newDirection = (target.position - transform.position).normalized;
      transform.Translate(moveSpeed * Time.deltaTime * newDirection);
   }

   private void FaceTarget()
   {
      var scale = transform.localScale;
      scale.x = transform.position.x > target.position.x ? -baseScaleX : baseScaleX;
      transform.localScale = scale;
   }
}
