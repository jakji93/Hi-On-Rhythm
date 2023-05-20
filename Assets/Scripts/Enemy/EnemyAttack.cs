using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
   [SerializeField] protected float attackDelay = 0.1f;
   [SerializeField] protected EnemyController controller;
   [SerializeField] protected Transform attackPoint;

   protected bool isAttacking = false;
   private bool canAttack = false;
   private float attackTimer = 0f;

   private void Start()
   {
      controller.OnStateChanged += EnemyController_OnStateChanged;
   }

   private void EnemyController_OnStateChanged(object sender, EnemyController.OnStateChangedEventArgs e)
   {
      canAttack = e.state == EnemyController.EnemyState.Attack;
   }

   private void Update()
   {
      if (canAttack && !isAttacking) {
         attackTimer += Time.deltaTime;
         if (attackTimer > attackDelay) {
            attackTimer = 0f;
            //trigger attack animation
            Debug.Log("Attacking");
         }
      }
   }

   public abstract void Attack();

   public void StartAttacking()
   {
      isAttacking = true;
      controller.IsAttacking(true);
   }

   public void EndAttacking()
   {
      isAttacking = false;
      controller.IsAttacking(false);
   }
}
