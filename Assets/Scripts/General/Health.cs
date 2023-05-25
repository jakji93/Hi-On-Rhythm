using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IHasProgress
{
   public event EventHandler OnTakeDamage;
   public event EventHandler OnDeath;
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
      curHealth = Mathf.Max(curHealth - damage, 0);
      if(curHealth <= 0 ) {
         OnDeath?.Invoke(this, EventArgs.Empty);
         OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
         {
            currentValue = 0,
            maxValue = maxHealth
         });
      } else {
         OnTakeDamage?.Invoke(this, EventArgs.Empty);
         OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
         {
            currentValue = curHealth,
            maxValue = maxHealth
         });
      }
   }
}
