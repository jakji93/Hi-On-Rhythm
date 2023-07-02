using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
   public static BossController Instance { get; private set; }
   [SerializeField] private Health health;

   private void Awake()
   {
      Instance = this;
   }

   public int GetBossHealth()
   {
      var curhealth = health.GetCurHealth();
      return curhealth;
   }

   public int GetBossMaxHealth()
   {
      var maxHealth = health.GetMaxHealth();
      return maxHealth;
   }
}
