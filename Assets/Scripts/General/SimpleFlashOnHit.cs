using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlashOnHit : MonoBehaviour
{
   [SerializeField] private Material flashMaterial;
   [SerializeField] private float duration;
   [SerializeField] private SpriteRenderer spriteRenderer;
   [SerializeField] private Health health;

   private Material originalMaterial;
   private Coroutine flashRoutine;
   

   void Start()
   {
      originalMaterial = spriteRenderer.material;
      if (health == null ) {
         Debug.LogError("Missing Health component");
      }
      health.OnTakeDamage += Health_OnTakeDamage;
   }

   private void Health_OnTakeDamage(object sender, System.EventArgs e)
   {
      Flash();
   }

   private void Flash()
   {
      if (flashRoutine != null) {
         StopCoroutine(flashRoutine);
      }
      flashRoutine = StartCoroutine(FlashRoutine());
   }

   private IEnumerator FlashRoutine()
   {
      spriteRenderer.material = flashMaterial;
      yield return new WaitForSeconds(duration);
      spriteRenderer.material = originalMaterial;
      flashRoutine = null;
   }
}
