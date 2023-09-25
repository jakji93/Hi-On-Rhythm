using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
   [SerializeField] protected EnemyController controller;
   [SerializeField] protected Transform attackPoint;

   protected bool isAttacking = false;
   private bool canAttack = false;

   private void Start()
   {
      controller.OnStateChanged += EnemyController_OnStateChanged;
      NoteManager.Instance.OnAttackBeat += NoteManager_OnAttackBeat;
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
