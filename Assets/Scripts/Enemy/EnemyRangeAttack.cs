using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : EnemyAttack
{
   [SerializeField] private Transform projectile;
   [SerializeField] private Transform target;

   public override void Attack()
   {

      Transform project = Instantiate(projectile, attackPoint.position, Quaternion.identity);
      Vector2 direction = target.position - attackPoint.position;
      direction.Normalize();
      project.up = direction;
   }
}
