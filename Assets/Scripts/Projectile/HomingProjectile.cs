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
         targetPlayer = PlayerControl.Instance.transform;
      }
      var newDirection = (targetPlayer.position - transform.position).normalized;
      var slowDirction = Vector3.RotateTowards(transform.up, newDirection, Time.deltaTime * angularSpeed, 0);
      transform.up = slowDirction;
   }
}