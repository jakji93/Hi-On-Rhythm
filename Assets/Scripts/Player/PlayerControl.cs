using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class PlayerControl : MonoBehaviour
{
   public static PlayerControl Instance { get; private set; }  
   [SerializeField] private Health health;
   [SerializeField] private TextMeshProUGUI healthText;

   private void Awake()
   {
      Instance = this;
   }

   void Start()
   {
      health.OnDeath += Health_OnDeath;
      health.OnTakeDamage += Health_OnTakeDamage;
      NoteManager.Instance.OnNoteMissed += NoteManager_OnNoteMissed;
      NoteManager.Instance.OnNoNoteHits += NoteManager_OnNoNoteHits;
      healthText.text = health.GetMaxHealth().ToString();
   }

   private void Health_OnTakeDamage(object sender, Health.OnTakeDamageEventArgs e)
   {
      healthText.text = health.GetCurHealth().ToString();
   }

   private void NoteManager_OnNoNoteHits(object sender, System.EventArgs e)
   {
      health.TakeDamage(10);
   }

   private void NoteManager_OnNoteMissed(object sender, System.EventArgs e)
   {
      health.TakeDamage(10);
   }

   private void Health_OnDeath(object sender, System.EventArgs e)
   {
      //trigger player dead animation
      healthText.text = "0";
      GameplayManager.Instance.PlayerDead();
   }

   public int GetPlayerHealth()
   {
      var curhealth = health.GetCurHealth();
      return curhealth;
   }

   public int GetPlayerMaxHealth()
   {
      var maxHealth = health.GetMaxHealth();
      return maxHealth;
   }
}
