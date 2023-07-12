using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile
{
   [SerializeField] private float angularSpeed;

   private Transform targetPlayer;

   protected override void Update()
   {
      AngularTurn();
      base.Update();
   }

   private void AngularTurn()
   {
      if (targetPlayer == null) {
         targetPlayer = PlayerControl.Instance.GetPlayerBodyTransform();
      }
      var newDirection = (targetPlayer.position - transform.position).normalized;
      Vector2 slowDirction = Vector3.RotateTowards(transform.right, newDirection, Time.deltaTime * angularSpeed, 0);
      transform.right = slowDirction;
   }
}
