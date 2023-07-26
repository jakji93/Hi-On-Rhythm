using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HitFlashManager : MonoBehaviour
{
   public static HitFlashManager Instance { get; private set; }

   [Range(0, 255)]
   [SerializeField] private float maxAlpha;
   [SerializeField] private float flashDuration;
   [SerializeField] private Image flashImage;

   private bool isFlashing = false;

   private void Awake()
   {
      Instance = this;
   }

   public void Flash()
   {
      if (isFlashing) return;
      flashImage.enabled = true;
      isFlashing = true;
      var color = flashImage.color;
      color.a = 0f;
      flashImage.color = color;
      var seq = DOTween.Sequence();
      seq.Append(flashImage.DOFade(maxAlpha / 255, flashDuration / 2));
      seq.Append(flashImage.DOFade(0, flashDuration / 2));
      seq.OnComplete(() => {
         isFlashing = false;
         flashImage.enabled = false;
      });
   }
}
