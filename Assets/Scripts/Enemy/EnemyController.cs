using System;
using System.Collections;
using System.Collections.Generic;
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
   private bool isAttacking = false;
   private EnemyState state;

   private void Start()
   {
      state = EnemyState.Move;
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
      return attackRange.IsInAttackRange();
   }
}
