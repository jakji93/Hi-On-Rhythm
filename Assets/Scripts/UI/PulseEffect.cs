using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PulseEffect : MonoBehaviour
{
   public float minScale = 1f;
   public float maxScale = 1.2f;
   public float pulseDuration = 1f;
   public bool constantPulse;
   public float delayBetweenPulse = 0f;

   private RectTransform rectTransform;

   private void Start()
   {
      rectTransform = GetComponent<RectTransform>();
      if(constantPulse ) {
         StartCoroutine(ConstantPulsingEffect());
      }
   }

   public void Pulse()
   {
      StartCoroutine(PulsingEffect());
   }

   private IEnumerator PulsingEffect()
   {
      // Scale up
      yield return ScaleTo(rectTransform, maxScale, pulseDuration / 2);

      // Scale down
      yield return ScaleTo(rectTransform, minScale, pulseDuration / 2);
   }

   private IEnumerator ConstantPulsingEffect()
   {
      while (true) {
         // Scale up
         yield return ScaleTo(rectTransform, maxScale, pulseDuration / 2);

         // Scale down
         yield return ScaleTo(rectTransform, minScale, pulseDuration / 2);

         yield return new WaitForSeconds(delayBetweenPulse);
      }
   }

   private IEnumerator ScaleTo(RectTransform target, float targetScale, float duration)
   {
      Vector3 originalScale = target.localScale;
      Vector3 targetScaleVector = new Vector3(targetScale, targetScale, 1f);
      float elapsedTime = 0f;

      while (elapsedTime < duration) {
         target.localScale = Vector3.Lerp(originalScale, targetScaleVector, elapsedTime / duration);
         elapsedTime += Time.deltaTime;
         yield return null;
      }

      target.localScale = targetScaleVector;
   }
}