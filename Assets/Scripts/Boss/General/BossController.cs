using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossController : MonoBehaviour
{
   public static BossController Instance { get; private set; }
   [SerializeField] private Health health;
   [SerializeField] private TextMeshProUGUI healthText;

   private void Awake()
   {
      Instance = this;
   }

   private void Start()
   {
      health.OnTakeDamage += Health_OnTakeDamage;
      healthText.text = "100%";
   }

   private void Health_OnTakeDamage(object sender, Health.OnTakeDamageEventArgs e)
   {
      var curHealth = health.GetCurHealth();
      var maxHealth = health.GetMaxHealth();
      healthText.text = Mathf.Floor(curHealth / (float) maxHealth * 100) + "%";
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
