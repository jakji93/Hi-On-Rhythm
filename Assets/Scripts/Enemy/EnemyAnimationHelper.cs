using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHelper : MonoBehaviour
{
   [SerializeField] private EnemyAttack enemyAttack;

   public void StartAttacking()
   {
      enemyAttack.StartAttacking();
   }

   public void Attack()
   {
      enemyAttack.Attack();
   }

   public void StopAttacking()
   {
      enemyAttack.EndAttacking();
   }
}
