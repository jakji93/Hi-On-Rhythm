using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyController;

public class Health : MonoBehaviour, IHasProgress
{
   public event EventHandler<OnTakeDamageEventArgs> OnTakeDamage;
   public class OnTakeDamageEventArgs : EventArgs
   {
      public int damage;
   }
   public event EventHandler<OnTakeDamageEventArgs> OnDeath;
   public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

   [SerializeField] private int maxHealth;

   private int curHealth;

   private void Awake()
   {
      curHealth = maxHealth;
   }

   public int GetMaxHealth()
   {
      return maxHealth;
   }

   public int GetCurHealth()
   {
      return curHealth;
   }

   public void TakeDamage(int damage)
   {
      if (damage <= 0) return;
      curHealth = Mathf.Max(curHealth - damage, 0);
      if(curHealth <= 0 ) {
         OnDeath?.Invoke(this, new OnTakeDamageEventArgs
         {
            damage = damage,
         });
         OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
         {
            currentValue = 0,
            maxValue = maxHealth
         });
      } else {
         OnTakeDamage?.Invoke(this, new OnTakeDamageEventArgs
         {
            damage = damage,
         });
         OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
         {
            currentValue = curHealth,
            maxValue = maxHealth
         });
      }
   }
}
