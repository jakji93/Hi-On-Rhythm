using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   public enum EnemyState
   {
      Move,
      Attack,
      Dead,
   }

   public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
   public class OnStateChangedEventArgs : EventArgs
   {
      public EnemyState state;
   }

   [SerializeField] private EnemyAttackRange attackRange;
   [SerializeField] private Health health;
   [SerializeField] private Collider2D collider;
   private bool isAttacking = false;
   private EnemyState state;

   private void Start()
   {
      health.OnDeath += Health_OnDeath;
      state = EnemyState.Move;
   }

   private void Health_OnDeath(object sender, EventArgs e)
   {
      collider.enabled = false;
      state = EnemyState.Dead;
      OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
      {
         state = state,
      });
      ScoreManager.Instance.EnemyKilled();
      //TODO death animation, set GO off for now
      gameObject.SetActive(false);
   }

   private void Update()
   {
      switch (state) {
         case EnemyState.Move:
            if(IsInAttackRange()) {
               state = EnemyState.Attack;
               OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
               {
                  state = state,
               });
            }
            break; 
         case EnemyState.Attack:
            if(!isAttacking && !IsInAttackRange()) {
               state = EnemyState.Move;
               OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
               {
                  state = state,
               });
            }
            break;
         case EnemyState.Dead:
            break;
      }
   }

   public void IsAttacking(bool isAttacking)
   {
      this.isAttacking = isAttacking;
   }

   private bool IsInAttackRange()
   {
      if(!attackRange) return false;
      return attackRange.IsInAttackRange();
   }
}
