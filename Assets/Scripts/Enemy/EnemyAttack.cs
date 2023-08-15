using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
   [SerializeField] protected float attackDelay = 0.1f;
   [SerializeField] protected EnemyController controller;
   [SerializeField] protected Transform attackPoint;
   [SerializeField] protected Animator animator;

   protected bool isAttacking = false;
   private bool canAttack = false;
   private float attackTimer;

   private void Start()
   {
      controller.OnStateChanged += EnemyController_OnStateChanged;
      NoteManager.Instance.OnAttackBeat += NoteManager_OnAttackBeat;
      attackTimer = attackDelay;
   }

   private void NoteManager_OnAttackBeat(object sender, System.EventArgs e)
   {
      if (!GameplayManager.Instance.IsGamePlaying()) return;
      if (canAttack && !isAttacking) {
         StartAttacking();
         Attack();
      }
   }

   private void EnemyController_OnStateChanged(object sender, EnemyController.OnStateChangedEventArgs e)
   {
      canAttack = e.state == EnemyController.EnemyState.Attack;
   }

   protected virtual void Update()
   {
      if (!GameplayManager.Instance.IsGamePlaying()) return;
      if (canAttack && !isAttacking) {
         attackTimer += Time.deltaTime;
         if (attackTimer > attackDelay) {
            attackTimer = 0f;
            animator.SetTrigger("IsAttacking");
         }
      }
   }

   public abstract void Attack();

   public void StartAttacking()
   {
      isAttacking = true;
      controller.IsAttacking(true);
   }

   public virtual void EndAttacking()
   {
      isAttacking = false;
      controller.IsAttacking(false);
   }

   private void OnDestroy()
   {
      NoteManager.Instance.OnAttackBeat -= NoteManager_OnAttackBeat;
   }
}
