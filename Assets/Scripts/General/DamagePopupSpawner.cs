using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
   [SerializeField] private Health health;
   [SerializeField] private GameObject damagePopup;
   [SerializeField] private float popupLifeTime;
   [SerializeField] private bool isAttachedToObject = true;

   private void Start()
   {
      health.OnTakeDamage += Health_OnTakeDamage;
      health.OnDeath += Health_OnDeath;
   }

   private void Health_OnDeath(object sender, Health.OnTakeDamageEventArgs e)
   {
      SpawnPopup(e.damage);
   }

   private void Health_OnTakeDamage(object sender, Health.OnTakeDamageEventArgs e)
   {
      SpawnPopup(e.damage);
   }

   private void SpawnPopup(int damage)
   {
      GameObject damagePop;
      if (isAttachedToObject) {
         damagePop = Instantiate(damagePopup, transform.position, Quaternion.identity, transform);
      }
      else {
         damagePop = Instantiate(damagePopup, transform.position, Quaternion.identity);
      }
      damagePop.GetComponent<DamagePopupController>().SetDamageTaken(damage);
      Destroy(damagePop, popupLifeTime);
   }
}
