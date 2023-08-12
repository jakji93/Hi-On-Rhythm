using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AudioSyncerColor : AudioSyncer
{
   [SerializeField] private Color restColor;
   [SerializeField] private Color beatColor;
   [SerializeField] private SpriteRenderer spriteRenderer;

   public override void OnBeat()
   {
      base.OnBeat();
      spriteRenderer.DOKill();
      spriteRenderer.DOColor(beatColor, timeToBeat).OnComplete(() =>
      {
         spriteRenderer.DOColor(restColor, restSmoothTime).SetEase(Ease.OutQuad);
      });
   }
}
