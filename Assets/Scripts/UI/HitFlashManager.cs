using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitFlashManager : MonoBehaviour
{
   public static HitFlashManager Instance { get; private set; }

   [SerializeField] private AnimationCurve alphaCurve;
   [Range(0, 255)]
   [SerializeField] private float maxAlpha;
   [SerializeField] private float flashDuration;
   [SerializeField] private Image flashImage;

   private float timer = 0f;
   private bool isFlashing = false;

   private void Awake()
   {
      Instance = this;
   }

   private void Update()
   {
      if(isFlashing) {
         timer += Time.deltaTime;
         var normalizeTimeer = timer / flashDuration;
         var flashColor = flashImage.color;
         flashColor.a = alphaCurve.Evaluate(normalizeTimeer) * maxAlpha/255;
         flashImage.color = flashColor;
         if(timer > flashDuration) {
            isFlashing = false;
            timer = 0f;
            flashImage.enabled = false;
         }
      }
   }

   public void Flash()
   {
      if (isFlashing) return;
      flashImage.enabled = true;
      isFlashing = true;
   }
}
