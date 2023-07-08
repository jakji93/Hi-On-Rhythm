using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : EnemyAttack
{
   [SerializeField] private Transform projectile;

   public override void Attack()
   {
      Transform project = Instantiate(projectile, attackPoint.position, Quaternion.identity);
      Vector2 direction = PlayerControl.Instance.transform.position - attackPoint.position;
      direction.Normalize();
      project.right = direction;
   }
}
