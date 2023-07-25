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
   [SerializeField] private float damageCooldown = 0f;

   private bool canTakeDamage = true;
   private float damageTimer = 0f;

   private int curHealth;

   private void Awake()
   {
      curHealth = maxHealth;
   }

   private void Update()
   {
      damageTimer -= Time.deltaTime;
      if(damageTimer <= 0f ) {
         canTakeDamage = true;
         damageTimer = 0f;
      } else {
         canTakeDamage = false;
      }
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
      if (damage <= 0 || curHealth <= 0 || !canTakeDamage) return;
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
         damageTimer = damageCooldown;
      }
   }
}
