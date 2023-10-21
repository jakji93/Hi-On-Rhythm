using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyRangeAttack : EnemyAttack
{
   [SerializeField] private Transform projectile;
   [SerializeField] private Transform body;

   public override void Attack()
   {
      body.DOKill();
      body.localScale = Vector3.one;
      body.DOPunchScale(new Vector3(1.3f, 1.3f, 1.3f), 0.2f, 0, 0);
      Transform project = Instantiate(projectile, attackPoint.position, Quaternion.identity);
      Vector2 direction = PlayerControl.Instance.GetPlayerBodyTransform().position - attackPoint.position;
      direction.Normalize();
      project.right = direction;
      EndAttacking();
   }

   private void OnDestroy()
   {
      body.DOKill();
   }
}
