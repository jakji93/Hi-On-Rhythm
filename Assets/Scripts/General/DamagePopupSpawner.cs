using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
   [SerializeField] private Health health;
   [SerializeField] private GameObject damagePopup;
   [SerializeField] private float popupLifeTime;

   private void Start()
   {
      health.OnTakeDamage += Health_OnTakeDamage;
   }

   private void Health_OnTakeDamage(object sender, Health.OnTakeDamageEventArgs e)
   {
      var damagePop = Instantiate(damagePopup, transform.position, Quaternion.identity, transform);
      damagePop.GetComponent<DamagePopupController>().SetDamageTaken(e.damage);
      Destroy(damagePop, popupLifeTime);
   }
}
