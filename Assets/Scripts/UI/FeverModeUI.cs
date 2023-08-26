using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FeverModeUI : MonoBehaviour
{
   [SerializeField] private CanvasGroup feverText;
   private void Start()
   {
      FeverManager.Instance.OnFeverModeChanged += Instance_OnFeverModeChanged;
   }

   private void Instance_OnFeverModeChanged(object sender, FeverManager.OnFeverModeEventArgs e)
   {
      if(e.isFeverMode) {
         transform.DOScale(1.1f, 0.1f);
         feverText.DOFade(0f, 1f).From(0.2f).SetEase(Ease.OutCubic);
         feverText.transform.DOScale(20f, 0.5f).From(1f).SetEase(Ease.OutCubic);
      } else {
         transform.DOScale(1f, 0.1f);
      }
   }
}
