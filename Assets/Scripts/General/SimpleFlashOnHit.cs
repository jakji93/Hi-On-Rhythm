using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFlashOnHit : MonoBehaviour
{
   [SerializeField] private Material flashMaterial;
   [SerializeField] private float duration;

   private SpriteRenderer spriteRenderer;
   private Material originalMaterial;
   private Coroutine flashRoutine;
   private Health health;

   void Start()
   {
      spriteRenderer = GetComponent<SpriteRenderer>();
      originalMaterial = spriteRenderer.material;
      health = GetComponent<Health>();
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
