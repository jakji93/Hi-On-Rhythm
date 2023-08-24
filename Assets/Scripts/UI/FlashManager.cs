using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FlashManager : MonoBehaviour
{
   [Range(0, 255)]
   [SerializeField] protected float maxAlpha;
   [SerializeField] protected float flashDuration;
   [SerializeField] protected Image flashImage;

   protected bool isFlashing = false;

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
